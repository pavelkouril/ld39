using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeSpawner : MonoBehaviour
{
    public List<RechargeSpawnPoint> SpawnPoints = new List<RechargeSpawnPoint>();

    public GameObject RechargePrefab;

    public int SpawnInterval = 15;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            foreach (var s in SpawnPoints)
            {
                if (s.SpawnedRecharge == null)
                {
                    var recharge = Instantiate(RechargePrefab, s.transform.position, Quaternion.identity);
                    s.SpawnedRecharge = recharge;
                }
            }
            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}
