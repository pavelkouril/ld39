using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningVision : MonoBehaviour
{
    public float MaxLength = 5;

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
        if (Physics.Raycast(ray, out hit, MaxLength))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position - transform.forward * MaxLength);
        }
    }
}
