using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject healthUIBarPrefab;

    [SerializeField] private Transform healthBarPoint;
    
    private Canvas worldCanvas;

    [SerializeField] public bool alwaysVisible;

    private Image healthSliderImage;

    private Transform UIBar;

    public float showTime;

    private float showTimeCounter;

    private void Awake()
    {
        worldCanvas = UIManager.Instance.EnemyHealthBarCanvas;
    }

    private void OnEnable()
    {
        UIBar = Instantiate(healthUIBarPrefab, worldCanvas.transform).transform;
        healthSliderImage = UIBar.GetChild(0).GetComponent<Image>();
        UIBar.gameObject.SetActive(alwaysVisible);
    }

    private void OnDisable()
    {
        if(UIBar!=null)
        Destroy(UIBar.gameObject);
    }

    public void UpdateHealthBar(float curHealth,float maxHealth)
    {
        if(curHealth<=0)
        {
            if (UIBar != null)
                Destroy(UIBar.gameObject);
        }
        healthSliderImage.fillAmount = curHealth / maxHealth;
        if (UIBar != null) UIBar.gameObject.SetActive(true);
        if(!alwaysVisible)
        {
            //¿ªÆô¼ÆÊ±Æ÷
            showTimeCounter = showTime;
            StopAllCoroutines();
            StartCoroutine(UpdataShowTime());
        }
    }

    private void LateUpdate()
    {
        if (UIBar != null)
        {
            UIBar.transform.position = healthBarPoint.position;
            UIBar.forward = -Camera.main.transform.forward;
        }
    }

    private IEnumerator UpdataShowTime()
    {
        while(showTimeCounter>=0)
        {
            showTimeCounter -= Time.deltaTime;
            yield return null;
        }
        UIBar.gameObject.SetActive(false);
    }

}
