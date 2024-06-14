using System;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(CapsuleCollider))]
public class RegularEnemy : LivingBeing
{
    public enum EnemyState {Chase, Attack, Die};
    public EnemyState currentState = EnemyState.Chase;
    public float minDamage;
    public float maxDamage;
    public float maxHp;
    public int scoreWorth;
    
    [SerializeField]
    protected NavMeshAgent _navigator;
    protected float damage;
    
    [SerializeField]
    protected float attackDistance;
    
    [SerializeField] 
    protected Timer pathResetTimer;
    
    
    protected override void Start()
    {
        base.Start();
        float t = (float)GameController.instance.GetCurrentWave() / GameController.instance.GetMaxWaves();
        damage = Mathf.Lerp(minDamage, maxDamage, t);
        hp = Mathf.Lerp(hp, maxHp, t);

        pathResetTimer.waitTime = 2;
        pathResetTimer.isLooping = true;
        pathResetTimer.callback = ResetPath;
    }
    
    protected virtual void FixedUpdate()
    {
        bool isKnockbacked = ProcessKnockback();
        if (!isKnockbacked && canMove)
        {
            switch (currentState)
            {
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Die:
                    break;
            }   
        }
        else
        {
            _navigator.isStopped = true;
            _navigator.velocity = Vector3.zero;
        }
    }
    
    protected virtual void Chase()
    {
        SetRotation(_navigator.transform.eulerAngles.y);
        _navigator.transform.localPosition = Vector3.zero;
        transform.position += _navigator.speed * Time.deltaTime * transform.forward;
        
        Transform closestPlayer = FindClosestPlayer();
        float distance2Player = Vector3.Distance(transform.position, closestPlayer.position);
        if (distance2Player <= attackDistance)
        {
            currentState = EnemyState.Attack;
            _navigator.isStopped = true;
            _navigator.velocity = Vector3.zero;
            pathResetTimer.Stop();
        } else if (pathResetTimer.IsStopped())
        {
            pathResetTimer.Begin();
        }
        
    }

    protected virtual void Attack()
    {
        Transform closestPlayer = FindClosestPlayer();
        float distance2Player = Vector3.Distance(transform.position, closestPlayer.position);
        if (distance2Player > attackDistance)
        {
            currentState = EnemyState.Chase;
            pathResetTimer.Begin();
        }
        
        gameObject.transform.LookAt(closestPlayer);
    }
    
    protected void SetRotation(float newRotation)
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
            GameController.instance.EnemyDeath(this, scoreWorth);
            GetComponent<CapsuleCollider>().enabled = false;
            _navigator.isStopped = true;
            currentState = EnemyState.Die;
        }
        else
        {
            GameController.instance.HaltScoreDecrease();
        }
    }

    protected Transform FindClosestPlayer()
    {
        float minDistance = Vector3.Distance(transform.position, GameController.instance.players[0].transform.position);
        Transform bestChoice = GameController.instance.players[0].transform;
        foreach (PlayerController player in GameController.instance.players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                bestChoice = player.transform;
            }
        }

        return bestChoice.transform;
    }

    private void ResetPath()
    {
        _navigator.isStopped = false;
        _navigator.SetDestination(FindClosestPlayer().position);
    }
}
