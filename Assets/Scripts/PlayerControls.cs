using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    public Player.Superpowers? SuperpowerToCast;

    private float yHeight;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        yHeight = transform.position.y;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            var tempTarget = hit.point;
            tempTarget.y = yHeight;

            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && hit.transform.CompareTag("Ground"))
            {
                transform.LookAt(tempTarget);
                player.SetMoveTarget(tempTarget);
            }

            if (Input.GetMouseButton(1) && SuperpowerToCast == Player.Superpowers.Teleport && hit.transform.CompareTag("Ground"))
            {
                player.Stop();
                player.Teleport(tempTarget);
            }

            if (Input.GetMouseButton(1) && SuperpowerToCast == Player.Superpowers.Fireball && hit.transform.CompareTag("Enemy"))
            {
                player.Stop();
                transform.LookAt(tempTarget);
                player.CastFireball(hit.transform);
            }

            if (Input.GetMouseButton(1) && SuperpowerToCast == Player.Superpowers.BurningVision)
            {
                player.Stop();
                transform.LookAt(tempTarget);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SuperpowerToCast = Player.Superpowers.Fireball;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SuperpowerToCast = Player.Superpowers.BurningVision;
            player.CastBurningVision();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SuperpowerToCast = Player.Superpowers.Teleport;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.CastRingOfFire();
        }

    }
}
