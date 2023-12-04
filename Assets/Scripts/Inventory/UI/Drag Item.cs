using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ItemUI currentItemUI;

    private SlotHolder currentSlot;

    private SlotHolder targetSlot;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentSlot = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录原始数据
        InventoryManager.Instance.dragOriginalSlot = currentSlot;

        transform.SetParent(InventoryManager.Instance.dragCanvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标位置移动
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品 交换数据
        //判断鼠标下方是否指向的是UI物体
        if(EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.Instance.CheckSlot(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetSlot = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                else
                    targetSlot = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                //根据目标格子的类型进行交换
                if (targetSlot != null)
                {
                    switch (targetSlot.slotType)
                    {
                        case SlotType.ConsumableBag:
                            if (currentItemUI.itemData.itemType == ItemType.Consumable) SwapItem();
                            break;
                        case SlotType.EquipmentBag:
                            if (currentItemUI.itemData.itemType != ItemType.Consumable) SwapItem();
                            break;
                        case SlotType.PrimaryWeapon:
                            if(currentItemUI.itemData.itemType== ItemType.PrimaryWeapon) SwapItem();
                            break;
                        case SlotType.SecondaryWeapon:
                            if (currentItemUI.itemData.itemType == ItemType.SecondaryWeapon) SwapItem();
                            break;
                        case SlotType.Action:
                            if (currentItemUI.itemData.itemType == ItemType.Consumable) SwapItem();
                            break;
                    }
                    currentSlot.UpdateItem();
                    targetSlot.UpdateItem();
                }
            }
        }
        transform.SetParent(InventoryManager.Instance.dragOriginalSlot.transform);
        RectTransform t = transform as RectTransform;
        t.offsetMax = Vector2.zero;
        t.offsetMin = Vector2.zero;
    }

    private void SwapItem()
    {
        //对自我交换进行处理
        if (targetSlot == currentSlot) return;

        //得到背包中实际的物品
        InventoryItem targetItem = targetSlot.itemUI.GetInventoryItem;
        InventoryItem tmpItem = currentSlot.itemUI.GetInventoryItem;

        //处理可叠加的情况 (同一种物品 + 物品可堆叠 )
        if(targetItem.itemData==tmpItem.itemData && targetItem.itemData.stackable)
        {
            targetItem.itemAmount += tmpItem.itemAmount;
            tmpItem.itemData = null; tmpItem.itemAmount = 0;
        }
        //不可叠加 进行交换
        else
        {
            targetSlot.itemUI.Bag.items[targetSlot.itemUI.Index] = tmpItem;
            currentSlot.itemUI.Bag.items[currentSlot.itemUI.Index] = targetItem;
        }
    }
}
