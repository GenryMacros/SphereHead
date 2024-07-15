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
    public event Action upgradeInstalled;
    
    
    public override void Fire()
    {
        if (ammo == 0 && !isInfiniteAmmo)
        {
            // TODO
            return;
        }
        
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
        InvokeUpgradeInstall();
        if (!isReady)
        {
            isReady = true;
            ammo = maxAmmo;
            return;
        }
        
        WeaponUpgrade newUpgrade = upgrade.upgradeParameters[0];
        rateOfFire -= newUpgrade.rateOfFireChange;
        damage += newUpgrade.damageChange;
        knockbackPower += newUpgrade.knockbackPowerChange;
        bulletSpeed += newUpgrade.bulletSpeedChange;
        maxBulletTravelDistance += newUpgrade.maxBulletTravelDistanceChange;
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

    protected void InvokeUpgradeInstall()
    {
        upgradeInstalled?.Invoke();
    }
}
