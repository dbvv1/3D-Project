using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

//����������
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    [JsonProperty] public List<InventoryItem> items;

    public int itemCapacity; //��ǰ�����������Ƕ��� 

    public int itemCount; //��ǰ������һ���ж�����Ʒ

    //�ڵ�ǰ�����������Ʒ
    public bool AddItem(ItemData_SO itemData, int itemAmount = 1)
    {
        if (itemCount == itemCapacity) return false;
        //�ɶѵ������
        if (itemData.stackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                //ֱ�ӽ��е��Ӳ���
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
            //���ɶѵ����������Ҫ�ڱ���������������Ʒ
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
    

    //�����ṩ���Ʋ���
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

    //��ձ���
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

//λ�ڱ����е���Ʒ�� ������ƷData �� ����amount
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