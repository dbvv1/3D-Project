using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : EnemyController
{
    [Header("远程岩石攻击")]
    [SerializeField] private GolemRock rockPrefab;
    [SerializeField] private Transform rockSpawnPoint;
    
    private GolemRock currentRock;
    
    protected override void SettingEnemyName()
    {
        enemyTypeName = "Golem";
    }

    public override void AttackNearF()
    {
        base.AttackNearF();
    }

    public override void AttackFarF()
    {
        base.AttackFarF();
    }

    //动画事件
    public void CreateRock()
    {
        currentRock = Instantiate(rockPrefab, rockSpawnPoint);
        currentRock.InitAfterCreate(this);
    }

    public void ThrowRock()
    {
        if (currentRock != null && currentRock.gameObject.activeSelf)
        {
            currentRock.InitAfterFly(this);
        }
    }
    
}
