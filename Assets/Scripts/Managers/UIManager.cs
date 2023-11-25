using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : Singleton<UIManager>
{
    [Header("Panel����")]
    [SerializeField]private GameObject backGroundPanel;

    [SerializeField]private GameObject characterStatsPanel;

    [SerializeField]private GameObject inventoryPanel;

    [SerializeField]private GameObject questTaskPanel;

    [SerializeField]private GameObject skillPanel;
    
    [FormerlySerializedAs("EnemyHealthBarCanvas")]
    [Header("��������")]
    [SerializeField]public Canvas enemyHealthBarCanvas;

    [SerializeField] private TextMeshProUGUI panelTitleText;
    [SerializeField] private TextMeshProUGUI leftPanelInfoText;
    [SerializeField] private TextMeshProUGUI rightPanelInfoText;
    
    public FadeCanvas fadeCanvas;

    private UIInputController inputActions;

    private PanelType currentPanel;

    private PanelType CurrentPanel 
    { 
        get => currentPanel;
        set
        {
            //�趨��ɫ���ͱ������
            characterStatsPanel.SetActive(value != PanelType.None);
            backGroundPanel.SetActive(value != PanelType.None);
            MouseManager.Instance.SetMouseCursor(value);
            if (value != PanelType.None)
            {
                GlobalEvent.CallStopTheWorldEvent();
            }
            else
            {
                GlobalEvent.CallContinueTheWorldEvent();
                InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
            }
            //����֮ǰ�����
            switch (currentPanel)
            {
                case PanelType.None:
                    break;
                case PanelType.Inventory:
                    inventoryPanel.SetActive(false);
                    characterStatsPanel.SetActive(false);
                    break;
                case PanelType.Skill:
                    skillPanel.SetActive(false);
                    break;
                case PanelType.QuestTask:
                    questTaskPanel.SetActive(false);
                    break;
            }
            currentPanel = value;
            //�趨�µ����
            switch (currentPanel)
            {
                case PanelType.None:
                    CloseAllPanels();
                    break;
                case PanelType.Inventory:
                    panelTitleText.text = "��Ʒ�ͽ�ɫ";
                    inventoryPanel.SetActive(true);
                    characterStatsPanel.SetActive(true);
                    break;
                case PanelType.Skill:
                    panelTitleText.text = "�������";
                    skillPanel.SetActive(true);
                    break;
                case PanelType.QuestTask:
                    TaskUIManager.Instance.OpenTaskPanel();
                    panelTitleText.text = "�������";
                    questTaskPanel.SetActive(true);
                    break;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        inputActions = new UIInputController();
    }

    private void Start()
    {
        CurrentPanel = PanelType.None;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        //����Tab�� ���� B �� ����UI Panel���� (ʹ��lambda���ʽ)
        inputActions.UI.OpenBagUI.started += _ =>
        {
            if (CurrentPanel !=PanelType.None) CurrentPanel = PanelType.None;
            else CurrentPanel = PanelType.Inventory;
        };
        //����ESC�� �򿪽��� ���� �ر����н��� 
        inputActions.UI.CloseAllUI.started += _ =>
        {
            if (CurrentPanel != PanelType.None)
                CurrentPanel = PanelType.None;
            else
                CurrentPanel = PanelType.Inventory;
        };
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    //�ر����е����
    private void CloseAllPanels()
    {
        backGroundPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        characterStatsPanel.SetActive(false);
        questTaskPanel.SetActive(false);
        skillPanel.SetActive(false);
    }
    
    //���������һ�����
    public void OnClickNextPanelButton()
    {
        CurrentPanel = CurrentPanel switch
        {
            PanelType.None => PanelType.None,
            PanelType.Inventory => PanelType.QuestTask,
            PanelType.QuestTask => PanelType.Inventory,
            _ => PanelType.None
        };
    }
    
    //���������һ�����
    public void OnClickPreviousPanelButton()
    {
        CurrentPanel = CurrentPanel switch
        {
            PanelType.None => PanelType.None,
            PanelType.Inventory => PanelType.QuestTask,
            PanelType.QuestTask => PanelType.Inventory,
            _ => PanelType.None
        };
    }
    

    //��������Ĺرհ�ťʱ
    public void OnCloseButtonClicked()
    {
        CurrentPanel = PanelType.None;
    }
    
}
