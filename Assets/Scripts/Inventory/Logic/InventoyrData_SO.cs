using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//背包的数据
[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/Inventory Data")]
public class InventoyrData_SO : ScriptableObject
{
    public List<InventoryItem> items;

    public int itemCapacity;   //当前背包的容量是多少 
     
    public int itemCount;      //当前背包中一共有多少物品

    //在当前背包中添加物品
    public bool AddItem(ItemData_SO itemData)
    {
        if (itemCount == itemCapacity) return false;
        //可堆叠的情况
        if (itemData.statckable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                //直接进行叠加操作
                if (items[i].itemData == itemData)
                {
                    items[i].itemAmount++;
                    return true;
                }
            }
        }
        //需要在背包中新添加这个物品
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
        //没有添加成功，说明背包满了 
        return false;
    }
}

//位于背包中的物品： 包含物品Data 和 数量amount
[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;

    public int itemAmount;
}
