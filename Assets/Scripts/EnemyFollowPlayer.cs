using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowPlayer : MonoBehaviour
{
    public Transform Player;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Player").transform;
        }

        StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            if (Player != null)
            {
                navMeshAgent.SetDestination(Player.position);
            }
            yield return new WaitForSeconds(1);
        }
    }
}
