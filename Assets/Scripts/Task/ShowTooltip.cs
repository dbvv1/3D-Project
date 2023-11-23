using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip :MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    private ItemUI currentItemUI;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //设置task中的tooltip
        TaskUIManager.Instance.itemTooltip.gameObject.SetActive(true);
        TaskUIManager.Instance.itemTooltip.SetItemText(currentItemUI.GetItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //设置task中的tooltip
        TaskUIManager.Instance.itemTooltip.gameObject.SetActive(false);
        TaskUIManager.Instance.itemTooltip.SetItemText(null);
    }
}
