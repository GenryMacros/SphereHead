using UnityEngine;

public class ZoneWeapon : Gun
{
    [SerializeField] 
    protected int zonesCount;
    [SerializeField] 
    protected float maxSpawnRadius;
    
    
    public override void Fire()
    {
        if (isReadyToFire)
        {
            Vector3 gunPos = transform.position;
            for (int i = 0; i < zonesCount; i++)
            {
                Vector2 zonePos = new Vector2(Random.Range(gunPos.x - maxSpawnRadius, gunPos.x + maxSpawnRadius),
                                            Random.Range(gunPos.z - maxSpawnRadius, gunPos.z + maxSpawnRadius));
                Bullet newBullet = Instantiate(bulletPrefab);
                
                newBullet.transform.position = new Vector3(zonePos.x, 0, zonePos.y);
                newBullet.Init(0, Vector3.zero, damage, knockbackPower, 0);
                newBullet.transform.localScale *= projectileScaleMultiplier;
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
