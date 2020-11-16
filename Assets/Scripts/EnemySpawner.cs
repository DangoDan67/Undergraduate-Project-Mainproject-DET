using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private World world;
    public GameObject score;

    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    [System.Serializable]
    public class wave
    {
        public string name;
        public GameObject enemy1;
        public int count;
        public float rate;
    }
    public wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves;
    private float waveCountdown;

    private SpawnState state = SpawnState.COUNTING;

    private float searchCountdown = 3f;

    private int addEnemy = 0;

    private int scoreToChange = 3;

    // Start is called before the first frame update
    void Start()
    {
        world = GetComponent<World>();
        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                waveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
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


    bool IsPosInWorld(Vector3 pos)
    {

        if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.ChunkHeight && pos.z >= 0 && pos.z < VoxelData.WorldSizeInVoxels)
            return true;
        else
            return false;

    }


    void waveCompleted()
    {
        if (score.GetComponent<score>().getIntScore() >= scoreToChange)
        {
            this.scoreToChange += 3;
            this.addEnemy += 1;
        }
        Debug.Log("wave Completed!");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All Waves Complete!");
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
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < (_wave.count + addEnemy); i++)
        {
            StartCoroutine(SpawnEnemy(_wave.enemy1));
            yield return new WaitForSeconds(25);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    IEnumerator SpawnEnemy(GameObject _enemy)
    {
        yield return new WaitForSeconds(10);
        Vector3 addDistance = new Vector3(15, 0, 15);
        Vector3 spawnPos = world.player.transform.position + addDistance;
        if (IsPosInWorld(spawnPos))
        {
            Instantiate(_enemy, spawnPos, _enemy.transform.rotation);
        }
        else
        {
            spawnPos = world.player.transform.position - addDistance;
            if (IsPosInWorld(spawnPos))
            {
                Instantiate(_enemy, spawnPos, _enemy.transform.rotation);
            }
        }
    }
}
