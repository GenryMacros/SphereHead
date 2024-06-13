using System.Collections.Generic;
using UnityEngine;


public struct SpawnTask
{
    public int regularEnemies; 
    public int rangedEnemies;
    public int maxActiveRanged;
    public int maxActiveRegular;
    public bool isAllLocationsOpen;
}


public class GameController : MonoBehaviour
{
    public List<PlayerController> players;
    
    public static GameController instance;
    public int activeRegularEnemies;
    public int activeRangeEnemies;
    
    [SerializeField]
    private float _minRegularEnemies;
    [SerializeField]
    private float _maxRegularEnemies;
    [SerializeField]
    private float _minRangedEnemies;
    [SerializeField]
    private float _maxRangedEnemies;
    
    [SerializeField]
    private EnemySpawner _spawner;
    
    [SerializeField]
    private float timeBetweenWaves;
    
    [SerializeField]
    private int _maxWaves;
    private int _enemiesCurrentWave;
    private int _currentWave = 1;
    
    void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        StartNewWave();
    }

    public int GetCurrentWave()
    {
        return _currentWave;
    }
    
    public int GetMaxWaves()
    {
        return _maxWaves;
    }
    
    public void EnemyDeath(LivingBeing deadEnemy)
    {
        if (deadEnemy is RangedEnemy)
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
    }
    
    private void WaveCleared()
    {
        _currentWave += 1;
        Invoke(nameof(StartNewWave), timeBetweenWaves);
    }

    private void StartNewWave()
    {
        SpawnTask task = new SpawnTask();
        float t = (float)_currentWave / _maxWaves;

        task.rangedEnemies = (int)Mathf.Lerp(_minRangedEnemies, _maxRangedEnemies, t);
        task.regularEnemies = (int)Mathf.Lerp(_minRegularEnemies, _maxRegularEnemies, t);
        task.maxActiveRanged = task.rangedEnemies / 2;
        task.maxActiveRegular = task.regularEnemies / 2;
        task.isAllLocationsOpen = _currentWave >= _maxWaves / 2;

        _enemiesCurrentWave = task.rangedEnemies + task.regularEnemies;
        _spawner.AssignTask(task);
    }
}
