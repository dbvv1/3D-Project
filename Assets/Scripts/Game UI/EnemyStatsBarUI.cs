using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnemyStatsBarUI : MonoBehaviour
{
    [SerializeField] private GameObject healthUIBarPrefab;

    [SerializeField] private GameObject energyUIBarPrefab;
    
    [SerializeField] private Transform healthBarPoint;

    [SerializeField] private Transform energyBarPoint;

    private Canvas worldCanvas;
    
    private Camera mainCamera;

    [SerializeField] public bool alwaysVisible;

    private Image healthSliderImage;
    
    private Image energySliderImage;

    private Transform healthUIBar;

    private Transform energyUIBar;

    public float showTime;

    private float showTimeCounter;

    private void Awake()
    {
        worldCanvas = UIManager.Instance.enemyStatsBarCanvas;
        mainCamera=Camera.main;
    }

    private void OnEnable()
    {
        healthUIBar = Instantiate(healthUIBarPrefab, worldCanvas.transform).transform;
        energyUIBar = Instantiate(energyUIBarPrefab, worldCanvas.transform).transform;
        healthSliderImage = healthUIBar.GetChild(0).GetComponent<Image>();
        energySliderImage = energyUIBar.GetChild(0).GetComponent<Image>(); 
        
        healthUIBar.gameObject.SetActive(alwaysVisible);
        energyUIBar.gameObject.SetActive(alwaysVisible);
    }

    private void OnDisable()
    {
        if(healthUIBar!=null) Destroy(healthUIBar.gameObject);
        if(energyUIBar!=null) Destroy(energyUIBar.gameObject);
    }

    public void UpdateStats(float curHealth, float maxHealth, float curEnergy, float maxEnergy)
    {
        if (curHealth <= 0)
        {
            if(healthUIBar!=null) Destroy(healthUIBar.gameObject);
            if(energyUIBar!=null) Destroy(energyUIBar.gameObject);
        }
        UpdateHealthBar(curHealth, maxHealth);
        UpdateEnergyBar(curEnergy, maxEnergy);
        if (!alwaysVisible)
        {
            //¿ªÆô¼ÆÊ±Æ÷
            showTimeCounter = showTime;
            StopAllCoroutines();
            StartCoroutine(UpdateShowTime());
        }
    }
    

    private void UpdateHealthBar(float curHealth, float maxHealth)
    {
        if (healthSliderImage != null) healthSliderImage.fillAmount = curHealth / maxHealth;
        if (healthUIBar != null) healthUIBar.gameObject.SetActive(true);
        
    }

    private void UpdateEnergyBar(float curEnergy, float maxEnergy)
    {
        if(energySliderImage!=null) energySliderImage.fillAmount = curEnergy / maxEnergy;
        if(energyUIBar!=null) energyUIBar.gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        if (healthUIBar != null)
        {
            healthUIBar.transform.position = healthBarPoint.position;
            healthUIBar.forward = -mainCamera.transform.forward;
        }

        if (energyUIBar != null)
        {
            energyUIBar.transform.position = energyBarPoint.position;
            energyUIBar.forward = -mainCamera.transform.forward;
        }
    }

    private IEnumerator UpdateShowTime()
    {
        while (showTimeCounter >= 0)
        {
            showTimeCounter -= Time.deltaTime;
            yield return null;
        }

        healthUIBar.gameObject.SetActive(false);
        energyUIBar.gameObject.SetActive(false);
    }
}