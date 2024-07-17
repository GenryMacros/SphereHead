using System;
using UnityEngine;

public class CircularGun : Gun
{
    [SerializeField] 
    protected float bulletsAngleGap;
    

    public override void Fire()
    {
        if (isReadyToFire)
        {
            Vector3 spawnPointPos = spawnPoint.transform.localPosition;
            spawnPointPos.y = 0;

            float circleRadius = Vector3.Distance(spawnPointPos, Vector3.zero);
            int bulletsPerShot = (int)(360.0f / bulletsAngleGap);
            
            
            for (int i = 0; i < bulletsPerShot; i++)
            {
                float angle = (bulletsAngleGap * i) * (MathF.PI / 180);
                
                Vector3 forwardVector = transform.forward;
                
                float dirX = (float)(forwardVector.x * Math.Cos(angle) - forwardVector.z * Math.Sin(angle));
                float dirY = (float)(forwardVector.x * Math.Sin(angle) + forwardVector.z * Math.Cos(angle));
                
                Vector3 bulletDirection = new Vector3(dirX, forwardVector.y, dirY);
                Vector3 bulletPos = new Vector3(transform.position.x + circleRadius * dirX, 
                                                0, 
                                                transform.position.z + circleRadius * dirY);
                
                Bullet newBullet = Instantiate(bulletPrefab);
                newBullet.transform.position = bulletPos;
                newBullet.transform.localScale *= projectileScaleMultiplier;
                newBullet.Init(bulletSpeed, bulletDirection, damage, knockbackPower, maxBulletTravelDistance);
            }
            isReadyToFire = false;
            if (_particles)
            {
                _particles.Play();
            }
            Invoke(nameof(MakeReadyToFire), rateOfFire);
        }
    } 
}
