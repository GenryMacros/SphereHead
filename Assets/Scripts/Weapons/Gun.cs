using System;
using UnityEngine;


public class Gun : Weapon
{
    [SerializeField]
    protected int maxAmmo;
    [SerializeField]
    protected int ammo;
    [SerializeField]
    protected Bullet bulletPrefab;
    [SerializeField]
    protected bool isInfiniteAmmo;
    
    public float projectileScaleMultiplier = 1.0f;
    public float bulletSpeed;
    public float maxBulletTravelDistance;
    public event Action ammoChanged;
    
    
    public override void Fire()
    {
        if (isReadyToFire)
        {
            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.source = owner;
            newBullet.transform.position = spawnPoint.transform.position;
            newBullet.Init(bulletSpeed, gameObject.transform.forward, damage, knockbackPower, maxBulletTravelDistance);
            newBullet.transform.localScale *= projectileScaleMultiplier;
            if (!isInfiniteAmmo)
            {
                ammo = Math.Max(ammo - 1, 0);   
                ammoChanged?.Invoke();
            }
        }
        base.Fire();
    } 

    public override void ApplyUpgrade(Upgrade upgrade)
    {
        if (!isReady)
        {
            isReady = true;
            ammo = maxAmmo;
            return;
        }
        
        WeaponUpgrade newUpgrade = upgrade.upgradeParameters[0];
        rateOfFire -= rateOfFire * newUpgrade.rateOfFireChangePercent;
        damage += damage * newUpgrade.damageChangePercent;
        knockbackPower += knockbackPower * newUpgrade.knockbackPowerChangePercent;
        bulletSpeed += bulletSpeed * newUpgrade.bulletSpeedChangePercent;
        maxBulletTravelDistance +=maxBulletTravelDistance * newUpgrade.maxBulletTravelDistanceChangePercent;
        maxAmmo += newUpgrade.maxAmmoIncrement;
        
        ammo = maxAmmo;
    }

    public void ReplenishAmmo(int increment)
    {
        ammo = Math.Min(ammo + increment, maxAmmo);
        ammoChanged?.Invoke();
    }
    
    public int GetAmmoCount()
    {
        return isInfiniteAmmo ? -1 : ammo;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }

    public bool IsInfiniteAmmo()
    {
        return isInfiniteAmmo;
    }
}
