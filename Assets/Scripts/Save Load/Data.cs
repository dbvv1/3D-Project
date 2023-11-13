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
    

    public void SaveScene()
    {
        
    }

    public void GetSavedScene()
    {
        
    }
    
    
    
}
