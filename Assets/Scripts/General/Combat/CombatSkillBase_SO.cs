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

    
    //从外部调用这个技能
    public abstract void InvokeSkill();
    
    //内部使用这个技能
    private void UseSkill()
    {
        //实际使用技能
        //播放技能的动画 等等

        isSkillDone = false;

        //处理技能CD：使用计时器Timer
        GOPoolManager.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime
            (skillCDTime, () => isSkillDone = true);

    }


    #region 外界调用

    //传进来引用
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
