using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAllWaves());
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
