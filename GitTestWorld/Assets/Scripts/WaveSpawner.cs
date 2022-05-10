using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING, DONE };

    [System.Serializable]
    public class Wave {
        public string name;
        public GameObject[] enemies;
        public int count;
        public float rate;
        public int currentHealth;
        public Transform[] spawnPoints;
    }

    public Wave[] waves;
    public int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;

    public bool isIndoorMap = true;

    [SerializeField] public HealthBarScript healthBar;
    [SerializeField] public RobotMotion robot;

    public bool firstStageDone = false;
    public GameObject stageBlocker;
    public bool thirdDone = false;
    public bool twoThirdsDone = false;
    public bool allWavesDone = false;
    public GameObject bossBlocker;
    public string firstStageText;
    public string allWavesDoneText;
    public BossMotion bossMotion;

    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI centerText;

    public FloppyLauncherScript floppyLauncher;
    public RaycastGun tesla;


    public SpawnState state = SpawnState.COUNTING;
    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        /*for (int i = 0; i < 100; i++)
        {
            if (waves[i] != null)
            {
                waves[i].name = "Round " + (i + 1);
                waves[i].count = i + 2;
                waves[i].currentHealth = 100 + (i * 50);
            }
        }*/
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

        if(firstStageDone)
        {
            firstStageDone = false;
            Destroy(stageBlocker);
            centerText.SetText(firstStageText);
            Invoke("ClearText", 2f);
        }

        if(allWavesDone)
        {
            allWavesDone = false;
            Destroy(bossBlocker);
            floppyLauncher.ammoLeft = floppyLauncher.fullAmmo;
            tesla.ammoLeft = tesla.fullAmmo;
            centerText.SetText(allWavesDoneText);
            Invoke("ClearText", 2f);
            if (WhatMapIsSelected.isIndoorMap)
            {
                bossMotion.playerInBossArena = true;
            }
        }

        waveCounter.SetText("Wave: " + (nextWave + 1));
    }

    void WaveCompleted() 
    {
        Debug.Log("Wave Completed!");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave == 2)
        {
            firstStageDone = true;
        }

        if (nextWave + 1 > waves.Length - 1)
        {
            state = SpawnState.DONE;
            Debug.Log("Completed All Waves. Looping...");
            allWavesDone = true;
        }
        else
        {
            nextWave++;
            Invoke("PlayNewWaveSound", timeBetweenWaves);
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
            SpawnEnemy(_en, _wave.currentHealth, _wave);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;   
    }

    void SpawnEnemy(GameObject _enemy, int health, Wave _wave)
    {
        Transform _sp = _wave.spawnPoints[Random.Range(0, _wave.spawnPoints.Length)];
        GameObject gameClone = Instantiate(_enemy, _sp.transform.position, _sp.transform.rotation);
        gameClone.tag = "Enemy";
        gameClone.GetComponent<RobotMotion>().SetCurrentHealth(health);
        gameClone.GetComponent<AudioSource>().volume = 1;
        gameClone.GetComponent<AudioSource>().time = 10f;
        Debug.Log("Spawning Enemy: " + _enemy.name);
    }

    public void ClearText()
    {
        if (centerText != null)
        {
            centerText.SetText("");
        }
    }

    private void PlayNewWaveSound() 
    {
        FindObjectOfType<AudioManager>().Play("NewWave");
    }
}
