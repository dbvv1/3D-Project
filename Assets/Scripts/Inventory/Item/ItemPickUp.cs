using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //往背包中加入物品  + 刷新背包 + 删除物品  （判断背包是否已满）
            bool add = false;
            switch (itemData.itemType)
            {
                case ItemType.Consumable:
                    add = InventoryManager.Instance.consumableInventory.AddItem(itemData);
                    InventoryManager.Instance.consumableContainer.RefreshContainerUI();
                    break;
                case ItemType.PrimaryWeapon:
                case ItemType.SecondaryWeapon:
                    add = InventoryManager.Instance.equipmentsInventory.AddItem(itemData);
                    InventoryManager.Instance.equipmentsContainer.RefreshContainerUI();
                    break;
            }

            //只有在add成功的情况下才删除物品
            if (add)
            {
                TaskManager.Instance.UpdateTaskProgress(itemData.itemName, 1);
                Destroy(gameObject);
            }
        }
    }
}
