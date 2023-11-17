using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("����")]
    public GameObject backGroundPanel;

    public GameObject characterStatsPanel;

    public GameObject inventoryPanel;

    public GameObject questTaskPanel;

    public GameObject skillPanel;

    public Canvas EnemyHealthBarCanvas;

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
                    inventoryPanel.SetActive(true);
                    break;
                case PanelType.Skill:
                    skillPanel.SetActive(true);
                    break;
                case PanelType.QuestTask:
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
        inputActions.UI.OpenBagUI.started += ctx =>
        {
            if (CurrentPanel == PanelType.Inventory) CurrentPanel = PanelType.None;
            else CurrentPanel = PanelType.Inventory;
        };
        //����ESC�� �򿪽��� ���� �ر����н��� 
        inputActions.UI.CloseAllUI.started += ctx =>
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

    //��������Ĺرհ�ťʱ
    public void OnCloseButtonClicked()
    {
        CurrentPanel = PanelType.None;
    }

}
