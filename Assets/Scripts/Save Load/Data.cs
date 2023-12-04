using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

//��¼������Ҫ�洢������
public class Data
{
    public string sceneToSave;

    public SerializeVector3 playerPos;
    
    //���н�ɫ��״̬��Ϣ
    public Dictionary<string, CharacterData_SO> characterStatsData = new();
    
    public List<TaskData_SO> tasks = new ();

    public Dictionary<string, TaskData_SO> taskDict = new();

    #region ������Ϣ

    public InventoryData_SO consumableInventory = ScriptableObject.CreateInstance<InventoryData_SO>();

    public InventoryData_SO equipmentsInventory = ScriptableObject.CreateInstance<InventoryData_SO>();

    public InventoryData_SO playerEquipmentInventory=ScriptableObject.CreateInstance<InventoryData_SO>();

    public InventoryData_SO actionInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
    
    #endregion
    
    
    /*public void SaveAllInventory(InventoryData_SO i1, InventoryData_SO i2, InventoryData_SO i3, InventoryData_SO i4)
    {
        consumableInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
        consumableInventory.SaveInventory(i1);
        equipmentsInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
        equipmentsInventory.SaveInventory(i2);
        playerEquipmentInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
        playerEquipmentInventory.SaveInventory(i3);
        actionInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
        actionInventory.SaveInventory(i4);
    }*/

    public void SaveScene(GameSceneSO gameScene)
    {
        sceneToSave = JsonUtility.ToJson(gameScene);
    }

    public GameSceneSO LoadScene()
    {
         var gameScene = ScriptableObject.CreateInstance<GameSceneSO>();
         JsonUtility.FromJsonOverwrite(sceneToSave, gameScene);
         return gameScene;
    }
    
    
    
}
