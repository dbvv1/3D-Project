using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>,ISavable
{
    [Header("��������")]
    public InventoryData_SO consumableInventory;
    
    public InventoryData_SO equipmentsInventory;

    public InventoryData_SO playerEquipmentInventory;

    public InventoryData_SO actionInventory;

    [Header("��������")]
    public ContainerUI consumableContainer;        //����Ʒ����

    public ContainerUI equipmentsContainer;        //װ������

    public ContainerUI actionContainer;            //������ϵı���

    public ContainerUI playerEquipmentContainer;   //��������װ����װ������

    private ContainerUI currentShowContainer;      //��ǰѡ��ı��� (ָ��ɫ����еı���ѡ��,�������������ڴ���װ��)

    [Header("����")]
    public TextMeshProUGUI titleText;

    public Tooltip itemTooltip;

    [Header("��ק���")]
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
        //�����б������� ���+ˢ��  TODO:���������µ���Ϸ �� ������Ϸ ���߼�
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

    #region �л���ͬ�ı�������
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
        titleText.text = "����Ʒ";
    }

    private void SelectEquipmentsContainer()
    {
        consumableContainer.gameObject.SetActive(false);
        equipmentsContainer.gameObject.SetActive(true);
        titleText.text = "װ��";
    }
    #endregion

    #region ��鵱ǰ���λ���Ƿ�λ��һ��SlotHoler֮�� ������ʵ����ק�ͽ�����
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

    #region Save ��ؽӿ�
    public string GetDataID()
    {
        return "Inventory";
    }

    public void SaveData(Data data)
    {
        //Save���еı���
        data.consumableInventory.SaveInventory(consumableInventory);
        data.equipmentsInventory.SaveInventory(equipmentsInventory);
        data.playerEquipmentInventory.SaveInventory(playerEquipmentInventory);
        data.actionInventory.SaveInventory(actionInventory);
    }

    public void LoadData(Data data)
    {
        //Load���еı���
        consumableInventory.LoadInventory(data.consumableInventory);
        equipmentsInventory.LoadInventory(data.equipmentsInventory);
        playerEquipmentInventory.LoadInventory(data.playerEquipmentInventory);
        actionInventory.LoadInventory(data.actionInventory);
        //ˢ�����еı���
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
