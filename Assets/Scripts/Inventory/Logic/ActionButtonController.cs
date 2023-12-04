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
            //向左滑动快捷栏
            SwapActionItem(leftActionSlot,rightActionSlot);
            SwapActionItem(mainActionSlot,rightActionSlot);
            UpdateActionUI();
        }
        else if(scroll < 0f)
        {
            //向右滑动快捷栏
            SwapActionItem(leftActionSlot,rightActionSlot);
            SwapActionItem(mainActionSlot,leftActionSlot);
            UpdateActionUI();
        }
        
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            //使用MainActionButton上的物品
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

    //在滚轮滑动时 执行物品的交换操作
    private void SwapActionItem(SlotHolder currentSlot, SlotHolder targetSlot)
    {
        //对自我交换进行处理
        if (targetSlot == currentSlot) return;

        //得到背包中实际的物品
        InventoryItem targetItem = targetSlot.itemUI.GetInventoryItem;
        InventoryItem tmpItem = currentSlot.itemUI.GetInventoryItem;

        //交换物品
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
