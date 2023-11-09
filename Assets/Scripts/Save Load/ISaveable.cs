public interface ISaveable
{
    void RegisterSaveData()=>DataManager.Instance.RegisterSaveData(this);

    void UnRegisterSaveData()=>DataManager.Instance.UnRegisterSaveData((this));

    /// <summary>
    /// ��Ҫ������ݷ��͸�DataManager
    /// </summary>
    void GetSaveData();

    /// <summary>
    /// ��DataManager�ж�ȡ����
    /// </summary>
    void LoadData();
}














