using System;
using UnityEngine;


//数据的定义
public class DataDefination : MonoBehaviour
{
    [SerializeField]private PersistentType persistentType;

    public string id;

    
    private void OnValidate()
    {
        if (persistentType == PersistentType.ReadWrite)
        {
            if (id == string.Empty)
                id = Guid.NewGuid().ToString();
        }
        else
            id = string.Empty;
    }
}
