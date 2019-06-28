using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    [SerializeField] int score = 0;
    [SerializeField] float loadNextLevelDelay = 2f;
     public int enemiesSpawned; 
     public int enemiesKilled;
     public int enemiesDeSpawned;
    public bool lastEnemySpawned = false;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void ResetEnemyCounts()
    {
        enemiesKilled = 0;
        enemiesDeSpawned = 0;
        enemiesSpawned = 0;
        lastEnemySpawned = false;
    }

    public IEnumerator CheckForLevelOver()
    {
        Debug.Log("Checking counts, Sp:" + enemiesSpawned + " , Kl:" + enemiesKilled + " , De:" + enemiesDeSpawned);
        if (!lastEnemySpawned) { yield break; }
        Debug.Log("Last Enemy has spawned");
        if (enemiesKilled + enemiesDeSpawned == enemiesSpawned)
        {
            yield return new WaitForSeconds(loadNextLevelDelay);
            FindObjectOfType<Level>().LoadNextLevel();
        }
    }

    public void AddToScore(int amount)
    {
        score += amount;
    }

    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }

    public void AddToEnemySpawnCount(int amount = 1)
    {
        enemiesSpawned += amount;
    }

    public void AddToEnemyKillCount(int amount = 1)
    {
        enemiesKilled += amount;
        StartCoroutine(CheckForLevelOver());
    }

    public void AddToEnemyDeSpawnCount()
    {
        enemiesDeSpawned += 1;
        StartCoroutine(CheckForLevelOver());
    }

    public void SetLastEnemySpawned(bool LastEnemySpawned = true)
    {
        lastEnemySpawned = LastEnemySpawned;
        StartCoroutine( CheckForLevelOver());
    }

}
