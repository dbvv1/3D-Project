using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ʒ��������Ϣ
[CreateAssetMenu(fileName ="New Item",menuName ="Item/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;        //��Ʒ����

    public string itemName;          //��Ʒ����
    
    public SerializeSprite serializeSprite;          //��Ʒͼ��

    [TextArea]
    public string itemDescription;   //��Ʒ��ϸ����

    public bool stackable;          //��Ʒ�Ƿ���Ա��ѵ�

    public UseableItemData_SO usableItemData;          //��Ʒ��ʹ�õ�������Ϣ

    public void InitItemData(ItemData_SO itemData)
    {
        itemType = itemData.itemType;
        itemName = itemData.itemName;
        serializeSprite = itemData.serializeSprite;
        itemDescription = itemData.itemDescription;
        stackable = itemData.stackable;
        usableItemData = ScriptableObject.CreateInstance<UseableItemData_SO>();
        usableItemData.InitUsableItemData(itemData.usableItemData);
    }
    
}
