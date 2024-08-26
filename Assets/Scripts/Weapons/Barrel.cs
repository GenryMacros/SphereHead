using UnityEngine;
using UnityEngine.UI;


public class Barrel : MonoBehaviour
{
    [SerializeField]
    protected float explosionRange;
    
    [SerializeField]
    protected ParticleSystem _destructionParticles;

    [SerializeField] 
    protected float cooldown;
    
    [SerializeField] 
    protected float cooldownIncrement;

    [SerializeField] 
    protected Image cooldownCircle;
    
    [SerializeField] 
    protected GameObject body;
    
    private float _timeOnCooldown;
    private bool _isReady = true;
    
    public float damage;
    public float knockbackPower;
    public OwnerEntity source = OwnerEntity.Environment;
    
    void Start()
    {
        if (_destructionParticles)
        {
            _destructionParticles.Stop();   
        }
    }
    
    void FixedUpdate()
    {
        if (!_isReady)
        {
            _timeOnCooldown += Time.deltaTime;
            if (_timeOnCooldown > cooldown)
            {
                _isReady = true;
                cooldown += cooldownIncrement;
                body.SetActive(true);
                return;
            }
            float t = _timeOnCooldown / cooldown;
            cooldownCircle.fillAmount = t;

        }
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (!_isReady)
        {
            return;
        }
        
        if (other.gameObject.CompareTag("bullet"))
        {
            Collider[] collidingInRadius = Physics.OverlapSphere(transform.position, explosionRange);
            foreach (Collider col in collidingInRadius)
            {
                LivingBeing colObject = col.gameObject.GetComponent<LivingBeing>();
                if (colObject)
                {
                    Vector3 dirVector;
                    dirVector = colObject.transform.position - transform.position;
                    dirVector.Normalize();

                    float distance2Target = Vector3.Distance(transform.position, colObject.transform.position);
                    float rangeDependentKnockback = Mathf.Lerp(explosionRange, knockbackPower, 
                        distance2Target / explosionRange);

                    colObject.TakeDamage(damage, rangeDependentKnockback,
                        new Vector2(dirVector.x, dirVector.z), source);
                }
            }
            _destructionParticles.Play();
            GoOnCooldown();
        }
    }

    private void GoOnCooldown()
    {
        _timeOnCooldown = 0;
        _isReady = false;
        cooldownCircle.fillAmount = 0;
        body.SetActive(false);
        Invoke(nameof(StopParticles), 0.7f);
    }

    private void StopParticles()
    {
        _destructionParticles.Stop();  
    }
}
