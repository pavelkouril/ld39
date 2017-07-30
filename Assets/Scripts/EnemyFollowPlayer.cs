using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowPlayer : MonoBehaviour
{
    public Player Player;

    private NavMeshAgent navMeshAgent;
    private Enemy enemy;

    private bool randomWalk = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        StartCoroutine(FollowPlayer());
    }

    void Update()
    {
        if (randomWalk && navMeshAgent.remainingDistance < 0.5f)
        {
            navMeshAgent.isStopped = true;
            enemy.IdleAnimation();
        }
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            if (Player != null && !Player.IsDead)
            {
                navMeshAgent.SetDestination(Player.transform.position);
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(1);
        }
        randomWalk = true;
        navMeshAgent.SetDestination(new Vector3(UnityEngine.Random.Range(-15f, 15f), 0, UnityEngine.Random.Range(-15f, 15f)));
    }
}
