using UnityEngine;


public class EnemyBullet : Bullet { 
    
    
    protected override void OnTriggerEnter(Collider other)
    {
        LivingBeing hitBeing = other.gameObject.GetComponent<LivingBeing>();
        if (hitBeing)
        {
            hitBeing.TakeDamage(_damage, _knockbackPower, _direction, source);
            SelfDestruct();
        }
    }
}
