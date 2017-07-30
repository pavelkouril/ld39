using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int Damage = 5;
    public float Speed = 1;
    public Vector3 Target { get; set; }

    void Update()
    {
        if (transform.position != Target)
        {
            transform.LookAt(Target);
            transform.position = Vector3.MoveTowards(transform.position, Target, Time.deltaTime * Speed);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
