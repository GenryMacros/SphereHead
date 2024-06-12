using UnityEngine;

public class LivingBeing : MonoBehaviour
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float knockbackRecoverTime;
    [SerializeField]
    private Timer knockbackTimer;
    
    protected bool canMove = true;
    
    private float knockbackDistance;
    private float knockbackSpeed = 30;
    private Vector3 _knockbackDir;

    protected void Start()
    {
        knockbackTimer.isLooping = false;
        knockbackTimer.waitTime = knockbackRecoverTime;
        knockbackTimer.callback += this.KnockbackRecover;
    }
    
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
        
        if (knockbackDistance <= 0)
        {
            knockbackTimer.Begin();
        }
        
        return true;
    }
    
    public virtual void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir)
    {
        hp -= damage;
        _knockbackDir = new Vector3(knockbackDir.x, 0, knockbackDir.y);
        knockbackDistance += knockbackPower;
        canMove = false;
        knockbackTimer.Stop();
    }

    void KnockbackRecover()
    {
        canMove = true;
    }
}
