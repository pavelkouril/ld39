using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Player Player;

    public Text TextFireballCharges;

    public Text TextBurningVisionCharges;

    public Text TextTeleportCharges;

    public Text TextRingOfFireCharges;

    public Slider SliderFireballCooldown;

    public Slider SliderBurningVisionCooldown;

    public Slider SliderTeleportCooldown;

    public Slider SliderRingOfFireCooldown;

    public Slider Healthbar;

    public Text Killcount;

    public GameObject DeathText;

    public GameObject BottomPanel;

    private KillManager killManager;

    public Image HealthbarFill;

    public GameObject FireballSelection;

    public GameObject BurningVisionSelection;

    public GameObject TeleportSelection;

    private void Awake()
    {
        killManager = GetComponent<KillManager>();
    }

    void Update()
    {
        Killcount.text = killManager.KillCount.ToString();

        Healthbar.value = Mathf.Clamp01(Player.Health / 100.0f);

        TextFireballCharges.text = Player.FireballChargesLeft.ToString();
        TextBurningVisionCharges.text = Player.BurningVisionChargesLeft.ToString();
        TextTeleportCharges.text = Player.TeleportChargesLeft.ToString();
        TextRingOfFireCharges.text = Player.RingOfFireChargesLeft.ToString();

        SliderFireballCooldown.value = Mathf.Clamp01((Time.time - Player.timeStampFireballCast) / Player.FireballCooldown);
        SliderBurningVisionCooldown.value = Mathf.Clamp01((Time.time - Player.timeStampBurningVisionCast) / Player.BurningVisionCooldown);
        SliderTeleportCooldown.value = Mathf.Clamp01((Time.time - Player.timeStampTeleportCast) / Player.TeleportCooldown);
        SliderRingOfFireCooldown.value = Mathf.Clamp01((Time.time - Player.timeStampRingOfFireCast) / Player.RingOfFireCooldown);

        if (Player.IsDead)
        {
            DeathText.SetActive(true);
            BottomPanel.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        if (Player.Health <= 30)
        {
            HealthbarFill.color = new Color(1, 0, 0, 0.216f);
        }

        FireballSelection.SetActive(Player.playerControls.SuperpowerToCast == Player.Superpowers.Fireball);
        BurningVisionSelection.SetActive(Player.playerControls.SuperpowerToCast == Player.Superpowers.BurningVision);
        TeleportSelection.SetActive(Player.playerControls.SuperpowerToCast == Player.Superpowers.Teleport);
    }
}
