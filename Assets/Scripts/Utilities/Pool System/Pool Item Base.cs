using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolItemBase : MonoBehaviour, IPool
{
    //�����ʹ����
    protected Transform user;

    [SerializeField] protected float maxDelayTime;   //�����˶���������ʱ��

    //�����ڶ�����еļ�ֵ ��̬����
    protected static string objectName;

    //���ö��������ڳ��е�����
    private void Awake()
    {
        SettingObjectName();
    }

    protected abstract void SettingObjectName();

    #region �ӿ�

    //��Ʒ�ĳ�ʼ������(��ָ��ʹ����)
    public virtual void SettingObject()
    {
        
    }

    //��Ʒ�ĳ�ʼ������������ʹ���ߣ����ҹ涨�����Ĵ���ʱ�䣬���Խ����Զ�����
    public virtual void SettingObject(Transform user)
    {
        this.user = user;
        GOPoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(maxDelayTime,() => RecycleObject(), false);
    }

    //������Ʒ
    public virtual void RecycleObject()
    {
        this.user = null;
        GOPoolManager.Instance.RecycleGameObject(gameObject, objectName);

    }

    #endregion


    public Transform GetUser() => user;
}
