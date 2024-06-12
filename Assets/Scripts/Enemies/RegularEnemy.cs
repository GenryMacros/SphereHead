using System;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(CapsuleCollider))]
public class RegularEnemy : LivingBeing
{
    public enum EnemyState {Chase, Attack};
    public PlayerController player;
    public EnemyState currentState = EnemyState.Chase;
    
    [SerializeField]
    protected NavMeshAgent _navigator;
    
    void Start()
    {
        base.Start();
    }
    
    void FixedUpdate()
    {
        bool isKnockbacked = ProcessKnockback();
        if (!isKnockbacked && canMove)
        {
            _navigator.isStopped = false;
            switch (currentState)
            {
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
            }   
        }
        else
        {
            _navigator.isStopped = true;
            _navigator.velocity = Vector3.zero;
        }
    }
    
    private void Chase()
    {
        _navigator.SetDestination(player.transform.position);
        SetRotation(_navigator.transform.eulerAngles.y);
        transform.position += _navigator.speed * Time.deltaTime * transform.forward;
        _navigator.transform.localPosition = Vector3.zero;
    }

    private void Attack()
    {
        
    }
    
    void SetRotation(float newRotation)
    {
        double roundedRotation = Math.Round(newRotation / 45.0f, MidpointRounding.AwayFromZero);
        roundedRotation *= 45.0f;
        gameObject.transform.eulerAngles = new Vector3(0, (float)roundedRotation, 0);
    }
    
    public override void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir)
    {
        base.TakeDamage(damage, knockbackPower, knockbackDir);
        if (hp <= 0)
        {
            //TODO
        }
    }
}
