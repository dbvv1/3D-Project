using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����������
[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items;

    public int itemCapacity;   //��ǰ�����������Ƕ��� 
     
    public int itemCount;      //��ǰ������һ���ж�����Ʒ

    //�ڵ�ǰ�����������Ʒ
    public bool AddItem(ItemData_SO itemData)
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
                    items[i].itemAmount++;
                    return true;
                }
            }
        }
        //��Ҫ�ڱ���������������Ʒ
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemData==null)
            {
                items[i].itemData = itemData;
                items[i].itemAmount = 1;
                itemCount++;
                return true;
            }
        }
        //û����ӳɹ���˵���������� 
        return false;
    }
    
    //�����ṩ���Ƴ�ʼ������
    public void SettingInventory(InventoryData_SO inventoryDataSo)
    {
        var targetItems = inventoryDataSo.items;
        items = new List<InventoryItem>(targetItems.Count);
        for (int i = 0; i < targetItems.Count; i++)
        {
            var item = new InventoryItem(targetItems[i].itemData, targetItems[i].itemAmount);
            items.Add(item);
        }
        itemCapacity = inventoryDataSo.itemCapacity;
        itemCount = inventoryDataSo.itemCount;
    }
}

//λ�ڱ����е���Ʒ�� ������ƷData �� ����amount
[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;

    public int itemAmount;
    
    public InventoryItem(){}

    public InventoryItem(ItemData_SO itemData, int itemAmount)
    {
        this.itemData = itemData;
        this.itemAmount = itemAmount;
    }
    
}
