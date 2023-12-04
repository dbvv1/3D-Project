using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

//背包的数据
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    [JsonProperty] public List<InventoryItem> items;

    public int itemCapacity; //当前背包的容量是多少 

    public int itemCount; //当前背包中一共有多少物品

    //在当前背包中添加物品
    public bool AddItem(ItemData_SO itemData, int itemAmount = 1)
    {
        if (itemCount == itemCapacity) return false;
        //可堆叠的情况
        if (itemData.stackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                //直接进行叠加操作
                if (items[i].itemData == itemData)
                {
                    items[i].itemAmount += itemAmount;
                    return true;
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemData == null)
                {
                    items[i].itemData = itemData;
                    items[i].itemAmount = itemAmount;
                    itemCount++;
                    return true;
                }
            }
        }
        else
        {
            //不可堆叠的情况：需要在背包中新添加这个物品
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemData == null)
                {
                    items[i].itemData = itemData;
                    items[i].itemAmount = 1;
                    itemCount++;
                    if (--itemAmount == 0) break;
                }
            }
        }

        return itemAmount == 0;
    }
    

    //对外提供复制操作
    public void SaveInventory(InventoryData_SO inventoryDataSo)
    {
        var targetItems = inventoryDataSo.items;
        items = new List<InventoryItem>(targetItems.Count);
        
        for (int i = 0; i < targetItems.Count; i++)
        {
            items.Add(new InventoryItem(targetItems[i].itemData, targetItems[i].itemAmount));
        }

        itemCapacity = inventoryDataSo.itemCapacity;
        itemCount = inventoryDataSo.itemCount;
    }

    public void LoadInventory(InventoryData_SO inventoryDataSo)
    {
        var targetItems = inventoryDataSo.items;
        for (int i = 0; i < targetItems.Count; i++)
        {
            if (targetItems[i].itemData != null)
                items[i].itemData = InventoryManager.Instance.GetItemByName(targetItems[i].itemData.itemName);
            else
                items[i].itemData = null;
            items[i].itemAmount = targetItems[i].itemAmount;
        }

        itemCapacity = inventoryDataSo.itemCapacity;
        itemCount = inventoryDataSo.itemCount;
    }

    //清空背包
    public void ClearInventory()
    {
        itemCount = 0;
        foreach (var inventoryItem in items)
        {
            inventoryItem.itemData = null;
            inventoryItem.itemAmount = 0;
        }
    }
}

//位于背包中的物品： 包含物品Data 和 数量amount
[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;

    public int itemAmount;

    public InventoryItem()
    {
    }

    public InventoryItem(ItemData_SO itemData, int itemAmount)
    {
        this.itemData = itemData;
        this.itemAmount = itemAmount;
    }
}