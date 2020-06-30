using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    private int enemyCount;
    private int currentWaveIndex = 0;
    void Start()
    {
        SpawnWave(waveConfigs[currentWaveIndex]);
        enemyCount = waveConfigs[currentWaveIndex].GetEnemyCount();
    }

    void Update()
    {
        if (enemyCount <= 0)
        {
            if (currentWaveIndex >= waveConfigs.Count - 1) { currentWaveIndex = 0; }
            else { currentWaveIndex += 1; }

            if (currentWaveIndex <= waveConfigs.Count) 
            {
                SpawnWave(waveConfigs[currentWaveIndex]);
                enemyCount = waveConfigs[currentWaveIndex].GetEnemyCount();
            }

            
        }
    }

    private void SpawnWave(WaveConfig wave)
    {
        GameObject enemy = wave.GetEnemyPrefab();
        bool doSpawn = true;
        
            StartCoroutine(CorotSpawnWave(wave));
    }

    IEnumerator CorotSpawnWave(WaveConfig w)
    {
        for (int n = 0; n != w.GetEnemyCount(); n++)
        {
            yield return new WaitForSeconds(w.GetSpawnInterval());
            GameObject gobj = Instantiate(w.GetEnemyPrefab()) as GameObject;
            gobj.GetComponent<EnemyPathing>().SetWaveConfig(w);
            gobj.GetComponent<Enemy>().SetEnemySpawner(this);
        }
    }

    public void DecreaseEnemyCount()
    {
        enemyCount -= 1;
    }
}
