using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private HashSet<ISaveable> saveableSet = new HashSet<ISaveable>();

    private Data data;

    protected override void Awake()
    {
        base.Awake();
        data = new Data();
    }

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

    public void Save()
    {
        foreach (var saveable in saveableSet)
        {
            saveable.SaveData(data);
        }
    }

    public void Load()
    {
        foreach (var saveable in saveableSet)
        {
            saveable.LoadData(data);
        }
    }
    
    
}

























