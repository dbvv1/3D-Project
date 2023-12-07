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
    private readonly HashSet<ISavable> savableSet = new HashSet<ISavable>();

    private Data saveData;

    private string jsonFolder;
    
    private readonly JsonSerializerSettings settings = new JsonSerializerSettings();

    protected override void Awake()
    {
        base.Awake();
        saveData = new Data();
        settings.TypeNameHandling = TypeNameHandling.Auto;
        jsonFolder = Application.persistentDataPath + "/Save Data/";
        ReadSaveData();
    }

    private void OnEnable()
    {
        GlobalEvent.continueGameEvent += Load;
    }

    private void OnDisable()
    {
        GlobalEvent.continueGameEvent -= Load;
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
        if (!savableSet.Contains(savable))
            savableSet.Add(savable);
    }

    public void UnRegisterSaveData(ISavable savable)
    {
        if (savableSet.Contains(savable))
            savableSet.Remove(savable);
    }

    
    //��save�б��е�����ȫ�����浽�ļ���
    public void Save()
    {
        foreach (var savable in savableSet)
        {
            savable.SaveData(saveData);
        }

        string resultPath = jsonFolder + "saveData.sav";


        string jsonData = JsonConvert.SerializeObject(saveData, settings);

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        
        File.WriteAllText(resultPath,jsonData);

    }
    
    //����save data�е����ݵ�����Ϸ����
    public void Load()
    {
        foreach (var savable in savableSet)
        {
            savable.LoadData(saveData);
        }
    }
    
    //���ļ��е����ݷ��õ�sava data��
    private void ReadSaveData()
    {
        string resultPath = jsonFolder + "saveData.sav";
        if (File.Exists(resultPath))
        {
            string stringData = File.ReadAllText(resultPath);
            saveData = JsonConvert.DeserializeObject<Data>(stringData,settings);
        }
    }
    
}

























