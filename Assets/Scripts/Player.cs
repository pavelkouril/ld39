using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    public enum Superpowers
    {
        Fireball,
        RingOfFire,
        Teleport,
        BurningVision,
    }

    public TutorialManager tutManager;

    public float Health;

    public Transform SpellSpawn;

    public Fireball Fireball;

    public RingOfFire RingOfFire;

    public BurningVision LeftEyeBurningVision;
    public BurningVision RightEyeBurningVision;

    public Animator animator;

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

    public ParticleSystem TeleportParticles;

    private Vector3? target;
    private float hitTimestamp;
    private bool shouldBurningVisionBeActive;
    private bool shouldTeleport;
    private Vector3? teleportTarget;
    private bool shouldCastRingOfFire;
    private Vector3? fireballTarget;

    public bool IsDead { get { return Health <= 0; } }

    public bool ShouldBlockMovement { get { return teleportTarget != null || shouldCastRingOfFire; } }

    public PlayerControls playerControls { get; private set; }

    public AudioClip HitSound;
    public AudioClip TeleportSound;
    public AudioClip BurningVisionSound;
    public AudioClip FireballSound;
    public AudioClip RingOfFireSound;
    public AudioClip PickupRechargeSound;
    private bool vignette;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
    }

    void Update()
    {
        if (IsDead)
        {
            animator.applyRootMotion = true;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("dying_backwards"))
            {
                animator.SetBool("Die", false);
            }
            else
            {
                animator.SetBool("Die", true);
            }

            if (!vignette)
            {
                vignette = true;
                var vig = ScriptableObject.CreateInstance<Vignette>();
                vig.enabled.Override(true);
                vig.intensity.Override(1);

                var volume = PostProcessManager.instance.QuickVolume(12, 110f, vig);
                volume.weight = 0f;

                DOTween.Sequence()
                    .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 0.6f, 5f));
            }
        }

        if (target != null && target != transform.position)
        {
            animator.SetBool("Run", true);
            transform.position = Vector3.MoveTowards(transform.position, (Vector3)target, Time.deltaTime * Speed);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if (shouldBurningVisionBeActive && timeStampBurningVisionCast + 2 < Time.time)
        {
            ActivateBurningVision();
        }

        if (shouldBurningVisionBeActive && timeStampBurningVisionCast + BurningVisionDuration + 1 <= Time.time)
        {
            CancelBurningVision();
        }

        if (hitTimestamp + 1 < Time.time)
        {
            animator.SetBool("Hit", false);
        }

        if (fireballTarget != null && timeStampFireballCast + 0.75f <= Time.time)
        {
            var fireball = Instantiate(Fireball, SpellSpawn.position, Quaternion.identity);
            fireball.Target = (Vector3)fireballTarget;
            fireballTarget = null;
            GetComponent<AudioSource>().clip = FireballSound;
            GetComponent<AudioSource>().Play();
        }

        if (timeStampFireballCast + FireballCooldown <= Time.time)
        {
            animator.SetBool("Fireball", false);
        }

        if (teleportTarget != null && timeStampTeleportCast + 0.75f <= Time.time)
        {
            transform.position = (Vector3)teleportTarget;
            teleportTarget = null;
            animator.SetBool("Teleport", false);
        }

        if (shouldCastRingOfFire && timeStampRingOfFireCast + 0.75f <= Time.time)
        {
            Instantiate(RingOfFire, transform.position, Quaternion.identity);
            shouldCastRingOfFire = false;
            animator.SetBool("RingOfFire", false);
            GetComponent<AudioSource>().clip = RingOfFireSound;
            GetComponent<AudioSource>().Play();
        }
    }

    public void CastRingOfFire()
    {
        if (RingOfFireChargesLeft > 0 && timeStampRingOfFireCast + RingOfFireCooldown < Time.time)
        {
            RingOfFireChargesLeft--;
            timeStampRingOfFireCast = Time.time;
            shouldCastRingOfFire = true;
            animator.SetBool("RingOfFire", true);
            target = null;
        }
    }

    internal void Stop()
    {
        target = null;
    }

    public void CastFireball(Vector3 target)
    {
        if (FireballChargesLeft > 0 && timeStampFireballCast + FireballCooldown < Time.time)
        {
            FireballChargesLeft--;
            timeStampFireballCast = Time.time;
            animator.SetBool("Fireball", true);

            fireballTarget = target;
        }
    }

    public void Teleport(Vector3 tempTarget)
    {
        if (TeleportChargesLeft > 0 && timeStampTeleportCast + TeleportCooldown < Time.time)
        {
            TeleportChargesLeft--;
            timeStampTeleportCast = Time.time;
            teleportTarget = tempTarget;
            TeleportParticles.Play();

            animator.SetBool("Teleport", true);

            GetComponent<AudioSource>().clip = TeleportSound;
            GetComponent<AudioSource>().Play();
        }
    }

    internal void TakeDamage(float damage)
    {
        Health -= damage;
        animator.SetBool("Hit", true);
        hitTimestamp = Time.time;

        var chromatic = ScriptableObject.CreateInstance<ChromaticAberration>();
        chromatic.enabled.Override(true);
        chromatic.intensity.Override(1);

        var volume = PostProcessManager.instance.QuickVolume(12, 100f, chromatic);
        volume.weight = 0f;

        DOTween.Sequence()
            .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 1f, 0.05f))
            .AppendInterval(0.2f)
            .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 0f, 0.05f))
            .OnComplete(() =>
            {
                RuntimeUtilities.DestroyVolume(volume, true);
                Destroy(this);
            });
        GetComponent<AudioSource>().clip = HitSound;
        GetComponent<AudioSource>().Play();
    }

    public void SetMoveTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    public void ActivateBurningVision()
    {
        LeftEyeBurningVision.gameObject.SetActive(true);
        RightEyeBurningVision.gameObject.SetActive(true);
        GetComponent<AudioSource>().clip = BurningVisionSound;
        GetComponent<AudioSource>().Play();
    }

    public void CastBurningVision()
    {
        if (BurningVisionChargesLeft > 0 && timeStampBurningVisionCast + BurningVisionCooldown < Time.time)
        {
            timeStampBurningVisionCast = Time.time;
            BurningVisionChargesLeft--;
            shouldBurningVisionBeActive = true;

            animator.SetBool("BurningVision", true);
        }
    }

    public void CancelBurningVision()
    {
        animator.SetBool("BurningVision", false);
        LeftEyeBurningVision.gameObject.SetActive(false);
        RightEyeBurningVision.gameObject.SetActive(false);
        shouldBurningVisionBeActive = false;
        playerControls.SuperpowerToCast = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Recharge") && other != null)
        {
            if (!other.GetComponent<Recharge>().IsUsed)
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
                other.GetComponent<Recharge>().IsUsed = true;
                Destroy(other.gameObject);
            }

            GetComponent<AudioSource>().clip = PickupRechargeSound;
            GetComponent<AudioSource>().Play();
        }

        if (other.CompareTag("RechargeTutorial") && other != null)
        {
            Destroy(other.gameObject);
            tutManager.StartGame();

            GetComponent<AudioSource>().clip = PickupRechargeSound;
            GetComponent<AudioSource>().Play();
        }
    }
}
