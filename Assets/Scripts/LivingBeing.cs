using UnityEngine;


public enum OwnerEntity
{
    Enemy = 0,
    Player = 1
}


public class LivingBeing : MonoBehaviour
{
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float knockbackRecoverTime;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    private Timer knockbackTimer;
    
    protected bool canMove = true;
    
    private float knockbackDistance;
    private float knockbackSpeed = 30;
    private Vector3 _knockbackDir;

    protected virtual void Start()
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
    
    public virtual void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir, OwnerEntity damageCauser)
    {
        hp -= (int)damage;
        _knockbackDir = new Vector3(knockbackDir.x, 0, knockbackDir.y);
        knockbackDistance += knockbackPower;
        canMove = false;
        _animator.enabled = false;
        knockbackTimer.Stop();
    }

    void KnockbackRecover()
    {
        canMove = true;
        _animator.enabled = true;
    }
}
