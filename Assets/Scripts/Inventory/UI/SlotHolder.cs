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
        if(eventData.clickCount>=2)
        {
            UseItem();
        }
    }

    //使用物品
    private void UseItem()
    {
        if(itemUI.itemData!=null&&itemUI.GetItemData.itemType==ItemType.Consumable)
        {
            //TODO：使用物品的具体实现
            itemUI.GetInventoryItem.itemAmount--;
        }
        UpdateItem();
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
