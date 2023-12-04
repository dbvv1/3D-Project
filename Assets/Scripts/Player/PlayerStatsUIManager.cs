using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIManager : Singleton<PlayerStatsUIManager>
{
    [Header("人物状态栏")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider magicSlider;
    [SerializeField] private Slider energySlider;

    [Header("人物面板状态显示")]
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI maxEnergyText;
    [SerializeField] private TextMeshProUGUI maxMagicText;
    [SerializeField] private TextMeshProUGUI moneyAmountText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expInfoText;
    [SerializeField] private Slider expSlider;
    
    private PlayerCharacterStats playerCurrentStats;

    private void Start()
    {
        playerCurrentStats = GameManager.Instance.playerCurrentStats;
        expInfoText.text = playerCurrentStats.CurExp + "/" + playerCurrentStats.CurNeedExp;
    }
    

    public void UpdateUserUIInfo()
    {
        UpdateMoneyAmountInfo();
        UpdateLevelInfo();
        RefreshMaxInformation();
    }

    public void UpdateMoneyAmountInfo()
    {
        moneyAmountText.text = playerCurrentStats.Money.ToString();
    }

    public void UpdateLevelInfo()
    {
        levelText.text=playerCurrentStats.CurLevel.ToString();
        expInfoText.text = playerCurrentStats.CurExp + " / " + playerCurrentStats.CurNeedExp;
        expSlider.value = playerCurrentStats.CurExp * 1.0f / playerCurrentStats.CurNeedExp;
    }

    public void UpdateSliderValue()
    {
        UpdateHealthSlider();
        UpdateMagicSlider();
        UpdateEnergySlider();

    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = playerCurrentStats.CurHealth / playerCurrentStats.MaxHealth;
    }

    private void UpdateMagicSlider()
    {
        magicSlider.value = playerCurrentStats.CurMagic / playerCurrentStats.MaxMagic;
    }

    private void UpdateEnergySlider()
    {
        energySlider.value = playerCurrentStats.CurEnergy / playerCurrentStats.MaxEnergy;
    }
    
    /// <summary>
    /// 更新左上角状态栏的Slider的长度
    /// </summary>
    /// <param name="maxHealthChange">最大生命值的变化</param>
    /// <param name="maxEnergyChange">最大能量值的变化</param>
    /// <param name="maxMagicChange">最大魔法值的变化</param>
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
