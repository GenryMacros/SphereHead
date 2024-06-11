using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    public float _damage;
    public float _knockbackPower;
    private float _speed = 0;

    public void Init(float speed, Vector3 direction, float damage, float knockbackPower)
    {
        _speed = speed;
        _direction = new Vector2(direction.x, direction.z);
        _knockbackPower = knockbackPower;
        _damage = damage;
    }

    void FixedUpdate()
    {
        Vector2 velocity = _speed * Time.deltaTime * _direction;
        transform.Translate(new Vector3(velocity.x, 0, velocity.y));   
    }
    
    private void OnTriggerEnter(Collider other)
    {
        LivingBeing hitBeing = other.gameObject.GetComponent<LivingBeing>();
        if (hitBeing)
        {
            hitBeing.TakeDamage(_damage, _knockbackPower, _direction);
            Destroy(gameObject);
        }
    }
}
