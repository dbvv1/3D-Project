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
        //��¼ԭʼ����
        InventoryManager.Instance.dragOriginalSlot = currentSlot;

        transform.SetParent(InventoryManager.Instance.dragCanvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //�������λ���ƶ�
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //������Ʒ ��������
        //�ж�����·��Ƿ�ָ�����UI����
        if(EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.Instance.CheckSlot(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetSlot = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                else
                    targetSlot = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                //����Ŀ����ӵ����ͽ��н���
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
        //�����ҽ������д���
        if (targetSlot == currentSlot) return;

        //�õ�������ʵ�ʵ���Ʒ
        InventoryItem targetItem = targetSlot.itemUI.GetInventoryItem;
        InventoryItem tmpItem = currentSlot.itemUI.GetInventoryItem;

        //����ɵ��ӵ���� (ͬһ����Ʒ + ��Ʒ�ɶѵ� )
        if(targetItem.itemData==tmpItem.itemData && targetItem.itemData.stackable)
        {
            targetItem.itemAmount += tmpItem.itemAmount;
            tmpItem.itemData = null; tmpItem.itemAmount = 0;
        }
        //���ɵ��� ���н���
        else
        {
            targetSlot.itemUI.Bag.items[targetSlot.itemUI.Index] = tmpItem;
            currentSlot.itemUI.Bag.items[currentSlot.itemUI.Index] = targetItem;
        }
    }
}
