using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public InventoyrData_SO Bag { get; set; }       //物品所属与哪个背包
    public int Index { get; set; } = -1;            //物品在背包中的下标值

    [HideInInspector]public ItemData_SO itemData;

    [SerializeField]private Image itemIcon;

    [SerializeField]private TextMeshProUGUI amountText;


    public void SetUpItemUI(ItemData_SO itemData, int amount)
    {
        if(amount==0)
        {
            Bag.items[Index].itemData = null;
            this.itemData = null;
            itemIcon.gameObject.SetActive(false);
            return;
        }

        if(itemData!=null)
        {
            itemIcon.sprite = itemData.itemIcon;
            amountText.text = amount.ToString();
            itemIcon.gameObject.SetActive(true);
            this.itemData = itemData;
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }
    }

    #region 获取实际物品的接口

    public InventoryItem GetInventoryItem => Bag.items[Index];

    public ItemData_SO GetItemData => Bag.items[Index].itemData;

    #endregion

}
