using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)] 
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance { get => instance; }

    protected virtual void Awake()
    {
        if (instance == null) instance = (T)this;
        else Destroy(gameObject);
        //可以选择添加   DontDestroyOnLoad(gameObject);
    }

    public static bool IsInitialized()
    {
        return instance != null;
    }

}
