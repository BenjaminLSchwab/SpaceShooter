using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public int enemiesSpawned;
    public int enemiesKilled;
    public int enemiesDeSpawned;
    // Start is called before the first frame update
    void Start()
    {
        
        //enemySpawners.AddRange(FindObjectsOfType<EnemySpawner>());
        foreach (var spawner in enemySpawners)
        {
            enemiesSpawned += spawner.GetEnemyCount();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToEnemyKillCount(int amount = 1)
    {
        enemiesKilled += amount;
        CheckForLevelOver();
    }

    public void AddToEnemyDeSpawnCount()
    {
        enemiesDeSpawned += 1;
        CheckForLevelOver();
    }

    public void CheckForLevelOver()
    {
        if (enemiesKilled + enemiesDeSpawned == enemiesSpawned)
        {          
            FindObjectOfType<Level>().LoadNextLevel();
        }
    }
}
