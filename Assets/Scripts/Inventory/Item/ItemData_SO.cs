using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ʒ��������Ϣ
[CreateAssetMenu(fileName ="New Item",menuName ="Item/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;        //��Ʒ����

    public string itemName;          //��Ʒ����

    public Sprite itemIcon;          //��Ʒͼ��

    [TextArea]
    public string itemDescription;   //��Ʒ��ϸ����

    public bool statckable;          //��Ʒ�Ƿ���Ա��ѵ�


}
