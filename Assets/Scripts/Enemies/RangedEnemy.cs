using System.Collections;
using UnityEngine;

public class RangedEnemy : RegularEnemy
{
    [SerializeField]
    protected GameObject _supplyDrop;
    
    public float minShootDistance;
    public float maxShootDistance;
    
    public float minProjectileSize;
    public float maxProjectileSize;
    
    public float minBulletSpeed;
    public float maxBulletSpeed;
    

    protected override void Start()
    {
        base.Start();
        pathResetTimer.waitTime = 0.5f;  
    }
    
    public override void SetGun(Weapon gun)
    {
        float t = (float)GameController.instance.GetCurrentWave() / GameController.instance.GetMaxWaves();
        _gun = gun;
        attackDistance = Mathf.Lerp(minShootDistance, maxShootDistance, t);
        
        _gun.ResetSpawnPoint(spawnPoint);
        Gun cast = _gun as Gun;
        if (cast)
        {
            cast.projectileScaleMultiplier = Mathf.Lerp(minProjectileSize, maxProjectileSize, t);
            cast.bulletSpeed = Mathf.Lerp(minBulletSpeed, maxBulletSpeed, t);
            cast.maxBulletTravelDistance = attackDistance;
            _gun.isReadyToFire = true;
        }
        _gun.rateOfFire = Mathf.Lerp(minFireRate, maxFireRate, t);
        _gun.damage = damage;
        _gun.knockbackPower = Mathf.Lerp(minKnockbackPower, maxKnockbackPower, t);
    }

    protected override void Attack()
    {
        Transform closestPlayer = FindClosestPlayer();
        float distance2Player = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
                                                 new Vector2(closestPlayer.position.x, closestPlayer.position.z));

        if (distance2Player > attackDistance)
        {
            currentState = EnemyState.Chase;
            pathResetTimer.Begin();
            _animator.SetBool("IsShooting",  false);
        }
        
        gameObject.transform.LookAt(closestPlayer);
        SetRotation(gameObject.transform.eulerAngles.y);
        
        _gun.Fire();
    }
    
    public override void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir, OwnerEntity damageCauser)
    {
        base.TakeDamage(damage, knockbackPower, knockbackDir, damageCauser);
        if (hp <= 0 && _supplyDrop)
        {
            Vector3 supplyBoxPosition = transform.position;
            supplyBoxPosition.y = 0;
            GameObject supplyBox = Instantiate(_supplyDrop, transform.parent);

            supplyBox.transform.position = supplyBoxPosition;
            Destroy (gameObject, 3.0f); 
        }
    }
    
    
}
