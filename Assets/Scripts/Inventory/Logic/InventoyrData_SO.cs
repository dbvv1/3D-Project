using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����������
[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/Inventory Data")]
public class InventoyrData_SO : ScriptableObject
{
    public List<InventoryItem> items;

    public int itemCapacity;   //��ǰ�����������Ƕ��� 
     
    public int itemCount;      //��ǰ������һ���ж�����Ʒ

    //�ڵ�ǰ�����������Ʒ
    public bool AddItem(ItemData_SO itemData)
    {
        if (itemCount == itemCapacity) return false;
        //�ɶѵ������
        if (itemData.statckable)
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
}

//λ�ڱ����е���Ʒ�� ������ƷData �� ����amount
[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;

    public int itemAmount;
}
