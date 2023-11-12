
public interface ISavable
{
    string GetDataID();
    
    public void RegisterSaveData()=>DataManager.Instance.RegisterSaveData(this);

    public void UnRegisterSaveData()=>DataManager.Instance.UnRegisterSaveData((this));

    /// <summary>
    /// 将数据保存到data当中
    /// </summary>
    void SaveData(Data data);

    /// <summary>
    /// 从Data中读取数据
    /// </summary>
    void LoadData(Data data);
}














