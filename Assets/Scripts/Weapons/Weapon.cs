using UnityEngine;


public class Weapon : MonoBehaviour
{
    public float rateOfFire;
    public float damage;
    public float knockbackPower;
    public float bulletSpeed;
    public float maxBulletTravelDistance;
    
    [SerializeField]
    protected bool isReady = false;
    
    protected bool isReadyToFire = true;
    
    [SerializeField]
    protected int ammo;
    [SerializeField]
    protected Bullet bulletPrefab;
    [SerializeField]
    protected GameObject spawnPoint;
    [SerializeField]
    protected string weaponName;
    
    protected void Start()
    {
        
    }

    public virtual void Fire()
    {
        if (isReadyToFire)
        {
            isReadyToFire = false;
            Invoke(nameof(MakeReadyToFire), rateOfFire);
        }
    }
    
    void MakeReadyToFire()
    {
        isReadyToFire = true;
    }
    
    public int GetAmmoCount()
    {
        return ammo;
    }
    
    public string GetWeaponName()
    {
        return isReady ? weaponName: "?????";
    }

    public bool IsReady()
    {
        return isReady;
    }

    public void ResetSpawnPoint(GameObject newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
