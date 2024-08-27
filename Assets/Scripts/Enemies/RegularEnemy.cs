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
    
    public float minFireRate;
    public float maxFireRate;
    
    public float minKnockbackPower;
    public float maxKnockbackPower;
    
    public BoxCollider spawnedBox;
    public int maxHp;
    public int scoreWorth;
    public AudioSource deathSound;
    
    [SerializeField]
    protected NavMeshAgent _navigator;
    [SerializeField]
    protected float attackDistance;
    [SerializeField] 
    protected Timer pathResetTimer;
    [SerializeField] 
    protected GameObject spawnPoint;
    
    protected float damage;
    protected Weapon _gun;
    
    private BoxCollider _hitBox;
    private float _timeWithoutMoving;
    private Timer _stuckTimer;
    
    protected override void Start()
    {
        base.Start();
        float t = (float)(GameController.instance.GetCurrentWave() - 1) / GameController.instance.GetMaxWaves();
        hp = (int)Mathf.Lerp(hp, maxHp, t);
        damage = (int)Mathf.Lerp(minDamage, maxDamage, t);
        
        pathResetTimer.waitTime = 1;
        pathResetTimer.isLooping = true;
        pathResetTimer.callback = ResetPath;
        _hitBox = GetComponent<BoxCollider>();
        ResetPath();

        _stuckTimer = spawnPoint.AddComponent<Timer>();
        _stuckTimer.isLooping = false;
        _stuckTimer.waitTime = 20.0f;
        _stuckTimer.callback = TpIfStuck;
        _stuckTimer.Begin();
    }
    
    public virtual void SetGun(Weapon gun)
    {
        
        float t = (float)(GameController.instance.GetCurrentWave() - 1) / GameController.instance.GetMaxWaves();
        _gun = gun;
        damage = Mathf.Lerp(minDamage, maxDamage, t);
        _gun.ResetSpawnPoint(spawnPoint);
        
        _gun.rateOfFire = Mathf.Lerp(minFireRate, maxFireRate, t);
        _gun.damage = damage;
        _gun.knockbackPower = Mathf.Lerp(minKnockbackPower, maxKnockbackPower, t);
    }
    
    public BoxCollider GetMeleeHitBox()
    {
        return _hitBox;
    }
    
    protected virtual void FixedUpdate()
    {
        if (GameController.instance.IsGamePaused())
        {
            return;
        }
        
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
        Transform closestPlayer = FindClosestPlayer();
        float distance2Player = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
                                                 new Vector2(closestPlayer.position.x, closestPlayer.position.z));
        
        if (distance2Player <= attackDistance * 1.5)
        {
            _animator.SetBool("IsStartingToShoot",  true);
        }
        else
        {
            _animator.SetBool("IsStartingToShoot",  false);
        }
        
        if (distance2Player <= attackDistance)
        {
            _timeWithoutMoving = 0;
            currentState = EnemyState.Attack;
            _navigator.isStopped = true;
            _navigator.velocity = Vector3.zero;
            pathResetTimer.Stop();
            _animator.SetBool("IsShooting", true);
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
            _timeWithoutMoving = 0;
            pathResetTimer.Begin();
            _animator.SetBool("IsShooting",  false);
        }
        
        gameObject.transform.LookAt(closestPlayer);
        SetRotation(gameObject.transform.eulerAngles.y);
        _gun.Fire();
    }
    
    protected void SetRotation(float newRotation)
    {
        double roundedRotation = Math.Round(newRotation / 45.0f, MidpointRounding.AwayFromZero);
        roundedRotation *= 45.0f;
        gameObject.transform.eulerAngles = new Vector3(0, (float)roundedRotation, 0);
    }
    
    public override void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir, OwnerEntity damageCauser)
    {
        base.TakeDamage(damage, knockbackPower, knockbackDir, damageCauser);
        if (hp <= 0)
        {
            _animator.enabled = true;
            if (currentState != EnemyState.Die)
            {
                GameController.instance.EnemyDeath(this, scoreWorth);   
            }
            GetComponent<CapsuleCollider>().enabled = false;
            _navigator.isStopped = true;
            currentState = EnemyState.Die;
            _animator.SetBool("IsDead",  true);
            deathSound.Play();
            Destroy (gameObject, 3.0f); 
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

    private void TpIfStuck()
    {
        if (spawnedBox)
        {
            Vector3 selfPos = gameObject.transform.position;
            Vector3 rectX = spawnedBox.transform.position;
            float rectWidth = spawnedBox.size.x + 15;
            float rectHeight = spawnedBox.size.z + 15;

            if ((selfPos.x < rectX.x + rectWidth && selfPos.x > rectX.x - rectWidth) &&
                (selfPos.z < rectX.z + rectHeight && selfPos.z > rectX.z - rectHeight))
            {
                gameObject.transform.position = new Vector3(0, gameObject.transform.position.y, 0);
                ResetPath();
            }
        }
    }
    
    private void ResetPath()
    {
        _navigator.isStopped = false;
        Vector3 playerPosition = FindClosestPlayer().position;
        playerPosition.y = 0;
        _navigator.SetDestination(playerPosition);
    }
}