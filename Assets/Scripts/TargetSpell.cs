using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpell : MonoBehaviour
{
    public float Speed = 1;
    public Transform Target { get; set; }

    void Update()
    {
        if (Target == null)
        {
            Destroy(this.gameObject);
        }
        if (transform.position != Target.position)
        {
            transform.LookAt(Target);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Time.deltaTime * Speed);
        }
    }
}
