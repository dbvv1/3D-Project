using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("引用")]
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
            //设定角色面板和背景面板
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
            //更改之前的面板
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
            //设定新的面板
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
        //按下Tab键 或者 B 键 进入UI Panel界面 (使用lambda表达式)
        inputActions.UI.OpenBagUI.started += ctx =>
        {
            if (CurrentPanel == PanelType.Inventory) CurrentPanel = PanelType.None;
            else CurrentPanel = PanelType.Inventory;
        };
        //按下ESC键 打开界面 或者 关闭所有界面 
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

    //关闭所有的面板
    private void CloseAllPanels()
    {
        backGroundPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        characterStatsPanel.SetActive(false);
        questTaskPanel.SetActive(false);
        skillPanel.SetActive(false);
    }

    //点击总面板的关闭按钮时
    public void OnCloseButtonClicked()
    {
        CurrentPanel = PanelType.None;
    }

}
