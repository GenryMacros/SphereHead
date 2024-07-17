using System.Collections.Generic;
using UnityEngine;


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
        if (_enemiesCurrentWave <= 0)
        {
            WaveCleared();
        }

        _scoreManager.IncrementScore(scoreWorth, true);
    }

    public void HaltScoreDecrease()
    {
        _scoreManager.IncrementScore(0, false);
    }
    
    private void WaveCleared()
    {
        _currentWave += 1;
        Invoke(nameof(StartNewWave), timeBetweenWaves);
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
