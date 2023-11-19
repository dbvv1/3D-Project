using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue",menuName= "Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces;

    public Dictionary<string, int> stringToDialoguePiece = new();

    #if UNITY_EDITOR
    private void OnValidate()
    {
        stringToDialoguePiece.Clear();
        for (int i = 0; i < dialoguePieces.Count; i++)
        {
            stringToDialoguePiece.Add(dialoguePieces[i].ID, i);
        }

    }
    #endif
}
