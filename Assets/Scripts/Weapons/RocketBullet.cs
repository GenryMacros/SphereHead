using UnityEngine;


public class RocketBullet : Bullet { 
    
    [SerializeField]
    protected float explosionRange;
    
    
    protected override void OnTriggerEnter(Collider other)
    {
        Collider[] collidingInRadius = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider col in collidingInRadius)
        {
            LivingBeing hitBeing = col.gameObject.GetComponent<LivingBeing>();       
            
            if (hitBeing)
            {
                Vector3 dirVector;
                if (col != other)
                {
                    dirVector = hitBeing.transform.position - transform.position;
                    dirVector.Normalize();
                }
                else
                {
                    dirVector = new Vector3(_direction.x, 0, _direction.y);
                }

                float distance2Target = Vector3.Distance(transform.position, hitBeing.transform.position);
                float rangeDependentKnockback = Mathf.Lerp(explosionRange, _knockbackPower, distance2Target / explosionRange);
                float rangeDependentDamage = Mathf.Lerp(_damage, _damage * 0.2f, distance2Target / explosionRange);
                
                hitBeing.TakeDamage(rangeDependentDamage, rangeDependentKnockback, new Vector2(dirVector.x, dirVector.z));
                SelfDestruct();
            }
        }
    }
}
