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
        
        //������� ����Ļ���� ��������Ϊ���ɼ�
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
        //���ص���Ϸ����: ���ع�� + �������������λ��(��Ҫ���Ǵ��ڶԻ�״̬)
        if (panelType == PanelType.None)
        {
            if (!DialogueUIManager.Instance.IsTalking)
            {
                Cursor.SetCursor(thirdPersonCombatMouseTexture, new Vector2(thirdPersonCombatMouseTexture.width / 2, thirdPersonCombatMouseTexture.height / 2), CursorMode.Auto);

                Cursor.lockState = CursorLockMode.Locked;

                Cursor.visible = false;
            }
        }
        //�����ɫ�����棺 ���ù��ͼ�� + ��ʾ���
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
        //������� ����Ļ���� ��������Ϊ���ɼ�
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
