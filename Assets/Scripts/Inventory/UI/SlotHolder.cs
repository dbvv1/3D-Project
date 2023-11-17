using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;

    public ItemUI itemUI;


    public void OnPointerClick(PointerEventData eventData)
    {
        ItemData_SO itemData = itemUI.GetItemData;
        if (eventData.clickCount >= 2 && itemData != null && itemUI.GetItemAmount >= 1)   
        {
            switch (itemData.itemType)
            {
                case ItemType.Consumable:
                    UseItem(itemData);
                    break;
                case ItemType.PrimaryWeapon:
                    EquipPrimaryWeaponWeapon(itemData);
                    break;
                case ItemType.SecondaryWeapon:
                    EquipSecondaryWeapon(itemData);
                    break;

            }

            itemUI.GetInventoryItem.itemAmount--;
            if(itemUI.GetInventoryItem.itemAmount==0) InventoryManager.Instance.itemTooltip.gameObject.SetActive(false); 
            UpdateItem();
        }
    }

    //使用物品
    private void UseItem(ItemData_SO itemData)
    {
        itemData.usableItemData.OnUse();
    }

    //TODO:装备主要武器
    private void EquipPrimaryWeaponWeapon(ItemData_SO itemData)
    {
        
    }

    //TODO:装备次要武器
    private void EquipSecondaryWeapon(ItemData_SO itemData)
    {
        
    }

    //通过SlotType获取对应的背包数据库 依次来更新其中的ItemUI
    public void UpdateItem()
    {
        itemUI.Bag = slotType switch
        {
            SlotType.ConsumableBag => InventoryManager.Instance.consumableInventory,
            SlotType.EquipmentBag => InventoryManager.Instance.equipmentsInventory,
            SlotType.PrimaryWeapon => InventoryManager.Instance.playerEquipmentInventory,
            SlotType.SecondaryWeapon => InventoryManager.Instance.playerEquipmentInventory,
            SlotType.Action => InventoryManager.Instance.actionInventory,
            _ => null
        };
        //通过背包数据库 找到ItemUI下标对应的物品 对ItemUI进行更新
        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetUpItemUI(item.itemData, item.itemAmount);
    }

    //鼠标进入：显示物品信息
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItemData != null)
        {
            InventoryManager.Instance.itemTooltip.gameObject.SetActive(true);
            InventoryManager.Instance.itemTooltip.SetItemText(itemUI.itemData);
        }
    }

    //鼠标离开
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
    }
}
