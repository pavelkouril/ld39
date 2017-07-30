using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EnemyDieEventHandler(Enemy sender);

[SelectionBase]
public class Enemy : MonoBehaviour
{
    public event EnemyDieEventHandler EnemyKilled;

    public float Health;

    public Animator animator;

    private EnemyFollowPlayer playerFollower;

    private Player player;

    private void Awake()
    {
        playerFollower = GetComponent<EnemyFollowPlayer>();
    }

    private void Update()
    {
        if (Health <= 0)
        {
            if (EnemyKilled != null)
            {
                EnemyKilled(this);
            }
            var go = Instantiate(GetComponentInChildren<ParticleSystem>(true).gameObject);
            go.SetActive(true);
            go.transform.position = transform.position;
            Destroy(go, 5f);
            Destroy(this.gameObject);
        }
    }

    internal void IdleAnimation()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);
    }

    public void FollowPlayer(Player p)
    {
        player = p;
        playerFollower.Player = p;
    }
}
