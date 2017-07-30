using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Player player;

    public Transform SpawnEnemyPos;

    public Transform SpawnRechargePos;

    public Text textField;

    public GameObject TutorialCanvas;

    public Enemy enemyPrefab;

    public Recharge rechargePrefab;

    public GameObject CreditsText;

    public int State = 0;

    private void Start()
    {
        player.FireballChargesLeft = 0;
        player.BurningVisionChargesLeft = 0;
        player.TeleportChargesLeft = 0;
        player.RingOfFireChargesLeft = 0;

        textField.text = "Move around by clicking with RMB. Or skip tutorial with Return key.";
        TutorialCanvas.SetActive(true);
    }


    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (State == 0 && Input.GetMouseButtonDown(1) && Physics.Raycast(ray, out hit, 300))
        {
            textField.text = "Great! Try teleporting around. Press E and select place to teleport with LMB.";
            State = 1;
            CreditsText.SetActive(false);
        }

        if (State == 0 && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        if (State == 1 && player.timeStampTeleportCast > 0 && Input.GetMouseButton(0) && Physics.Raycast(ray, out hit, 300))
        {
            textField.text = "Beware the enemy! Kill him using Fireball by hitting Q and clicking with LMB on your target.";
            State = 2;
            var enemy = Instantiate(enemyPrefab, SpawnEnemyPos.position, Quaternion.identity);
            enemy.FollowPlayer(player);
            enemy.EnemyKilled += Enemy_EnemyFireballKilled;
        }

        if (State == 1)
        {
            player.FireballChargesLeft = 0;
            player.BurningVisionChargesLeft = 0;
            player.TeleportChargesLeft = 99;
            player.RingOfFireChargesLeft = 0;
        }

        if (State == 2)
        {
            player.FireballChargesLeft = 99;
            player.BurningVisionChargesLeft = 0;
            player.TeleportChargesLeft = 0;
            player.RingOfFireChargesLeft = 0;
        }

        if (State == 3)
        {
            player.FireballChargesLeft = 0;
            player.BurningVisionChargesLeft = 99;
            player.TeleportChargesLeft = 0;
            player.RingOfFireChargesLeft = 0;
        }

        if (State == 4)
        {
            player.FireballChargesLeft = 0;
            player.BurningVisionChargesLeft = 0;
            player.TeleportChargesLeft = 0;
            player.RingOfFireChargesLeft = 99;
        }

        if (State == 5)
        {
            player.FireballChargesLeft = 0;
            player.BurningVisionChargesLeft = 0;
            player.TeleportChargesLeft = 0;
            player.RingOfFireChargesLeft = 0;
        }
    }

    private void Enemy_EnemyFireballKilled(Enemy sender)
    {
        textField.text = "Now try killing using the Burning Vision! Hit W and use LMB to target!";
        State = 3;
        var enemy = Instantiate(enemyPrefab, SpawnEnemyPos.position, Quaternion.identity);
        enemy.FollowPlayer(player);
        enemy.EnemyKilled += Enemy_EnemyBVKilled;
    }

    private void Enemy_EnemyBVKilled(Enemy sender)
    {
        textField.text = "Last but not least, kill them with your Ring of Fire! Hit R.";
        State = 4;
        player.CancelBurningVision();
        var enemy = Instantiate(enemyPrefab, SpawnEnemyPos.position, Quaternion.identity);
        enemy.FollowPlayer(player);
        enemy.EnemyKilled += Enemy_EnemyRoFKilled;
    }


    private void Enemy_EnemyRoFKilled(Enemy sender)
    {
        textField.text = "To recharge your powers, pick up the Recharger.";
        State = 5;
        var recharge = Instantiate(rechargePrefab, SpawnRechargePos.position, Quaternion.identity);
        recharge.gameObject.tag = "RechargeTutorial";
    }

    internal void StartGame()
    {
        this.enabled = false;
        TutorialCanvas.SetActive(false);
        GetComponent<EnemySpawner>().enabled = true;
        GetComponent<RechargeSpawner>().enabled = true;

        player.FireballChargesLeft = 4;
        player.BurningVisionChargesLeft = 1;
        player.TeleportChargesLeft = 1;
        player.RingOfFireChargesLeft = 1;
        player.timeStampRingOfFireCast = 0;
        player.Health = 100;
    }
}
