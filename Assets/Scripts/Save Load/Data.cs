using System.Collections.Generic;
using UnityEngine;

//记录所有需要存储的数据
public class Data
{
    private string curScene;
    
    //所有角色的状态信息
    public Dictionary<string, CharacterData_SO> characterStatsData = new();

    #region 背包信息

    public InventoryData_SO consumableInventory = ScriptableObject.CreateInstance<InventoryData_SO>();

    public InventoryData_SO equipmentsInventory = ScriptableObject.CreateInstance<InventoryData_SO>();

    public InventoryData_SO playerEquipmentInventory=ScriptableObject.CreateInstance<InventoryData_SO>();

    public InventoryData_SO actionInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
    
    #endregion

    public List<TaskData_SO> tasks;
    
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

    public void SaveScene()
    {
        
    }

    public void GetSavedScene()
    {
        
    }
    
    
    
}
