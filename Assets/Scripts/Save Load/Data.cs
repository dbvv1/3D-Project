using System.Collections.Generic;
using UnityEngine;

//��¼������Ҫ�洢������
public class Data
{
    private string curScene;
    
    //���н�ɫ��״̬��Ϣ
    public Dictionary<string, CharacterData_SO> characterStatsData = new();

    #region ������Ϣ

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
