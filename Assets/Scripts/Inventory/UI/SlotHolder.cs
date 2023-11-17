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

    //ʹ����Ʒ
    private void UseItem(ItemData_SO itemData)
    {
        itemData.usableItemData.OnUse();
    }

    //TODO:װ����Ҫ����
    private void EquipPrimaryWeaponWeapon(ItemData_SO itemData)
    {
        
    }

    //TODO:װ����Ҫ����
    private void EquipSecondaryWeapon(ItemData_SO itemData)
    {
        
    }

    //ͨ��SlotType��ȡ��Ӧ�ı������ݿ� �������������е�ItemUI
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
        //ͨ���������ݿ� �ҵ�ItemUI�±��Ӧ����Ʒ ��ItemUI���и���
        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetUpItemUI(item.itemData, item.itemAmount);
    }

    //�����룺��ʾ��Ʒ��Ϣ
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItemData != null)
        {
            InventoryManager.Instance.itemTooltip.gameObject.SetActive(true);
            InventoryManager.Instance.itemTooltip.SetItemText(itemUI.itemData);
        }
    }

    //����뿪
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
    }
}
