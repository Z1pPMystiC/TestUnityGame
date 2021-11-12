using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave {
        public string name;
        public GameObject[] enemies;
        public int count;
        public float rate;
        public int currentHealth;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;

    public Transform[] spawnPoints;

    [SerializeField] public HealthBarScript healthBar;
    [SerializeField] public RobotMotion robot;
    public bool spawnWaves;

    public SpawnState state = SpawnState.COUNTING;
    private void Start()
    {
        if (spawnWaves)
        {
            waveCountdown = timeBetweenWaves;
        }
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive()) {
                if (!healthBar.IsDead())
                {
                    WaveCompleted();
                }
                else
                {
                    state = SpawnState.WAITING;
                }
                
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
        else
        {
            nextWave++;
        }
        
    }
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
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
            GameObject _en = _wave.enemies[Random.Range(0, _wave.enemies.Length)];
            SpawnEnemy(_en, _wave.currentHealth);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(GameObject _enemy, int health)
    {
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject gameClone = Instantiate(_enemy, _sp.transform.position, _sp.transform.rotation);
        gameClone.tag = "Enemy";
        gameClone.GetComponent<RobotMotion>().SetCurrentHealth(health);
        Debug.Log("Spawning Enemy: " + _enemy.name);
    }
}
