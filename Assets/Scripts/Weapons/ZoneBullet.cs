using UnityEngine;

public class ZoneBullet : Bullet
{
    [SerializeField] 
    protected float zoneRadius;
    [SerializeField] 
    protected float zoneActivationTime;
    [SerializeField] 
    protected float zoneDuration;
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    protected override void OnTriggerEnter(Collider other) { }
}
