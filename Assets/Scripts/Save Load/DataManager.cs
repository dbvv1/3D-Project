using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private HashSet<ISaveable> saveableSet = new HashSet<ISaveable>(); 
    
    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableSet.Contains(saveable))
            saveableSet.Add(saveable);
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        if (saveableSet.Contains(saveable))
            saveableSet.Remove(saveable);
    }
    
    
}

























