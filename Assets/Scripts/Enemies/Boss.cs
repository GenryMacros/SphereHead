using UnityEngine;

public class Boss : LivingBeing
{
    
    public enum BossState {ZoneAttack, LaserAttack, CircularAttack, None, Dead};
    public BossState currentState = BossState.None;
    public PlayerController player;
    
    [SerializeField] 
    private GameObject bulletsSpawnPoint;
    
    [SerializeField] 
    private GameObject circularBulletsSpawnPoint;

    [SerializeField] 
    private Weapon _mainWeapon; 
    [SerializeField] 
    private Weapon _circularWeapon; 
    [SerializeField] 
    private Weapon _laserWeapon; 
    [SerializeField] 
    private Weapon _zoneWeapon;
    [SerializeField] 
    private Weapon _knockbackWeapon;
    [SerializeField] 
    private float rotatingTime;
    [SerializeField] 
    private float laserCooldown = 10;
    [SerializeField] 
    private float circularCooldown = 10;
    [SerializeField] 
    private float _circleAttacks;
    
    private Timer laserCooldownTimer;
    private Timer circularCooldownTimer;
    private float _fromLastStateStart;
    private float _angleStart;
    private bool _isCircularReady = true;
    private bool _isLaserReady = true;
    private float _circleAttacksLeft = 0;
    
    void Start()
    {
        base.Start();
        _mainWeapon.ResetSpawnPoint(bulletsSpawnPoint);
        _circularWeapon.ResetSpawnPoint(circularBulletsSpawnPoint);
        _laserWeapon.ResetSpawnPoint(bulletsSpawnPoint);
        _zoneWeapon.ResetSpawnPoint(bulletsSpawnPoint);

        circularCooldownTimer = gameObject.AddComponent<Timer>();
        circularCooldownTimer.waitTime = circularCooldown;
        circularCooldownTimer.callback = MakeCircularReady;
        
        laserCooldownTimer = gameObject.AddComponent<Timer>();
        laserCooldownTimer.waitTime = laserCooldown;
        laserCooldownTimer.callback = MakeLaserReady;
        
        player = GameController.instance.players[0];
        transform.eulerAngles = Vector3.zero;
    }

    public void Activate()
    {
       // _knockbackWeapon.Fire();
    }
    
    protected virtual void FixedUpdate()
    {
        if (GameController.instance.IsGamePaused() || currentState == BossState.Dead)
        {
            return;
        }

        _fromLastStateStart += Time.deltaTime;
        
        switch (currentState)
        {
            case BossState.None:
                ProcessSimpleStraightAttack();
                break;
            case BossState.CircularAttack:
                ProcessCircularAttack();
                break;
            case BossState.LaserAttack:
                ProcessLaserAttack();
                break;
            case BossState.ZoneAttack:
                ProcessZoningAttack();
                break;
        }   
        
        if (currentState == BossState.None)
        {
            DetermineState();
        }
    }

    private void DetermineState()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 position = new Vector2(transform.position.x, transform.position.z);
        float distance2Player = Vector2.Distance(playerPos, position);

        if (distance2Player > 20 || !_isCircularReady)
        {
            currentState = _isLaserReady ? BossState.LaserAttack : BossState.None;
        }
        else
        {
            currentState = BossState.CircularAttack;
            _circleAttacksLeft = _circleAttacks;
        }

        _angleStart = transform.eulerAngles.y;
        _fromLastStateStart = 0.0f;
    }
    
    private void ProcessSimpleStraightAttack()
    {
        gameObject.transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
        gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);
        _mainWeapon.Fire();
    }
    
    private void ProcessCircularAttack()
    {
        if (_circularWeapon.isReadyToFire)
        {
            _circleAttacksLeft -= 1;
            _circularWeapon.transform.Rotate(0, 30, 0);
        }
        
        _circularWeapon.Fire();

        if (_circleAttacksLeft == 0)
        {
            currentState = BossState.None;
            _isCircularReady = false;
            circularCooldownTimer.Begin();
        }
    }

    private void ProcessZoningAttack()
    {
        _zoneWeapon.Fire();
        currentState = BossState.None;
    }

    private void ProcessLaserAttack()
    {
        float t = _fromLastStateStart / rotatingTime;
        float newY = Mathf.Lerp(_angleStart, _angleStart + 720, t);
        gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, newY, transform.eulerAngles.z);
        
        _laserWeapon.Fire();
        if (t >= 1)
        {
            currentState = BossState.None;
            _isLaserReady = false;
            laserCooldownTimer.Begin();
        }
    }

    private void MakeLaserReady()
    {
        _isLaserReady = true;
    }

    private void MakeCircularReady()
    {
        _isCircularReady = true;
    }
    
     public override void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir, OwnerEntity damageCauser)
        {
            base.TakeDamage(damage, knockbackPower, knockbackDir, damageCauser);

            if (hp <= 0)
            {
                currentState = BossState.Dead;
                Destroy(gameObject, 5.0f);
            }
        }
}
