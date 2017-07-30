﻿using System;
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

    public Fireball Fireball;

    public RingOfFire RingOfFire;

    public BurningVision LeftEyeBurningVision;
    public BurningVision RightEyeBurningVision;

    public int FireballChargesLeft;
    public int BurningVisionChargesLeft;
    public int TeleportChargesLeft;
    public int RingOfFireChargesLeft;

    public float FireballCooldown;
    public float BurningVisionCooldown;
    public float TeleportCooldown;
    public float RingOfFireCooldown;

    public float BurningVisionDuration;

    internal float timeStampFireballCast = -11;
    internal float timeStampBurningVisionCast = -11;
    internal float timeStampTeleportCast = -11;
    internal float timeStampRingOfFireCast = -11;

    public float Speed = 5;

    private Vector3? target;

    public bool IsDead { get { return Health <= 0; } }

    void Update()
    {
        if (IsDead)
        {
            Debug.Log("you died!");
        }

        if (timeStampBurningVisionCast + BurningVisionDuration <= Time.time)
        {
            CancelBurningVision();
        }

        if (target != null && target != transform.position)
        {
            //transform.LookAt((Vector3)target);
            transform.position = Vector3.MoveTowards(transform.position, (Vector3)target, Time.deltaTime * Speed);
        }
    }

    public void CastRingOfFire()
    {
        if (RingOfFireChargesLeft > 0 && timeStampRingOfFireCast + RingOfFireCooldown < Time.time)
        {
            RingOfFireChargesLeft--;
            timeStampRingOfFireCast = Time.time;

            var rof = Instantiate(RingOfFire, transform.position, Quaternion.identity);
        }
    }

    internal void Stop()
    {
        target = null;
    }

    public void CastFireball(Transform t)
    {
        if (FireballChargesLeft > 0 && timeStampFireballCast + FireballCooldown < Time.time)
        {
            FireballChargesLeft--;
            timeStampFireballCast = Time.time;

            var fireball = Instantiate(Fireball, SpellSpawn.position, Quaternion.identity);
            fireball.Target = t;
        }
    }

    public void Teleport(Vector3 tempTarget)
    {
        if (TeleportChargesLeft > 0 && timeStampTeleportCast + TeleportCooldown < Time.time)
        {
            TeleportChargesLeft--;
            timeStampTeleportCast = Time.time;

            transform.position = tempTarget;
        }
    }

    public void SetMoveTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    public void CastBurningVision()
    {
        if (BurningVisionChargesLeft > 0 && timeStampBurningVisionCast + BurningVisionCooldown < Time.time)
        {
            timeStampBurningVisionCast = Time.time;
            BurningVisionChargesLeft--;

            LeftEyeBurningVision.gameObject.SetActive(true);
            RightEyeBurningVision.gameObject.SetActive(true);
        }
    }

    public void CancelBurningVision()
    {
        LeftEyeBurningVision.gameObject.SetActive(false);
        RightEyeBurningVision.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Recharge"))
        {
            var rand = UnityEngine.Random.Range(0, 4);
            switch (rand)
            {
                case 0:
                    FireballChargesLeft += 4;
                    break;
                case 1:
                    BurningVisionChargesLeft += 1;
                    break;
                case 2:
                    TeleportChargesLeft += 1;
                    break;
                case 3:
                    RingOfFireChargesLeft += 1;
                    break;
            }

            Destroy(other.gameObject);
        }
    }
}
