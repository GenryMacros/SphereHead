
using UnityEngine;

public class Gun : Weapon
{
    public float projectileScaleMultiplier = 1.0f;
    
    void Start()
    {
        base.Start();
    }
    
    public override void Fire()
    {
        if (isReadyToFire)
        {
            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = spawnPoint.transform.position;
            newBullet.Init(bulletSpeed, gameObject.transform.forward, damage, knockbackPower, maxBulletTravelDistance);
            newBullet.transform.localScale *= projectileScaleMultiplier;
        }
        base.Fire();
    } 
    
    void FixedUpdate()
    {
        
    }
    
    public override void ApplyUpgrade(Upgrade upgrade)
    {
        WeaponUpgrade newUpgrade = JsonUtility.FromJson<WeaponUpgrade>(upgrade.upgradeParameters);
        rateOfFire += rateOfFire * newUpgrade.rateOfFireChangePercent;
        damage += damage * newUpgrade.damageChangePercent;
        knockbackPower += knockbackPower * newUpgrade.knockbackPowerChangePercent;
        bulletSpeed += bulletSpeed * newUpgrade.bulletSpeedChangePercent;
        maxBulletTravelDistance +=maxBulletTravelDistance * newUpgrade.maxBulletTravelDistanceChangePercent;
        maxAmmo += newUpgrade.maxAmmoIncrement;

        ammo = maxAmmo;
    }
}
