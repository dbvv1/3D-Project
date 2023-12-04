using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ActionButtonController : MonoBehaviour
{
    [SerializeField] private  SlotHolder mainActionSlot;
    
    [SerializeField] private  SlotHolder leftActionSlot;
    
    [SerializeField] private  SlotHolder rightActionSlot;

    private float scroll;
    
    private void Update()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if(scroll > 0f)
        {
            //���󻬶������
            SwapActionItem(leftActionSlot,rightActionSlot);
            SwapActionItem(mainActionSlot,rightActionSlot);
            UpdateActionUI();
        }
        else if(scroll < 0f)
        {
            //���һ��������
            SwapActionItem(leftActionSlot,rightActionSlot);
            SwapActionItem(mainActionSlot,leftActionSlot);
            UpdateActionUI();
        }
        
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            //ʹ��MainActionButton�ϵ���Ʒ
            var itemData = mainActionSlot.itemUI.GetItemData;
            if (itemData != null)
            {
                UseActionItem(itemData);
            }
        }
    }

    private void UseActionItem(ItemData_SO itemData)
    {
        itemData.usableItemData.OnUse();
        mainActionSlot.itemUI.GetInventoryItem.itemAmount--;
        TaskManager.Instance.UpdateTaskProgress(itemData.itemName, -1);
        if(mainActionSlot.itemUI.GetInventoryItem.itemAmount==0) InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
        mainActionSlot.UpdateItem();
    }

    //�ڹ��ֻ���ʱ ִ����Ʒ�Ľ�������
    private void SwapActionItem(SlotHolder currentSlot, SlotHolder targetSlot)
    {
        //�����ҽ������д���
        if (targetSlot == currentSlot) return;

        //�õ�������ʵ�ʵ���Ʒ
        InventoryItem targetItem = targetSlot.itemUI.GetInventoryItem;
        InventoryItem tmpItem = currentSlot.itemUI.GetInventoryItem;

        //������Ʒ
        targetSlot.itemUI.Bag.items[targetSlot.itemUI.Index] = tmpItem;
        currentSlot.itemUI.Bag.items[currentSlot.itemUI.Index] = targetItem;

    }

    private void UpdateActionUI()
    {
        mainActionSlot.UpdateItem();
        leftActionSlot.UpdateItem();
        rightActionSlot.UpdateItem();
    }

}
