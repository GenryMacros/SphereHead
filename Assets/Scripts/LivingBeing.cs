using UnityEngine;

public class LivingBeing : MonoBehaviour
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float speed;

    private float knockbackDistance;
    private float knockbackSpeed = 20;
    private Vector3 _knockbackDir;

    protected bool ProcessKnockback()
    {
        if (knockbackDistance <= 0)
        {
            knockbackDistance = 0;
            return false;
        }
        Vector3 knockbackVelocity;
        if (knockbackDistance - knockbackSpeed * Time.deltaTime < 0)
        {
            knockbackVelocity =  knockbackDistance * _knockbackDir;
        }
        else
        { 
            knockbackVelocity = knockbackSpeed * Time.deltaTime * _knockbackDir;   
        }
        transform.Translate(knockbackVelocity, Space.World);
        knockbackDistance -= knockbackSpeed * Time.deltaTime;
        return true;
    }
    
    public virtual void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir)
    {
        hp -= damage;
        _knockbackDir = new Vector3(knockbackDir.x, 0, knockbackDir.y);
        knockbackDistance = knockbackPower;
        if (hp <= 0)
        {
            //TODO
        }
    }
}
