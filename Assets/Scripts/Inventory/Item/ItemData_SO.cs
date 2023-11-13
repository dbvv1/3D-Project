using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

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


}
