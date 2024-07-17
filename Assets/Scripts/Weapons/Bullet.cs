using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    public float _damage;
    public float _knockbackPower;
    public OwnerEntity source = OwnerEntity.Player;
    
    [SerializeField]
    protected ParticleSystem _destructionParticles;
    
    protected Vector2 _direction = Vector2.zero;
    protected float _speed = 0;
    protected float _maxBulletTravelDistance = 0;
    protected float _traveledDistance = 0;

    private void Start()
    {
        if (_destructionParticles)
        {
            _destructionParticles.Stop();   
        }
    }

    public void Init(float speed, Vector3 direction, float damage, float knockbackPower, float maxBulletTravelDistance)
    {
        _speed = speed;
        _direction = new Vector2(direction.x, direction.z);
        _knockbackPower = knockbackPower;
        _damage = damage;
        _maxBulletTravelDistance = maxBulletTravelDistance;

        if (_destructionParticles)
        {
            _destructionParticles.Stop();   
        }
    }
    
    protected virtual void FixedUpdate()
    {
        Vector2 velocity = _speed * Time.deltaTime * _direction;
        transform.Translate(new Vector3(velocity.x, 0, velocity.y));
        _traveledDistance += velocity.magnitude;

        if (_traveledDistance >= _maxBulletTravelDistance)
        {
            gameObject.SetActive(false);
            SelfDestruct();
        }
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (source == OwnerEntity.Enemy && gameObject.CompareTag("boss"))
        {
            return;
        }
        
        LivingBeing hitBeing = other.gameObject.GetComponent<LivingBeing>();
        if (hitBeing)
        {
            hitBeing.TakeDamage(_damage, _knockbackPower, _direction, source);
            SelfDestruct();
        } else if (other.gameObject.CompareTag("wall"))
        {
            _speed = 0;
            if (_destructionParticles)
            {
                _destructionParticles.Play();
                StartCoroutine(DestructWithTimeout());
            }
            else
            {
                SelfDestruct();
            }
        }
    }

    IEnumerator DestructWithTimeout()
    {
        yield return new WaitForSeconds(0.2f);
        SelfDestruct();
    }
    
    protected virtual void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
