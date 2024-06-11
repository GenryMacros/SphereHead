using UnityEngine;

public class Gun : Weapon
{
    
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
            newBullet.Init(bulletSpeed, gameObject.transform.forward, damage, knockbackPower);
        }
        base.Fire();
    } 
    
    void FixedUpdate()
    {
        
    }
}
