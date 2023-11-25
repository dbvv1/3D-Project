using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskRequirement : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI requireNameText;

    [SerializeField]private TextMeshProUGUI progressNumberText;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">����Ŀ�������</param>
    /// <param name="needAmount">���������</param>
    /// <param name="currentAmount">���е�����</param>
    public void SetupRequirement(string name, int needAmount, int currentAmount)
    {
        requireNameText.text = name;
        progressNumberText.text = currentAmount + "/" + needAmount;
    }

    public void SetupRequirement(string name, bool isFinished)
    {
        if (isFinished)
        {
            requireNameText.text = name;
            progressNumberText.text = "�����";
            requireNameText.color = Color.gray;
            progressNumberText.color = Color.gray;
        }
    }

    
}
