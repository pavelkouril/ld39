using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    public float Health;

    private void Update()
    {
        if (Health <= 0)
        {
            var go = Instantiate(GetComponentInChildren<ParticleSystem>(true).gameObject);
            go.SetActive(true);
            go.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }
}
