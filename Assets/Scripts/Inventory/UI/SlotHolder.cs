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

    //ʹ����Ʒ
    private void UseItem()
    {
        if(itemUI.itemData!=null&&itemUI.GetItemData.itemType==ItemType.Consumable)
        {
            //TODO��ʹ����Ʒ�ľ���ʵ��
            itemUI.GetInventoryItem.itemAmount--;
        }
        UpdateItem();
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
