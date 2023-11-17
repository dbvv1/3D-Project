using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIManager : Singleton<PlayerStatsUIManager>
{
    [Header("人物状态栏")]
    public Slider healthSlider;
    public Slider magicSlider;
    public Slider energySlider;

    [Header("人物面板状态显示")]
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

    public void UpdateSliderWidth(float maxHealthChange,float maxEnergyChange,float maxMagicChange)
    {
        UpdateHealthSliderWidth(maxHealthChange);
        UpdateEnergySliderWidth(maxEnergyChange);
        UpdateMagicSliderWidth(maxMagicChange);
    }

    private void UpdateHealthSliderWidth(float maxHealthChange)
    {
        float pre = playerCurrentStats.MaxHealth - maxHealthChange;
        float cur = playerCurrentStats.MaxHealth;
        Vector2 healthSlideSize = ((RectTransform)healthSlider.transform).sizeDelta;
        healthSlideSize.x *= (1 + (cur - pre) / pre);
        ((RectTransform)healthSlider.transform).sizeDelta = healthSlideSize;
    }

    private void UpdateEnergySliderWidth(float maxEnergyChange)
    {
        float pre = playerCurrentStats.MaxEnergy - maxEnergyChange;
        float cur = playerCurrentStats.MaxEnergy;
        Vector2 energySlideSize = ((RectTransform)energySlider.transform).sizeDelta;
        energySlideSize.x *= (1 + (cur - pre) / pre);
        ((RectTransform)energySlider.transform).sizeDelta = energySlideSize;
    }

    private void UpdateMagicSliderWidth(float maxMagicChange)
    {
        float pre = playerCurrentStats.MaxMagic - maxMagicChange;
        float cur = playerCurrentStats.MaxMagic;
        Vector2 magicSlideSize = ((RectTransform)magicSlider.transform).sizeDelta;
        magicSlideSize.x *= (1 + (cur - pre) / pre);
        ((RectTransform)magicSlider.transform).sizeDelta = magicSlideSize;
    }

    public void RefreshMaxInformation()
    {
        maxHealthText.text = playerCurrentStats.MaxHealth.ToString("F0");
        maxEnergyText.text = playerCurrentStats.MaxEnergy.ToString("F0");
        maxMagicText.text = playerCurrentStats.MaxMagic.ToString("F0");
    }

}
