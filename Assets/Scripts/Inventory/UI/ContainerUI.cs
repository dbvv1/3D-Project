using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;

    //刷新每个格子的内容 在获取物品的时候调用
    public void RefreshContainerUI()
    {
        for(int i=0;i<slotHolders.Length;i++)
        {
            slotHolders[i].itemUI.Index = i;
            slotHolders[i].UpdateItem();
        }
    }
}
