using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    public Texture2D thirdPersonCombatMouseTexture;

    public Texture2D characterPanelMouseTexture;

    private void OnEnable()
    {
        GlobalEvent.onEnterDialogue += OnEnterDialogue;
        GlobalEvent.onExitDialogue += OnExitDialogue;
        
        //锁定光标 到屏幕中心 并且设置为不可见
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

    }

    private void OnDisable()
    {
        GlobalEvent.onEnterDialogue -= OnEnterDialogue;
        GlobalEvent.onExitDialogue -= OnExitDialogue;
    }

    public void SetMouseCursor(PanelType panelType)
    {
        //返回到游戏界面: 隐藏光标 + 控制鼠标在中心位置(需要不是处在对话状态)
        if (panelType == PanelType.None)
        {
            if (!DialogueUIManager.Instance.IsTalking)
            {
                Cursor.SetCursor(thirdPersonCombatMouseTexture, new Vector2(thirdPersonCombatMouseTexture.width / 2, thirdPersonCombatMouseTexture.height / 2), CursorMode.Auto);

                Cursor.lockState = CursorLockMode.Locked;

                Cursor.visible = false;
            }
        }
        //进入角色面板界面： 设置光标图标 + 显示光标
        else
        {
            Cursor.SetCursor(characterPanelMouseTexture, Vector2.zero, CursorMode.Auto);

            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

        }

    }

    public void ShowMouseCursor()
    {
        Cursor.visible = true;
    }

    public void HideMouseCursor()
    {
        Cursor.visible = false;
    }

    private void OnEnterDialogue()
    {
        Cursor.SetCursor(characterPanelMouseTexture, Vector2.zero, CursorMode.Auto);

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }
    
    private void OnExitDialogue()
    {
        //锁定光标 到屏幕中心 并且设置为不可见
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
