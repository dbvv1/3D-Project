public interface ISaveable
{
    void GetDataID();
    
    void RegisterSaveData()=>DataManager.Instance.RegisterSaveData(this);

    void UnRegisterSaveData()=>DataManager.Instance.UnRegisterSaveData((this));

    /// <summary>
    /// �����ݱ��浽data����
    /// </summary>
    void SaveData(Data data);

    /// <summary>
    /// ��Data�ж�ȡ����
    /// </summary>
    void LoadData(Data data);
}














