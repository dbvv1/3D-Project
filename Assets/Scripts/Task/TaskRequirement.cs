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
    /// <param name="name">需求目标的名字</param>
    /// <param name="needAmount">需求的数量</param>
    /// <param name="currentAmount">现有的数量</param>
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
            progressNumberText.text = "已完成";
            requireNameText.color = Color.gray;
            progressNumberText.color = Color.gray;
        }
    }

    
}
