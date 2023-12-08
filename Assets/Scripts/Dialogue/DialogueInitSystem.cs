using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitSystem : MonoBehaviour
{
    [SerializeField] private List<DialogueData_SO> dialogueDatas;

    private void Start()
    {
        foreach (var VARIABLE in dialogueDatas)
        {
            VARIABLE.SetDialogueDictionary();
        }
    }
}
