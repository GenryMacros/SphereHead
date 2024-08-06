using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public struct SpawnTask
{
    public int regularEnemies; 
    public int rangedEnemies;
    public int maxActiveRanged;
    public int maxActiveRegular;
    public bool isAllLocationsOpen;
    public bool isBossTask;
}


public class GameController : MonoBehaviour
{
    public List<PlayerController> players;
    
    public static GameController instance;
    public int activeRegularEnemies;
    public int activeRangeEnemies;
    public bool isBossAlive = false;
    
    [SerializeField] 
    private Material _changingMaterial;
    [SerializeField] 
    private Material _noiseMaterial;

    [SerializeField] 
    private float _startNoise = 0.001f;
    [SerializeField] 
    private float _endNoise = 0.02f;
    [SerializeField] 
    private Color _changingMaterialInitialColor;
    [SerializeField] 
    private Color _changingMaterialEndColor;
    
    [SerializeField]
    private float _minRegularEnemies;
    [SerializeField]
    private float _maxRegularEnemies;
    [SerializeField]
    private float _minRangedEnemies;
    [SerializeField]
    private float _maxRangedEnemies;
    
    [SerializeField]
    private DeathScreen deathScreen;
    [SerializeField]
    private Notificator _notificator;
    [SerializeField]
    private EnemySpawner _spawner;
    [SerializeField]
    private float timeBetweenWaves;
    [SerializeField]
    private int _maxWaves;
    
    [SerializeField]
    private ScoreManager _scoreManager;
    
    private int _enemiesCurrentWave;
    private int _currentWave = 1;
    private bool _isGamePaused = false;

    private float _totalEnemiesCurrentWave;
    private float _lastNoiseTarget;
    private float _noiseTarget;
    private Color _lastColorTarget;
    private Color _colorTarget;
    
    void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        StartNewWave();
        foreach (var player in players)
        {
            player.death += PlayerDied;
        }
        _changingMaterial.SetColor("_EmissionColor", _changingMaterialInitialColor);
        _noiseMaterial.SetFloat("_noise_amount", 0.0f);

        _colorTarget = _changingMaterialInitialColor;
        _noiseTarget = 0.0f;
        
        ChangeShadersTarget();
    }

    public int GetCurrentWave()
    {
        return _currentWave;
    }
    
    public int GetMaxWaves()
    {
        return _maxWaves;
    }
    
    public void EnemyDeath(LivingBeing deadEnemy, int scoreWorth)
    {
        if (deadEnemy is Boss)
        {
            BossDied();
            return;
        }
        else if (deadEnemy is RangedEnemy)
        {
            activeRangeEnemies -= 1;
        }
        else
        {
            activeRegularEnemies -= 1;
        }
        _enemiesCurrentWave -= 1;
        float enemyT = (_totalEnemiesCurrentWave - _enemiesCurrentWave) / _totalEnemiesCurrentWave;

        _noiseMaterial.SetFloat("_noise_amount",
            Mathf.Lerp(_lastNoiseTarget, _noiseTarget, enemyT));
        _changingMaterial.SetColor("_EmissionColor", 
            Color.Lerp(_lastColorTarget, _colorTarget, enemyT));
        
        _scoreManager.IncrementScore(scoreWorth, true);
        if (_enemiesCurrentWave <= 0)
        {
            WaveCleared();
        }
    }

    public void HaltScoreDecrease()
    {
        _scoreManager.IncrementScore(0, false);
    }
    
    private void WaveCleared()
    {
        _currentWave += 1;
        
        ChangeShadersTarget();
        
        Invoke(nameof(StartNewWave), timeBetweenWaves);
    }

    private void ChangeShadersTarget()
    {
        if (_currentWave == 7)
        {
            _lastColorTarget = _colorTarget;
            _lastNoiseTarget = _noiseTarget;
            return;
        }
        _lastColorTarget = new Color(_colorTarget.r, _colorTarget.g, _colorTarget.b);
        _colorTarget = Color.Lerp(_changingMaterialInitialColor, _changingMaterialEndColor, (_currentWave + 1) / 7.0f);
        
        if (_currentWave >= 2)
        {
            _lastNoiseTarget = _noiseTarget;
            _noiseTarget =  Mathf.Lerp(_startNoise, _endNoise, (3.0f - (7 - (_currentWave + 1))) / 3.0f);
        }
    }
    
    private void StartNewWave()
    {
        SpawnTask task = new SpawnTask();
        if (_currentWave == 7)
        {
            task.isBossTask = true;
        }
        else
        {
            float t = (float)(_currentWave - 1) / _maxWaves;
        
            task.rangedEnemies = (int)Mathf.Lerp(_minRangedEnemies, _maxRangedEnemies, t);
            task.regularEnemies = (int)Mathf.Lerp(_minRegularEnemies, _maxRegularEnemies, t);
            task.maxActiveRanged = task.rangedEnemies > 1 ? task.rangedEnemies / 3 : 1;
            task.maxActiveRegular = task.regularEnemies / 2;
            task.isAllLocationsOpen = _currentWave >= _maxWaves / 2;
        
            _enemiesCurrentWave = task.rangedEnemies + task.regularEnemies;
            _totalEnemiesCurrentWave = _enemiesCurrentWave;
        }
        _spawner.AssignTask(task);
        
        _notificator.AppendSpecialNotification($"Wave {_currentWave} start", NotificationType.Enemy);
    }

    public bool IsGamePaused()
    {
        return _isGamePaused;
    }
    
    private void PlayerDied()
    {
        _isGamePaused = true;
        deathScreen.Activate();
    }

    private void BossDied()
    {
        Debug.Log("Boss died");
    }
}
