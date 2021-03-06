﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    public Player.Superpowers? SuperpowerToCast;

    public LayerMask layerMask;

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
        if (player.IsDead)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && Physics.Raycast(ray, out hit, 300, layerMask) && !player.ShouldBlockMovement)
        {
            var tempTarget = hit.point;
            tempTarget.y = yHeight;

            if (Input.GetMouseButtonDown(1))
            {
                player.CancelBurningVision();
                transform.LookAt(tempTarget);
                player.SetMoveTarget(tempTarget);
            }

            if (Input.GetMouseButtonDown(0) && SuperpowerToCast == Player.Superpowers.Teleport && hit.transform.CompareTag("Ground"))
            {
                player.CancelBurningVision();
                player.Stop();
                player.Teleport(tempTarget);
                SuperpowerToCast = null;
            }

            if (Input.GetMouseButtonDown(0) && SuperpowerToCast == Player.Superpowers.Fireball)
            {
                player.Stop();
                transform.LookAt(tempTarget);
                player.CastFireball(hit.point);
                SuperpowerToCast = null;
            }

            if (Input.GetMouseButton(0) && SuperpowerToCast == Player.Superpowers.BurningVision)
            {
                player.Stop();
                transform.LookAt(tempTarget);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && player.FireballChargesLeft > 0)
        {
            player.CancelBurningVision();
            SuperpowerToCast = Player.Superpowers.Fireball;
        }

        if (Input.GetKeyDown(KeyCode.W) && player.BurningVisionChargesLeft > 0)
        {
            SuperpowerToCast = Player.Superpowers.BurningVision;
            player.CastBurningVision();
        }

        if (Input.GetKeyDown(KeyCode.E) && player.TeleportChargesLeft > 0)
        {
            player.CancelBurningVision();
            SuperpowerToCast = Player.Superpowers.Teleport;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.CancelBurningVision();
            player.CastRingOfFire();
        }

    }
}
