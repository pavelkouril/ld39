using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningVision : MonoBehaviour
{
    public float MaxLength = 5;
    public float DamagePerSecond = 5;

    public LayerMask layerMask;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);

        Ray ray = new Ray(transform.position, -transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, MaxLength, layerMask))
        {
            lineRenderer.SetPosition(1, hit.point);
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().TakeDamage(Time.deltaTime * DamagePerSecond);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position - transform.forward * MaxLength);
        }
    }
}
