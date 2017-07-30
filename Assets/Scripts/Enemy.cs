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

    public float Damage;

    private EnemyFollowPlayer playerFollower;

    private Player player;
    private float attacTimestamp;
    private float attacLength = 2.32f;

    private float hitTimestamp;

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
        if (animator.GetBool("Hit") && hitTimestamp + 1.5f < Time.time)
        {
            animator.SetBool("Hit", false);
            WalkAnimation();
        }
    }

    public void IdleAnimation()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Hit", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", true);
    }

    public void AttackAnimation()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Hit", false);
        animator.SetBool("Attack", true);
        animator.SetBool("Idle", false);
    }

    public void HitAnimation()
    {
        hitTimestamp = Time.time;
        animator.SetBool("Walk", false);
        animator.SetBool("Hit", true);
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", false);
    }

    public void WalkAnimation()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", false);
    }

    public void FollowPlayer(Player p)
    {
        player = p;
        playerFollower.Player = p;
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
        if (!animator.GetBool("Hit"))
        {
            HitAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (attacTimestamp + attacLength <= Time.time && !animator.GetBool("Hit"))
            {
                attacTimestamp = Time.time;
                AttackAnimation();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !animator.GetBool("Hit"))
        {
            if (attacTimestamp + attacLength <= Time.time)
            {
                other.GetComponent<Player>().TakeDamage(Damage);
                attacTimestamp = Time.time;
                AttackAnimation();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WalkAnimation();
        }
    }
}
