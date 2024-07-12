using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum SupplyType
{
    SpecificGunSupply = 0,
    GunsSupply = 1,
    HealthSupply = 2,
}


public class Supply : MonoBehaviour
{
    [SerializeField] 
    private float gunsSupplyRestorePercent = 0.2f;
    [SerializeField] 
    private float gunSupplyRestorePercent = 0.5f;
    [SerializeField] 
    private float healthRestorePercent = 0.3f;
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        Dictionary<SupplyType, float> type2ChooseChance = new Dictionary<SupplyType, float>()
        {
            {SupplyType.HealthSupply, 0.0f},
            {SupplyType.SpecificGunSupply, 0.0f},
            {SupplyType.GunsSupply, 0.0f},
        };
        
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        List<Weapon> arsenal = players[0].GetActiveArsenal();
        
        float meanPlayersHealth = 0.0f;
        float maxPlayerHealth = players[0].GetMaxHP();
        
        foreach (PlayerController player in players)
        {
            meanPlayersHealth += player.GetHP();
        }

        meanPlayersHealth /= players.Length;

        type2ChooseChance[SupplyType.HealthSupply] = Mathf.Lerp(1.5f, 0.1f, meanPlayersHealth / maxPlayerHealth);
        
        if (arsenal.Count == 1)
        {
            foreach (PlayerController player in players)
            {
                player.Heal(player.GetMaxHP() * healthRestorePercent);
            }
        }
        else
        {
            int totalMaxAmmo = 0;
            int currenTotalAmmo = 0;
            float currentMinAmmoPercent = 1.0f;
            Gun currentMinGun = null;
            foreach (Weapon weapon in arsenal)
            {
                Gun cast = weapon as Gun;
                if (cast)
                {
                    int currentGunMaxAmmo = cast.GetMaxAmmo();
                    if (cast.IsInfiniteAmmo())
                    {
                        continue;
                    }
                    int currentGunActualAmmo = cast.GetAmmoCount();
                    float ammoLeftPercent = currentGunActualAmmo / (float)currentGunMaxAmmo;

                    if (ammoLeftPercent <= 0.5)
                    {
                        type2ChooseChance[SupplyType.GunsSupply] += 0.1f;
                    }
                    
                    if (ammoLeftPercent < currentMinAmmoPercent)
                    {
                        currentMinAmmoPercent = ammoLeftPercent;
                        currentMinGun = cast;
                    }
                }
            }

            if (currentMinAmmoPercent <= 0.2)
            {
                type2ChooseChance[SupplyType.SpecificGunSupply] = 0.5f;
            }

            SupplyType supplyType = (SupplyType)GetRandomWeightedIndex(new float[]
            {
                type2ChooseChance[SupplyType.SpecificGunSupply],
                type2ChooseChance[SupplyType.GunsSupply],
                type2ChooseChance[SupplyType.HealthSupply]
            });

            switch (supplyType)
            {
                case SupplyType.SpecificGunSupply:
                    currentMinGun.ReplenishAmmo((int)(currentMinGun.GetMaxAmmo() * gunSupplyRestorePercent));
                    break;
                case SupplyType.GunsSupply:
                    foreach (Weapon weapon in arsenal)
                    {
                        Gun cast = weapon as Gun;
                        if (cast)
                        {
                            cast.ReplenishAmmo((int)(cast.GetMaxAmmo() * gunsSupplyRestorePercent));
                        }
                    }
                    break;
                case SupplyType.HealthSupply:
                    PlayerController player = other.gameObject.GetComponent<PlayerController>();
                    player.Heal(player.GetMaxHP() * healthRestorePercent);
                    break;
            }
        }
        Destroy(gameObject);
    }
    
    private int GetRandomWeightedIndex(float[] weights)
    {
        float weightSum = 0f;
        foreach (float weight in weights)
        {
            weightSum += weight;
        }
        
        int index = weights.Length - 1;
        while (index > 0)
        {
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }
            weightSum -= weights[index];
            index -= 1;
        }
        return index;
    }
}
