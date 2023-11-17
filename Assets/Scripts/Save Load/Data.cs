using System.Collections.Generic;
using UnityEngine;

//��¼������Ҫ�洢������
public class Data
{
    private string curScene;
    
    //���н�ɫ��״̬��Ϣ
    public Dictionary<string, CharacterData_SO> characterStatsData = new();

    #region ������Ϣ

    public InventoryData_SO consumableInventory ;

    public InventoryData_SO equipmentsInventory ;

    public InventoryData_SO playerEquipmentInventory;

    public InventoryData_SO actionInventory;
    

    #endregion

    public void SaveAllInventory(InventoryData_SO i1, InventoryData_SO i2, InventoryData_SO i3, InventoryData_SO i4)
    {
        consumableInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
        consumableInventory.SaveInventory(i1);
        equipmentsInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
        equipmentsInventory.SaveInventory(i2);
        playerEquipmentInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
        playerEquipmentInventory.SaveInventory(i3);
        actionInventory = ScriptableObject.CreateInstance<InventoryData_SO>();
        actionInventory.SaveInventory(i4);
    }

    public void SaveScene()
    {
        
    }

    public void GetSavedScene()
    {
        
    }
    
    
    
}
