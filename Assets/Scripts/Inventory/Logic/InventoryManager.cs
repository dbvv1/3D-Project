using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryManager : Singleton<InventoryManager>, ISavable
{
    [Header("背包数据")]
    public InventoryData_SO consumableInventory;

    public InventoryData_SO equipmentsInventory;

    public InventoryData_SO playerEquipmentInventory;

    public InventoryData_SO actionInventory;

    [Header("背包容器")] public ContainerUI consumableContainer; //消耗品背包

    public ContainerUI equipmentsContainer; //装备背包

    public ContainerUI actionContainer; //快捷栏上的背包
    
    public ContainerUI playerEquipmentContainer; //人物正在装备的装备背包

    private ContainerUI currentShowContainer; //当前选择的背包 (指角色面板中的背包选项,不包括人物正在穿的装备)

    [Header("引用")] 
    public Transform actionOnGameTransform;

    public Transform actionOnUITransform;
    
    public TextMeshProUGUI titleText;

    public Tooltip itemTooltip;

    [Header("拖拽相关")] public Canvas dragCanvas;

    [HideInInspector] public SlotHolder dragOriginalSlot;

    private Dictionary<string, ItemData_SO> itemNameToItemData = new Dictionary<string, ItemData_SO>();

    public ContainerUI CurrentShowContainer
    {
        get => currentShowContainer;
        set
        {
            currentShowContainer = value;
            if (value == consumableContainer) SelectConsumableContainer();
            else if (value == equipmentsContainer) SelectEquipmentsContainer();
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GameManager.Instance.gameConfig.InitItemDict(itemNameToItemData);
        CurrentShowContainer = consumableContainer;
        //对所有背包进行 清空+刷新 
        consumableInventory.ClearInventory();
        equipmentsInventory.ClearInventory();
        playerEquipmentInventory.ClearInventory();
        actionInventory.ClearInventory();
        RefreshAllContainer();
    }

    private void OnEnable()
    {
        ((ISavable)this).RegisterSaveData();
    }

    private void OnDisable()
    {
        ((ISavable)this).UnRegisterSaveData();
    }

    public void SetActionContainerParent(PanelType panelType)
    {
        switch (panelType)
        {
            case PanelType.None:            
            case PanelType.Skill:
            case PanelType.QuestTask:
                actionContainer.transform.SetParent(actionOnGameTransform);
                actionContainer.transform.localScale = new Vector3(1, 1, 1);
                actionContainer.transform.localPosition = Vector3.zero;
                break;
            case PanelType.Inventory:
                actionContainer.transform.SetParent(actionOnUITransform);
                actionContainer.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                actionContainer.transform.localPosition = new Vector3(44, -344, 0);
                break;
        }
    }

    #region 切换不同的背包界面

    public void OnButtonClickConsumableContainer()
    {
        SelectConsumableContainer();
    }

    public void OnButtonClickEquipmentsContainer()
    {
        SelectEquipmentsContainer();
    }

    private void SelectConsumableContainer()
    {
        consumableContainer.gameObject.SetActive(true);
        equipmentsContainer.gameObject.SetActive(false);
        titleText.text = "消耗品";
    }

    private void SelectEquipmentsContainer()
    {
        consumableContainer.gameObject.SetActive(false);
        equipmentsContainer.gameObject.SetActive(true);
        titleText.text = "装备";
    }

    #endregion

    #region 检查当前鼠标位置是否位于一个SlotHoler之内 （用于实现拖拽和交换）

    public bool CheckSlot(Vector3 position)
    {
        bool mouseInSlot = false;
        mouseInSlot |= CheckInSpecifiedInventory(actionContainer, position);
        mouseInSlot |= CheckInSpecifiedInventory(playerEquipmentContainer, position);
        if (currentShowContainer.gameObject.activeInHierarchy)
            mouseInSlot |= CheckInSpecifiedInventory(currentShowContainer, position);
        return mouseInSlot;
    }

    private bool CheckInSpecifiedInventory(ContainerUI specifiedInventory, Vector3 position)
    {
        foreach (var t1 in specifiedInventory.slotHolders)
        {
            RectTransform t = t1.transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                return true;
        }

        return false;
    }

    #endregion

    public InventoryData_SO GetSpecialInventory(InventoryType inventoryType)
    {
        var res = inventoryType switch
        {
            InventoryType.ConsumableInventory => consumableInventory,
            InventoryType.EquipmentInventory => equipmentsInventory,
            InventoryType.PlayerEquipmentInventory => playerEquipmentInventory,
            InventoryType.ActionInventory => actionInventory,
            _ => null
        };
        return res;
    }

    public void RefreshAllContainer()
    {
        consumableContainer.RefreshContainerUI();
        equipmentsContainer.RefreshContainerUI();
        actionContainer.RefreshContainerUI();
        playerEquipmentContainer.RefreshContainerUI();
    }

    public ItemData_SO GetItemByName(string name)
    {
        if (itemNameToItemData.ContainsKey(name))
            return itemNameToItemData[name];
        else
            return null;
    }

    //消耗物品 
    public void TaskCostItem(InventoryItem costItem, int costAmount)
    {
        switch (costItem.itemData.itemType)
        {
            case ItemType.Consumable:
                //先从消耗物品背包中查找，再从快捷栏物品中查找
                TaskCostItem(costItem, costAmount, consumableInventory);
                if (costAmount != 0)
                    TaskCostItem(costItem, costAmount, actionInventory);
                
                consumableContainer.RefreshContainerUI();
                actionContainer.RefreshContainerUI();
                break;
            case ItemType.PrimaryWeapon:
            case ItemType.SecondaryWeapon:
                //从装备背包中查找
                TaskCostItem(costItem, costAmount, equipmentsInventory);

                equipmentsContainer.RefreshContainerUI();
                break;
        }

    }

    private void TaskCostItem(InventoryItem costItem, int costAmount, InventoryData_SO inventory)
    {
        foreach (var item in inventory.items.Where(item => item.itemData == costItem.itemData))
        {
            if (item.itemAmount >= costAmount)
            {
                item.itemAmount -= costAmount;
                costAmount = 0;
                break;
            }
            else
            {
                costAmount -= item.itemAmount;
                item.itemAmount = 0;
            }
        }
    }

    public void TaskRewardItem(InventoryItem rewardItem, int rewardAmount)
    {
        switch (rewardItem.itemData.itemType)
        {
            case ItemType.Consumable:
                consumableInventory.AddItem(rewardItem.itemData, rewardItem.itemAmount);
                consumableContainer.RefreshContainerUI();
                break;
            case ItemType.PrimaryWeapon:
            case ItemType.SecondaryWeapon:
                equipmentsInventory.AddItem(rewardItem.itemData, rewardItem.itemAmount);
                equipmentsContainer.RefreshContainerUI();
                break;
        }
    }

    #region Save 相关接口

    public string GetDataID()
    {
        return "Inventory";
    }

    public void SaveData(Data data)
    {
        //Save所有的背包
        data.consumableInventory.SaveInventory(consumableInventory);
        data.equipmentsInventory.SaveInventory(equipmentsInventory);
        data.playerEquipmentInventory.SaveInventory(playerEquipmentInventory);
        data.actionInventory.SaveInventory(actionInventory);
    }

    public void LoadData(Data data)
    {
        //Load所有的背包
        consumableInventory.LoadInventory(data.consumableInventory);
        equipmentsInventory.LoadInventory(data.equipmentsInventory);
        playerEquipmentInventory.LoadInventory(data.playerEquipmentInventory);
        actionInventory.LoadInventory(data.actionInventory);
        //刷新所有的背包
        RefreshAllContainer();
    }

    #endregion

    #region 检测任务物品

    public void CheckTaskItemInBag(string requireItemName)
    {
        foreach (var inventoryItem in consumableInventory.items)
        {
            if (inventoryItem.itemData != null)
            {
                if (inventoryItem.itemData.itemName == requireItemName)
                {
                    TaskManager.Instance.UpdateTaskProgress(requireItemName, inventoryItem.itemAmount);
                }
            }
        }

        foreach (var inventoryItem in actionInventory.items)
        {
            if (inventoryItem.itemData != null)
            {
                if (inventoryItem.itemData.itemName == requireItemName)
                {
                    TaskManager.Instance.UpdateTaskProgress(requireItemName, inventoryItem.itemAmount);
                }
            }
        }

        foreach (var inventoryItem in equipmentsInventory.items)
        {
            if (inventoryItem.itemData != null)
            {
                if (inventoryItem.itemData.itemName == requireItemName)
                {
                    TaskManager.Instance.UpdateTaskProgress(requireItemName, inventoryItem.itemAmount);
                }
            }
        }
    }

    #endregion

    //检测背包和快捷栏中的物品
}