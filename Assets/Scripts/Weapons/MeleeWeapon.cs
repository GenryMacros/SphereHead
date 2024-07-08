using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] 
    private BoxCollider _hitBox;
    private List<Collider> _colliders = new List<Collider>();

    void Start()
    {
        base.Start();
    }
    
    public override void Fire()
    {
        if (isReadyToFire)
        {
            foreach (Collider col in _colliders)
            {
                if (!col)
                {
                    continue;
                }
                LivingBeing beingInRange = col.gameObject.GetComponent<LivingBeing>();
                if (beingInRange)
                {
                    beingInRange.TakeDamage(damage, knockbackPower, 
                                new Vector2(transform.forward.x, transform.forward.z), owner);
                }
            }   
        }
        base.Fire();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!_colliders.Contains(other))
        {
            _colliders.Add(other);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        _colliders.Remove(other);
    }
    
    public override void ResetSpawnPoint(GameObject newSpawnPoint)
    {
        _hitBox.transform.position = newSpawnPoint.transform.position;
    }
    
}
