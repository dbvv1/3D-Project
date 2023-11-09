using System;
using System.Collections;
using UnityEngine;

public class Timer : PoolItemBase
{
    private bool timeIsDone;

    protected override void SettingObjectName()
    {
        objectName = "Timer";
    }


    /// <summary>
    /// ������ʱ��
    /// </summary>
    /// <param name="timer">��ʱʱ��</param>
    /// <param name="callBackAction">�ص�����</param>
    public void CreateTime(float timer, Action callBackAction, bool timeIsDone = false)
    {
        this.timeIsDone = timeIsDone;
        if (timeIsDone) ExecutiveAction(callBackAction);
        else StartCoroutine(TimerCoroutine(timer, callBackAction));
    }

    IEnumerator TimerCoroutine(float timer, Action callBackAction)
    {
        yield return new WaitForSeconds(timer);
        ExecutiveAction(callBackAction);
    }

    private void ExecutiveAction(Action callBackAction)
    {
        callBackAction?.Invoke();
        RecycleObject();
    }

    public override void RecycleObject()
    {
        StopAllCoroutines();
        base.RecycleObject();
    }


}
