using UnityEngine;

public class RangedEnemy : RegularEnemy
{
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
        
        _gun.ResetSpawnPoint(spawnPoint);
        if (gun is Gun)
        {
            ((Gun)_gun).projectileScaleMultiplier = Mathf.Lerp(minProjectileSize, maxProjectileSize, t);   
        }
        _gun.rateOfFire = Mathf.Lerp(minFireRate, maxFireRate, t);
        _gun.damage = damage;
        _gun.knockbackPower = Mathf.Lerp(minKnockbackPower, maxKnockbackPower, t);
        _gun.bulletSpeed = Mathf.Lerp(minBulletSpeed, maxBulletSpeed, t);
        attackDistance = Mathf.Lerp(minShootDistance, maxShootDistance, t);
        _gun.maxBulletTravelDistance = attackDistance;
    }

    protected override void Attack()
    {
        Transform closestPlayer = FindClosestPlayer();
        float distance2Player = Vector3.Distance(transform.position, closestPlayer.position);
        if (distance2Player > attackDistance)
        {
            currentState = EnemyState.Chase;
            pathResetTimer.Begin();
        }
        
        gameObject.transform.LookAt(closestPlayer);
        SetRotation(gameObject.transform.eulerAngles.y);
        
        _gun.Fire();
    }
}
