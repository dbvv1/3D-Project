using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditorInternal;
using System.IO;

[CustomEditor(typeof(DialogueData_SO))]
public class DialogueCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open in Editor"))
        {
            DialogueEditor.InitWindow(target as DialogueData_SO);
        }

        base.OnInspectorGUI();

    }
}

public class DialogueEditor : EditorWindow
{
    DialogueData_SO currentData;

    ReorderableList piecesList = null;

    Vector2 scrollPos = Vector2.zero;

    Dictionary<string, ReorderableList> optionListDict = new Dictionary<string, ReorderableList>();

    [MenuItem("Dbvv/Dialogue Editor")]

    public static void Init()
    {
        DialogueEditor editorWindow = GetWindow<DialogueEditor>("Dialogue Editor");
        editorWindow.autoRepaintOnSceneChange = true;
    }

    public static void InitWindow(DialogueData_SO data)
    {
        DialogueEditor editorWindow = GetWindow<DialogueEditor>("Dialogue Editor");
        editorWindow.currentData = data;

    }

    [OnOpenAsset]
    public static bool OpenAsset(int instanceID,int line)
    {
        DialogueData_SO data = EditorUtility.InstanceIDToObject(instanceID) as DialogueData_SO;

        if(data!=null)
        {
            DialogueEditor.InitWindow(data);
            return true;
        }
        return false;
    }

    private void OnSelectionChange()
    {
        //选择改变时调用
        var newData=Selection.activeObject as DialogueData_SO;
        if(newData)
        {
            currentData = newData;
            SetupReorderableList();
        }
        else
        {
            currentData = null;
            piecesList = null;
        }
        Repaint();
    }

    private void OnGUI()
    {
        if(currentData!=null)
        {
            EditorGUILayout.LabelField(currentData.name, EditorStyles.boldLabel);
            GUILayout.Space(10);

            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (piecesList == null)
                SetupReorderableList();

            piecesList.DoLayoutList();
            GUILayout.EndScrollView();
        }
        else
        {
            if(GUILayout.Button("Create New Dialogue"))
            {
                string dataPath = "Assets/Game Data/Dialogue Data/";
                if (!Directory.Exists(dataPath))
                    Directory.CreateDirectory(dataPath);

                DialogueData_SO newData = ScriptableObject.CreateInstance<DialogueData_SO>();
                newData.dialoguePieces = new List<DialoguePiece>();
                AssetDatabase.CreateAsset(newData, dataPath+"/"+"New Dialogue.asset");
                currentData = newData;

            }

            GUILayout.Label("NO DATA SELECTED!", EditorStyles.boldLabel);
        }
    }

    private void OnDisable()
    {
        optionListDict.Clear();
    }

    private void SetupReorderableList()
    {
        piecesList = new ReorderableList(currentData.dialoguePieces, typeof(DialoguePiece), true, true, true, true);
       
        piecesList.drawHeaderCallback += OnDrawPieceHeader;

        piecesList.drawElementCallback += OnDrawPieceListElement;

        piecesList.elementHeightCallback += OnHeightChanged;
    }

    private float OnHeightChanged(int index)
    {
        return GetPieceHeight(currentData.dialoguePieces[index]);
    }

    float GetPieceHeight(DialoguePiece piece)
    {
        var height = EditorGUIUtility.singleLineHeight;

        var isExpand = piece.canExpand;

        if(isExpand)
        {
            height += EditorGUIUtility.singleLineHeight * 9;

            var options = piece.options;

            if (options.Count > 1)
            {
                height += EditorGUIUtility.singleLineHeight * options.Count;
            }
        }


        return height;
    }

    private void OnDrawPieceHeader(Rect rect)
    {
        GUI.Label(rect, "Dialogue Pieces");
    }

    private void OnDrawPieceListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorUtility.SetDirty(currentData);

        GUIStyle textStyle = new GUIStyle("TextField");

        if(index<currentData.dialoguePieces.Count)
        {
            DialoguePiece currentPiece = currentData.dialoguePieces[index];

            Rect tempRect = rect;

            tempRect.height = EditorGUIUtility.singleLineHeight;

            currentPiece.canExpand = EditorGUI.Foldout(tempRect, currentPiece.canExpand, currentPiece.ID);

            if(currentPiece.canExpand)
            {
                tempRect.width = 30;
                tempRect.y += tempRect.height;
                EditorGUI.LabelField(tempRect, "ID");

                tempRect.x += tempRect.width;
                tempRect.width = 100;
                currentPiece.ID = EditorGUI.TextField(tempRect, currentPiece.ID);

                tempRect.x += tempRect.width + 10;
                EditorGUI.LabelField(tempRect, "Quest");

                tempRect.x += 40;
                currentPiece.taskData = (TaskData_SO)EditorGUI.ObjectField(tempRect, currentPiece.taskData, typeof(TaskData_SO), false);

                tempRect.y += EditorGUIUtility.singleLineHeight + 5;
                tempRect.x = rect.x;
                tempRect.height = 60;
                tempRect.width = tempRect.height;
                currentPiece.image = (Sprite)EditorGUI.ObjectField(tempRect, currentPiece.image, typeof(Sprite), false);

                //文本框
                tempRect.x += tempRect.width + 5;
                tempRect.width = rect.width - tempRect.x;
                textStyle.wordWrap = true;
                currentPiece.text = (string)EditorGUI.TextField(tempRect, currentPiece.text, textStyle);

                //画选项
                tempRect.y += tempRect.height + 5;
                tempRect.x = rect.x;
                tempRect.width = rect.width;

                string optionListKey = currentPiece.ID + currentPiece.text;

                if (optionListKey != string.Empty) 
                {
                    if (!optionListDict.ContainsKey(optionListKey))
                    {
                        var optionList = new ReorderableList(currentPiece.options, typeof(DialogueOption), true, true, true, true);

                        optionList.drawHeaderCallback += OnDrawOptionHeader;

                        optionList.drawElementCallback += (optionRect, optionIndex, optionActive, optionFocused) =>
                        {
                            OnDrawOptionElement(currentPiece, optionRect, optionIndex, optionActive, optionFocused);
                        };

                        optionListDict[optionListKey] = optionList;
                    }

                    optionListDict[optionListKey].DoList(tempRect);

                }
            }

        }
    }

    private void OnDrawOptionHeader(Rect rect)
    {
        GUI.Label(rect, "Option Text");
        rect.x += rect.width * 0.5f + 10;
        GUI.Label(rect, "Target ID");
        rect.x += rect.width * 0.3f;
        GUI.Label(rect, "Apply");
    }

    private void OnDrawOptionElement(DialoguePiece currentPiece, Rect optionRect, int optionIndex, bool optionActive, bool optionFocused)
    {
        var currentOption = currentPiece.options[optionIndex];
        var tempRect = optionRect;

        tempRect.width = optionRect.width * 0.5f;
        currentOption.text = EditorGUI.TextField(tempRect, currentOption.text);

        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.3f;
        currentOption.targetID = EditorGUI.TextField(tempRect, currentOption.targetID);

        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.2f;
        currentOption.takeTask = EditorGUI.Toggle(tempRect, currentOption.takeTask);

    }
}
