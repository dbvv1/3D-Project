using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

//物品的数据信息
[CreateAssetMenu(fileName ="New Item",menuName ="Item/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;        //物品类型

    public string itemName;          //物品名字
    
    public SerializeSprite serializeSprite;          //物品图标

    [TextArea]
    public string itemDescription;   //物品详细描述

    public bool stackable;          //物品是否可以被堆叠


}
