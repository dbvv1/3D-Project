using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIManager : Singleton<PlayerStatsUIManager>
{
    [Header("����״̬��")]
    public Slider healthSlider;
    public Slider magicSlider;
    public Slider energySlider;

    [Header("�������״̬��ʾ")]
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI maxEnergyText;
    public TextMeshProUGUI maxMagicText;

    private CharacterStats playerCurrentStats;

    private void Start()
    {
        playerCurrentStats = GameManager.Instance.playerCurrentStats;
    }

    public void UpdateHealthSlider()
    {
        healthSlider.value = playerCurrentStats.CurHealth / playerCurrentStats.MaxHealth;
    }

    public void UpdateMagicSlider()
    {
        magicSlider.value = playerCurrentStats.CurMagic / playerCurrentStats.MaxMagic;
    }

    public void UpdateEnergySlider()
    {
        energySlider.value = playerCurrentStats.CurEnergy / playerCurrentStats.MaxEnergy;
    }

    public void RefreshMaxInformation()
    {
        maxHealthText.text = playerCurrentStats.MaxHealth.ToString("F0");
        maxEnergyText.text = playerCurrentStats.MaxEnergy.ToString("F0");
        maxMagicText.text = playerCurrentStats.MaxMagic.ToString("F0");
    }

}
