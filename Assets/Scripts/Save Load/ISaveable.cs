public interface ISaveable
{
    void RegisterSaveData()=>DataManager.Instance.RegisterSaveData(this);

    void UnRegisterSaveData()=>DataManager.Instance.UnRegisterSaveData((this));

    /// <summary>
    /// 将要存的数据发送给DataManager
    /// </summary>
    void GetSaveData();

    /// <summary>
    /// 从DataManager中读取数据
    /// </summary>
    void LoadData();
}














