using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public InventoryData_SO Bag { get; set; } //物品所属与哪个背包
    public int Index { get; set; } = -1; //物品在背包中的下标值

    [HideInInspector] public ItemData_SO itemData;

    [SerializeField] private Image itemIcon;

    [SerializeField] private TextMeshProUGUI amountText;

    private async void LoadSprite()
    {
        var spriteAssetReference = new AssetReference(itemData.serializeSprite.spriteAddress);
        itemIcon.sprite = await spriteAssetReference.LoadAssetAsync<Sprite>().Task;
    }

    public void SetUpItemUI(ItemData_SO itemData, int amount)
    {
        if (amount == 0)
        {
            Bag.items[Index].itemData = null;
            this.itemData = null;
            itemIcon.gameObject.SetActive(false);
            itemIcon.sprite = null;
            return;
        }

        if (amount < 0) itemData = null;

        if (itemData != null)
        {
            this.itemData = itemData;
            LoadSprite();
            amountText.text = amount.ToString();
            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }
    }

    #region 获取实际物品的接口（从背包数据库中获取）

    public InventoryItem GetInventoryItem => Bag.items[Index];

    public int GetItemAmount => Bag.items[Index].itemAmount;

    //public ItemData_SO GetItemData => Bag.items[Index].itemData;
    public ItemData_SO GetItemData => itemData;

    #endregion
}