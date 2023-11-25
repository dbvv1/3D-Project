using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatSkillBase_SO : ScriptableObject
{
    [SerializeField] private string skillName;
    [SerializeField] private int skillID;
    [SerializeField] private bool isSkillDone;
    [SerializeField] private float skillCDTime;
    [SerializeField] private float skillDistance;

    
    //���ⲿ�����������
    public abstract void InvokeSkill();
    
    //�ڲ�ʹ���������
    private void UseSkill()
    {
        //ʵ��ʹ�ü���
        //���ż��ܵĶ��� �ȵ�

        isSkillDone = false;

        //������CD��ʹ�ü�ʱ��Timer
        GOPoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime
            (skillCDTime, () => isSkillDone = true);

    }


    #region ������

    //����������
    public void InitSkill()
    {

    }

    public string GetSkillName() => skillName;
    public int GetSkillID() => skillID;
    public void SetIsSkillDone(bool isDone) => isSkillDone = isDone;
    public bool GetIsSkillDone() => isSkillDone;
    //public float GetSkillCDTime() => skillCDTime;
    //public float GetSkillDistance() => skillDistance;


    #endregion



}
