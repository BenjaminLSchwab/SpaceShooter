using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] EnemySpawner NextSpawner;
    [SerializeField] float DelayNextSpawner = 0f;






    // Start is called before the first frame update


    private void OnEnable()
    {
        StartCoroutine(SpawnAllWaves());
    }



    private IEnumerator SpawnAllWaves()
    {
        yield return new WaitForSeconds(1);

        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++ )
        {
            yield return SpawnAllEnemiesInWave(waveConfigs[waveIndex]);
        }
    

        if (NextSpawner)
        {
            yield return new WaitForSeconds(DelayNextSpawner);
            NextSpawner.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public int GetEnemyCount()
    {
        var ans = 0;
        foreach (var wave in waveConfigs)
        {
            ans += wave.GetNumberOfEnemies();
        }
        return ans;
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
        {
            
            
            var newEnemy = Instantiate(
            waveConfig.GetEnemyPrefab(),
            waveConfig.GetWaypoints()[0].position,
            Quaternion.identity
            );

            var pathing = newEnemy.GetComponent<EnemyPathing>();
            if (pathing) { pathing.SetWaveConfig(waveConfig); }
        yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
        

    }
}
