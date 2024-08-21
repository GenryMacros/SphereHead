using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] 
    public Timer timer;
    
    [SerializeField] 
    private List<BoxCollider> regularSpawningZones;
    [SerializeField] 
    private List<BoxCollider> specialSpawningZones;
    private SpawnTask _currentTask;

    [SerializeField] 
    private RegularEnemy regularPrefab;
    [SerializeField] 
    private RangedEnemy rangedPrefab;
    [SerializeField] 
    private Boss bossPrefab;
    [SerializeField] 
    private Gun rangedEnemyGun;
    
    [SerializeField] 
    private MeleeWeapon meleeEnemyWeapon;
    
    private void Start()
    {
        timer.isLooping = true;
        timer.waitTime = 1;
        timer.callback = Spawn;
    }
    
    private void Spawn()
    {
        if (_currentTask.isBossTask)
        {
            if (!GameController.instance.isBossAlive)
            {
                Boss boss = Instantiate(bossPrefab, transform);
                boss.transform.position = new Vector3(0, 8, 0);
                boss.transform.eulerAngles = Vector3.zero;
                boss.Activate();
                GameController.instance.isBossAlive = true;
            }
            else
            {
                return;
            }
        }
        int enemiesLeft = _currentTask.rangedEnemies + _currentTask.regularEnemies;
        if (enemiesLeft > 0)
        {
            int activeRanged = GameController.instance.activeRangeEnemies;
            int activeRegular = GameController.instance.activeRegularEnemies;

            bool isRangedCanBeSpawned = _currentTask.rangedEnemies > 0 && activeRanged < _currentTask.maxActiveRanged;
            bool isRegularCanBeSpawned = _currentTask.regularEnemies > 0 && activeRegular < _currentTask.maxActiveRegular;
            
            int randomVal = Random.Range(0, 100);
            if (isRangedCanBeSpawned && (randomVal > 80 || !isRegularCanBeSpawned))
            {
                BoxCollider selectedZone = SelectRandomArea(true);
                Vector3 position = SelectedRandomPosition(selectedZone);
                RangedEnemy newRanged = Instantiate(rangedPrefab, transform);
                newRanged.transform.position = position;
                
                Gun newGun = Instantiate(rangedEnemyGun, newRanged.gameObject.transform);
                
                newRanged.spawnedBox = selectedZone;
                newRanged.SetGun(newGun);
                
                _currentTask.rangedEnemies -= 1;
                GameController.instance.activeRangeEnemies += 1;
            }
            else if (isRegularCanBeSpawned)
            {
                BoxCollider selectedZone = SelectRandomArea(false);
                Vector3 position = SelectedRandomPosition(selectedZone);
                RegularEnemy newRegular = Instantiate(regularPrefab, transform);
                MeleeWeapon newGun = Instantiate(meleeEnemyWeapon, newRegular.transform);

                newRegular.spawnedBox = selectedZone;
                newRegular.SetGun(newGun);
                newRegular.transform.position = position;
                
                _currentTask.regularEnemies -= 1;
                GameController.instance.activeRegularEnemies += 1;
            }
        }
        else
        {
            timer.Stop();
        }
    }

    private BoxCollider SelectRandomArea(bool isForRanged)
    {
        List<BoxCollider> regionsPool;
        int randomVal = Random.Range(0, 100);
        int specialLocationChance = isForRanged ? 20 : (_currentTask.isAllLocationsOpen ? 50 : 100);

        regionsPool = randomVal > specialLocationChance ? specialSpawningZones : regularSpawningZones;

        float[] weights = Enumerable.Repeat(1.0f, regionsPool.Count).ToArray();
        BoxCollider selectedZone = regionsPool[GetRandomWeightedIndex(weights)];
        return selectedZone;
    }
    
    private Vector3 SelectedRandomPosition(BoxCollider selectedZone)
    {
        
        Vector3 worldSpaceCenter = selectedZone.transform.TransformPoint(selectedZone.center);
        Vector2 zonesXBorders = new Vector2(worldSpaceCenter.x - selectedZone.size.x / 2,
                                            worldSpaceCenter.x + selectedZone.size.x / 2);
        Vector2 zoneZBorders = new Vector2(worldSpaceCenter.z - selectedZone.size.z / 2,
                                           worldSpaceCenter.z + selectedZone.size.z / 2);
        
        return new Vector3(Random.Range(zonesXBorders.x, zonesXBorders.y), 
                           -1, Random.Range(zoneZBorders.x, zoneZBorders.y));

    }
    
    private int GetRandomWeightedIndex(float[] weights)
    {
        float weightSum = 0f;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }
        
        int index = weights.Length - 1;
        while (index > 0)
        {
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }
            weightSum -= weights[index];
            index -= 1;
        }
        return index;
    }

    public void AssignTask(SpawnTask newTask)
    {
        _currentTask = newTask;
        timer.Begin();
    }
}
