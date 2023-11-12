
public interface ISavable
{
    string GetDataID();
    
    public void RegisterSaveData()=>DataManager.Instance.RegisterSaveData(this);

    public void UnRegisterSaveData()=>DataManager.Instance.UnRegisterSaveData((this));

    /// <summary>
    /// �����ݱ��浽data����
    /// </summary>
    void SaveData(Data data);

    /// <summary>
    /// ��Data�ж�ȡ����
    /// </summary>
    void LoadData(Data data);
}














