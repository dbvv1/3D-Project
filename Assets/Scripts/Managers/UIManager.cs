using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PanelInfo
{
    public GameObject panel;

    public PanelType panelType;

    public string panelName;

    public PanelInfo(GameObject panel,PanelType panelType,string panelName)
    {
        this.panel = panel;
        this.panelType = panelType;
        this.panelName = panelName;
    }
    
}

public class UIManager : Singleton<UIManager>
{
    [Header("Panel引用")] 
    [SerializeField] private GameObject menuImage;
    
    [SerializeField] private GameObject backGroundImage;

    [SerializeField] private GameObject characterStatsPanel;

    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private GameObject questTaskPanel;

    [SerializeField] private GameObject skillPanel;

    private readonly List<PanelInfo> panels = new();

    [Header("其他引用")] 
    [SerializeField] public Canvas enemyStatsBarCanvas;

    [SerializeField] private TextMeshProUGUI panelTitleText;
    [SerializeField] private TextMeshProUGUI leftPanelInfoText;
    [SerializeField] private TextMeshProUGUI rightPanelInfoText;

    [Header("主菜单场景")] 
    [SerializeField] private GameSceneSO menuScene;

    public FadeCanvas fadeCanvas;

    private UIInputController inputActions;

    private PanelType currentPanel;

    private int currentPanelIndex = 0;

    private PanelType CurrentPanel
    {
        get => currentPanel;
        set
        {
            CloseAllPanels();
            //设定角色面板和背景面板
            characterStatsPanel.SetActive(value != PanelType.None && value != PanelType.QuestTask);
            backGroundImage.SetActive(value != PanelType.None);
            MouseManager.Instance.SetMouseCursorByPanelType(value);
            if (value != PanelType.None)
            {
                GlobalEvent.CallStopTheWorldEvent();
            }
            else
            {
                GlobalEvent.CallContinueTheWorldEvent();
                InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
            }

            currentPanel = value;
            // 设置面板的标题信息
            SetLeftAndRightPanelInfo();
            
            // 设置快捷栏物品的显示
            InventoryManager.Instance.SetActionContainerParent(currentPanel);

            if (currentPanel != PanelType.None) panels[currentPanelIndex].panel.SetActive(true);
            if(currentPanel==PanelType.QuestTask) TaskUIManager.Instance.OpenTaskPanel();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        menuImage.SetActive(true);
        inputActions = new UIInputController();
        // 初始化需要所有的面板种类
        panels.Add(new PanelInfo(inventoryPanel, PanelType.Inventory, "物品和角色"));
        panels.Add(new PanelInfo(questTaskPanel, PanelType.QuestTask, "任务"));
        panels.Add(new PanelInfo(skillPanel, PanelType.Skill, "技能和角色"));
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
            if (CurrentPanel != PanelType.None) CurrentPanel = PanelType.None;
            else
            {
                currentPanelIndex = 0;
                CurrentPanel = PanelType.Inventory;
            }
        };
        //按下ESC键 打开界面 或者 关闭所有界面 
        inputActions.UI.CloseAllUI.started += _ =>
        {
            if (CurrentPanel != PanelType.None)
                CurrentPanel = PanelType.None;
            else
            {
                currentPanelIndex = 0;
                CurrentPanel = PanelType.Inventory;
            }
        };
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    
    // 回到主菜单面板
    public void ReturnToMainMenu()
    {
        DataManager.Instance.Save();
        Time.timeScale = 1;
        SceneLoader.Instance.SceneTransition(menuScene, Vector3.zero, true);
    }

    public void SetPanelAfterLoad(SceneType sceneType)
    {
        menuImage.SetActive(sceneType == SceneType.Persistent | sceneType == SceneType.Menu);
        if (sceneType == SceneType.RestScene | sceneType == SceneType.FightScene) CloseAllPanels();
    }

    
    // 关闭所有的面板
    private void CloseAllPanels()
    {
        backGroundImage.SetActive(false);
        inventoryPanel.SetActive(false);
        characterStatsPanel.SetActive(false);
        questTaskPanel.SetActive(false);
        skillPanel.SetActive(false);
    }

    // 得到下一个面板的信息
    private int GetNextPanelIndex()
    {
        return (currentPanelIndex + 1) % panels.Count;
    }

    // 得到上一个面板的信息
    private int GetPrePanelIndex()
    {
        return (currentPanelIndex - 1 + panels.Count) % panels.Count;
    }
    
    // 设置左右面板的信息
    private void SetLeftAndRightPanelInfo()
    {
        panelTitleText.text = panels[currentPanelIndex].panelName;
        leftPanelInfoText.text = panels[GetPrePanelIndex()].panelName;
        rightPanelInfoText.text = panels[GetNextPanelIndex()].panelName;
    }
    

    #region Button的监听事件

    //点击启用下一个面板
    public void OnClickNextPanelButton()
    {
        currentPanelIndex = GetNextPanelIndex();
        CurrentPanel = panels[currentPanelIndex].panelType;
    }

    //点击启用上一个面板
    public void OnClickPreviousPanelButton()
    {
        currentPanelIndex = GetPrePanelIndex();
        CurrentPanel = panels[currentPanelIndex].panelType;
    }


    //点击总面板的关闭按钮时
    public void OnCloseButtonClicked()
    {
        CurrentPanel = PanelType.None;
        currentPanelIndex = 0;
    }

    #endregion
}