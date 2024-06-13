
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
}
