using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : Singleton<UIManager>
{
    [Header("Panel引用")]
    [SerializeField]private GameObject backGroundPanel;

    [SerializeField]private GameObject characterStatsPanel;

    [SerializeField]private GameObject inventoryPanel;

    [SerializeField]private GameObject questTaskPanel;

    [SerializeField]private GameObject skillPanel;
    
    [FormerlySerializedAs("EnemyHealthBarCanvas")]
    [Header("其他引用")]
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
            //设定新的面板
            switch (currentPanel)
            {
                case PanelType.None:
                    CloseAllPanels();
                    break;
                case PanelType.Inventory:
                    panelTitleText.text = "物品和角色";
                    inventoryPanel.SetActive(true);
                    characterStatsPanel.SetActive(true);
                    break;
                case PanelType.Skill:
                    panelTitleText.text = "技能面板";
                    skillPanel.SetActive(true);
                    break;
                case PanelType.QuestTask:
                    TaskUIManager.Instance.OpenTaskPanel();
                    panelTitleText.text = "任务面板";
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
        inputActions.UI.OpenBagUI.started += _ =>
        {
            if (CurrentPanel !=PanelType.None) CurrentPanel = PanelType.None;
            else CurrentPanel = PanelType.Inventory;
        };
        //按下ESC键 打开界面 或者 关闭所有界面 
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

    //关闭所有的面板
    private void CloseAllPanels()
    {
        backGroundPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        characterStatsPanel.SetActive(false);
        questTaskPanel.SetActive(false);
        skillPanel.SetActive(false);
    }
    
    //点击启用下一个面板
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
    
    //点击启用上一个面板
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
    

    //点击总面板的关闭按钮时
    public void OnCloseButtonClicked()
    {
        CurrentPanel = PanelType.None;
    }
    
}
