using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece 
{
    public string ID;

    public Sprite image;

    [TextArea]
    public string text;

    public List<DialogueOption> options = new List<DialogueOption>();

    public TaskData_SO taskData;

    [HideInInspector]public bool canExpand;
}