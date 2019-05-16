using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool loopAllWaves;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());

        } while (loopAllWaves);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++ )
        {
            yield return SpawnAllEnemiesInWave(waveConfigs[waveIndex]);
        }
    }

    // Update is called once per frame
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
        {
            var newEnemy = Instantiate(
            waveConfig.GetEnemyPrefab(),
            waveConfig.GetWaypoints()[0].position,
            Quaternion.identity
            );

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
        yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
