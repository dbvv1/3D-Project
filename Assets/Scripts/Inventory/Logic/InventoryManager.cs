using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("��������")]
    public InventoyrData_SO consumableInventory;
    
    public InventoyrData_SO euqipmentsInventory;

    public InventoyrData_SO playerEuqipmentInventory;

    public InventoyrData_SO actionInventory;

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

    #region �л���ͬ�ı�������
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
        titleText.text = "����Ʒ";
    }

    private void SelectEuqipmentsContainer()
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

}
