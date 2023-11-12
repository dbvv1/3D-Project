using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using JetBrains.Annotations;

[DefaultExecutionOrder(-100)]
public class DataManager : Singleton<DataManager>
{
    private HashSet<ISavable> saveableSet = new HashSet<ISavable>();

    private Data saveData;

    private string jsonFolder;

    protected override void Awake()
    {
        base.Awake();
        saveData = new Data();

        jsonFolder = Application.persistentDataPath + "/Save Data";
        Debug.Log(jsonFolder);
        ReadSaveData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
        
    }

    public void RegisterSaveData(ISavable savable)
    {
        if (!saveableSet.Contains(savable))
            saveableSet.Add(savable);
    }

    public void UnRegisterSaveData(ISavable savable)
    {
        if (saveableSet.Contains(savable))
            saveableSet.Remove(savable);
    }

    
    //将save列表中的数据全部保存到文件中
    public void Save()
    {
        foreach (var saveable in saveableSet)
        {
            saveable.SaveData(saveData);
        }

        string resultPath = jsonFolder + "saveData.sav";

        string jsonData = JsonConvert.SerializeObject(saveData);

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        
        File.WriteAllText(resultPath,jsonData);

    }
    
    //根据savedata中的数据调整游戏数据
    public void Load()
    {
        foreach (var saveable in saveableSet)
        {
            saveable.LoadData(saveData);
        }
    }
    
    //将文件中的数据放置到savadata中
    private void ReadSaveData()
    {
        string resultPath = jsonFolder + "saveData.sav";
        if (File.Exists(resultPath))
        {
            string stringData = File.ReadAllText(resultPath);
            saveData = JsonConvert.DeserializeObject<Data>(stringData);
        }
    }
    
}


//可被序列化的Vector3
public class SerializeVector3
{
    public float x, y, z;

    public SerializeVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    
}
























