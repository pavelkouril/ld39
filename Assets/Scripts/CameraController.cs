using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollArea = 16.0f;
    public float scrollSpeed = 8.0f;
    public Vector2 xBounds = new Vector2(-20.0f, 20.0f);
    public Vector2 zBounds = new Vector2(-20.0f, 20.0f);
    private Vector2 mousePosition;
    private float angle;

    void Start()
    {
        mousePosition = new Vector2();
        angle = 0.0f;
    }

    void LateUpdate()
    {
        ProcessTransformation();
        //ProcessZoom();
    }

    public static float InvertMouseY(float y)
    {
        return Screen.height - y;
    }

    private void ToLocalSpace(float hVal, float vVal, out float xComponent, out float zComponent)
    {
        xComponent = (hVal * scrollSpeed * Mathf.Cos(angle * Mathf.Deg2Rad) + vVal * scrollSpeed * Mathf.Sin(angle * Mathf.Deg2Rad)) * Time.deltaTime;
        zComponent = (hVal * scrollSpeed * -Mathf.Sin(angle * Mathf.Deg2Rad) + vVal * scrollSpeed * Mathf.Cos(angle * Mathf.Deg2Rad)) * Time.deltaTime;
    }

    private void ProcessTransformation()
    {
        mousePosition.x = Input.mousePosition.x;
        mousePosition.y = Input.mousePosition.y;

        float H = -Input.GetAxis("Horizontal");
        float V = -Input.GetAxis("Vertical");
        float x;
        float z;

        if (mousePosition.x < scrollArea)
        {
            H += 1.0f;
        }
        else if (mousePosition.x > Screen.width - scrollArea)
        {
            H -= 1.0f;
        }

        if (mousePosition.y < scrollArea)
        {
            V += 1.0f;
        }
        else if (mousePosition.y > Screen.height - scrollArea)
        {
            V -= 1.0f;
        }

        V *= -1;
        H *= -1;

        H = Mathf.Clamp(H, -1.0f, 1.0f);
        V = Mathf.Clamp(V, -1.0f, 1.0f);
        ToLocalSpace(H, V, out x, out z);

        transform.Translate(new Vector3(x, 0.0f, z), Space.World);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xBounds.x, xBounds.y),
            transform.position.y,
            Mathf.Clamp(transform.position.z, zBounds.x, zBounds.y));
    }
}
