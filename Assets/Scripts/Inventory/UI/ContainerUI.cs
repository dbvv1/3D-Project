using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;

    //ˢ��ÿ�����ӵ����� �ڻ�ȡ��Ʒ��ʱ�����
    public void RefreshContainerUI()
    {
        for(int i=0;i<slotHolders.Length;i++)
        {
            slotHolders[i].itemUI.Index = i;
            slotHolders[i].UpdateItem();
        }
    }
}
