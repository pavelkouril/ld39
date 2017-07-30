using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> SpawnPoints = new List<Transform>();

    public int AliveZombies { get { return spawnedAmount - killManager.KillCount; } }

    public Enemy ZombiePrefab;

    private int waveCount;

    public int WaveInterval = 5;

    public Player player;

    private KillManager killManager;

    private int spawnedAmount;


    private void Awake()
    {
        killManager = GetComponent<KillManager>();
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (!player.IsDead)
        {
            if (AliveZombies < 100)
            {
                waveCount++;

                foreach (var s in SpawnPoints)
                {
                    var enemy = Instantiate(ZombiePrefab, s.transform.position, Quaternion.identity);
                    enemy.FollowPlayer(player);
                    enemy.EnemyKilled += killManager.Enemy_EnemyKilled;
                }

                if (waveCount % 10 == 0)
                {
                    WaveInterval = Mathf.Max(WaveInterval - 1, 4);
                }
            }
            yield return new WaitForSeconds(WaveInterval);
        }
    }
}
