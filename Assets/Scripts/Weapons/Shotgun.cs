using System;
using UnityEngine;


[System.Serializable]
public struct ShotgunUpgrade
{
    public float rateOfFireChangePercent;
    public float damageChangePercent;
    public float knockbackPowerChangePercent;
    public float bulletSpeedChangePercent;
    public float maxBulletTravelDistanceChangePercent;  
    public float spreadConeAngleChangePercent;
    public int bulletsPerShotChange;
    public int maxAmmoIncrement;
}


public class Shotgun : Gun
{
    [SerializeField]
    protected float spreadConeAngle;
    [SerializeField]
    protected float bulletsPerShot;


    public override void Fire()
    {
        if (isReadyToFire)
        {
            float halfSpreadAngle = spreadConeAngle / 2.0f;
            float angleStep = spreadConeAngle / bulletsPerShot;
            for (int i = 0; i < bulletsPerShot; i++)
            {
                float angle = (-halfSpreadAngle + angleStep * i) * (MathF.PI/180);
                Vector3 forwardVector = transform.forward;
               
                double dirX = forwardVector.x * Math.Cos(angle) - forwardVector.z * Math.Sin(angle);
                double dirY = forwardVector.x * Math.Sin(angle) + forwardVector.z * Math.Cos(angle);
                
                Vector3 bulletDirection = new Vector3((float)dirX, forwardVector.y, (float)dirY);
                
                Bullet newBullet = Instantiate(bulletPrefab);
                newBullet.transform.position = spawnPoint.transform.position;
                newBullet.Init(bulletSpeed, bulletDirection, damage, knockbackPower, maxBulletTravelDistance);
            }
        }
        base.Fire();
    } 
    
    public override void ApplyUpgrade(Upgrade upgrade)
    {
        ShotgunUpgrade newUpgrade = JsonUtility.FromJson<ShotgunUpgrade>(upgrade.upgradeParameters);
        rateOfFire += rateOfFire * newUpgrade.rateOfFireChangePercent;
        damage += damage * newUpgrade.damageChangePercent;
        knockbackPower += knockbackPower * newUpgrade.knockbackPowerChangePercent;
        bulletSpeed += bulletSpeed * newUpgrade.bulletSpeedChangePercent;
        maxBulletTravelDistance +=maxBulletTravelDistance * newUpgrade.maxBulletTravelDistanceChangePercent;
        maxAmmo += newUpgrade.maxAmmoIncrement;
        spreadConeAngle += spreadConeAngle * newUpgrade.spreadConeAngleChangePercent;
        bulletsPerShot += newUpgrade.bulletsPerShotChange;
        
        ammo = maxAmmo;
    }
}