using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("背包数据")]
    public InventoyrData_SO consumableInventory;
    
    public InventoyrData_SO euqipmentsInventory;

    public InventoyrData_SO playerEuqipmentInventory;

    public InventoyrData_SO actionInventory;

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

    public ContainerUI CurrentShowContainer { 
        get => currentShowContainer;
        set
        {
            currentShowContainer = value;
            if (value == consumableContainer) SelectConsumableContainer();
            else if (value == equipmentsContainer) SelectEuqipmentsContainer();
        } 
    }

    private void Start()
    {
        CurrentShowContainer = consumableContainer;
        CurrentShowContainer.RefreshContainerUI();
        equipmentsContainer.RefreshContainerUI();
        actionContainer.RefreshContainerUI();
        playerEquipmentContainer.RefreshContainerUI();
    }

    #region 切换不同的背包界面
    public void OnButtonClickConsumableContainer()
    {
        SelectConsumableContainer();
    }

    public void OnButtonClickEquipmentsContainer()
    {
        SelectEuqipmentsContainer();
    }

    private void SelectConsumableContainer()
    {
        consumableContainer.gameObject.SetActive(true);
        equipmentsContainer.gameObject.SetActive(false);
        titleText.text = "消耗品";
    }

    private void SelectEuqipmentsContainer()
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

}
