using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class DialogueOption
{
    
    public string text;

    public string targetID;
    
    public bool takeTask;
}