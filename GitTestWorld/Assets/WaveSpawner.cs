using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave {
        public string name;
        public GameObject enemy;
        public int count;
        public float rate;

    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;

    [SerializeField] public Transform spawnPoint;
    [SerializeField] public HealthBarScript healthBar;

    public SpawnState state = SpawnState.COUNTING;
    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive()) {
                // Begin a new round
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0) {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }

    }

    void WaveCompleted() 
    {
        Debug.Log("Wave Completed!");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("Completed All Waves. Looping...");
        }

        nextWave++;
    }
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            Debug.Log("Search Complete");
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                Debug.Log("enemies are gone");
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(GameObject _enemy)
    {

        GameObject gameClone = Instantiate(_enemy, spawnPoint.transform.position, spawnPoint.transform.rotation);
        gameClone.tag = "Enemy";
        Debug.Log("Spawning Enemy: " + _enemy.name);
    }
}
