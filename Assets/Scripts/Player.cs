using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Superpowers
    {
        Fireball,
        RingOfFire,
        Teleport,
        BurningVision,
    }

    public float Health;

    public Transform SpellSpawn;

    public TargetSpell Fireball;

    public ParticleSystem RingOfFire;

    public BurningVision LeftEyeBurningVision;
    public BurningVision RightEyeBurningVision;

    public float Speed = 5;

    private Vector3? target;

    void Update()
    {
        if (target != null && target != transform.position)
        {
            //transform.LookAt((Vector3)target);
            transform.position = Vector3.MoveTowards(transform.position, (Vector3)target, Time.deltaTime * Speed);
        }
    }

    public void CastRingOfFire()
    {
        if (!RingOfFire.isPlaying)
        {
            RingOfFire.Play();
            foreach (var e in Physics.OverlapSphere(transform.position, 5, 1 << 9))
            {
                e.GetComponent<Enemy>().Health -= 10;
                Vector3 dir = (e.transform.position - transform.position).normalized;
                e.transform.position = dir * 5;
            }
        }
    }

    internal void Stop()
    {
        target = null;
    }

    public void CastFireball(Transform t)
    {
        var fireball = Instantiate(Fireball);
        fireball.transform.position = SpellSpawn.position;
        fireball.Target = t;
    }

    public void Teleport(Vector3 tempTarget)
    {
        transform.position = tempTarget;
    }

    public void SetMoveTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    public void CastBurningVision()
    {
        LeftEyeBurningVision.gameObject.SetActive(true);
        RightEyeBurningVision.gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Health -= 2 * Time.deltaTime;
        }
    }
}
