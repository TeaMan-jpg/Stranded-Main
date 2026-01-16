using Platformers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 5f;
    private float timeSinceLastSpawn;


    [SerializeField] private EnemyAI enemyPrefab;
    private IObjectPool<EnemyAI> enemyPool;

    private void Awake()
    {
        enemyPool = new ObjectPool<EnemyAI>(CreateEnemy, OnGet, OnRelease);
    }   

    private void OnGet(EnemyAI enemy)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = spawnPoint.position;
        enemy.gameObject.SetActive(true);
    }
    private void OnRelease(EnemyAI enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private EnemyAI CreateEnemy()
    {
        EnemyAI enemy = Instantiate(enemyPrefab);
        enemy.SetPool(enemyPool);
        return enemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeSinceLastSpawn)
        {

            enemyPool.Get();
            timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        }

    }
}
