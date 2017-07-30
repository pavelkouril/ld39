using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillManager : MonoBehaviour
{
    public int KillCount = 0;

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void Enemy_EnemyKilled(Enemy sender)
    {
        KillCount++;
    }
}
