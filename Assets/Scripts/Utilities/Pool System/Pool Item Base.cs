using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolItemBase : MonoBehaviour, IPool
{
    //对象的使用者
    protected Transform user;

    [SerializeField] protected float maxDelayTime;   //定义了对象的最长激活时间

    //对象在对象池中的键值 静态变量
    protected static string objectName;

    //设置对象种类在池中的名称
    private void Awake()
    {
        SettingObjectName();
    }

    protected abstract void SettingObjectName();

    #region 接口

    //物品的初始化操作(不指定使用者)
    public virtual void SettingObject()
    {
        
    }

    //物品的初始化操作（附带使用者，并且规定了最大的存在时间，可以进行自动管理）
    public virtual void SettingObject(Transform user)
    {
        this.user = user;
        GOPoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(maxDelayTime,() => RecycleObject(), false);
    }

    //回收物品
    public virtual void RecycleObject()
    {
        this.user = null;
        GOPoolManager.Instance.RecycleGameObject(gameObject, objectName);

    }

    #endregion


    public Transform GetUser() => user;
}
