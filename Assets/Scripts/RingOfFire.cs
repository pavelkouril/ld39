using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfFire : MonoBehaviour
{
    public ParticleSystem RingParticles;

    private float timeStart;
    private new SphereCollider collider;

    private void Start()
    {
        timeStart = Time.time;
        collider = GetComponent<SphereCollider>();
        RingParticles.transform.SetParent(null, true);
    }

    private void Update()
    {
        if (collider.radius < 5)
        {
            collider.radius = Mathf.Min(collider.radius + (Time.deltaTime * 3), 5);
            var shape = RingParticles.shape;
            shape.radius = collider.radius;
            RingParticles.Play();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(1);
            Vector3 dir = (other.transform.position - transform.position).normalized;
            other.transform.position = transform.position + (dir * (collider.radius + 1));
        }
    }
}
