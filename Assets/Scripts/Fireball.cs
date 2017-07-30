using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int Damage = 5;
    public float Speed = 1;
    public Transform Target { get; set; }

    void Update()
    {
        if (Target == null)
        {
            Destroy(this.gameObject);
        }
        else if (transform.position != Target.position)
        {
            transform.LookAt(Target);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Time.deltaTime * Speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Health -= Damage;
            Destroy(this.gameObject);
        }
    }
}
