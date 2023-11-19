using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>,ISavable
{
    [Header("背包数据")]
    public InventoryData_SO consumableInventory;
    
    public InventoryData_SO equipmentsInventory;

    public InventoryData_SO playerEquipmentInventory;

    public InventoryData_SO actionInventory;

    [Header("背包容器")]
    public ContainerUI consumableContainer;        //消耗品背包

    public ContainerUI equipmentsContainer;        //装备背包

    public ContainerUI actionContainer;            //快捷栏上的背包

    public ContainerUI playerEquipmentContainer;   //人物正在装备的装备背包

    private ContainerUI currentShowContainer;      //当前选择的背包 (指角色面板中的背包选项,不包括人物正在穿的装备)

    [Header("引用")]
    public TextMeshProUGUI titleText;

    public Tooltip itemTooltip;

    [Header("拖拽相关")]
    public Canvas dragCanvas;

    [HideInInspector]public SlotHolder dragOriginalSlot;

    private Dictionary<string, ItemData_SO> itemNameToItemData = new Dictionary<string, ItemData_SO>();

    public ContainerUI CurrentShowContainer { 
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
        //对所有背包进行 清空+刷新  TODO:后续会做新的游戏 和 继续游戏 的逻辑
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

    private bool CheckInSpecifiedInventory(ContainerUI specifiedInventory,Vector3 position)
    {
        for(int i=0;i< specifiedInventory.slotHolders.Length;i++)
        {
            RectTransform t = specifiedInventory.slotHolders[i].transform as RectTransform;
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

    private void RefreshAllContainer()
    {
        consumableContainer.RefreshContainerUI();
        equipmentsContainer.RefreshContainerUI();
        actionContainer.RefreshContainerUI();
        playerEquipmentContainer.RefreshContainerUI();
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

    
    public ItemData_SO GetItemByName(string name)
    {
        if (itemNameToItemData.ContainsKey(name))
            return itemNameToItemData[name];
        else
            return null;
    }
    
}
