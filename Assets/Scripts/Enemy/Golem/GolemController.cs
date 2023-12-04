using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : EnemyController
{
    [Header("Զ����ʯ����")]
    [SerializeField] private GolemRock rockPrefab;
    [SerializeField] private Transform rockSpawnPoint;
    
    private GolemRock currentRock;

    private const string StaticName = "Golem";
    private const EnemyLevelType StaticType = EnemyLevelType.Elite;

    public override string EnemyName => StaticName;
    public override EnemyLevelType EnemyLevel => StaticType;

    public override void InitAfterGenerate()
    {
        base.InitAfterGenerate();
    }

    public override EnemyFactory CreateFactory(EnemyController enemyPrefab)
    {
        return new GolemFactory(enemyPrefab as GolemController);
    }

    public override void AttackNearF()
    {
        base.AttackNearF();
    }

    public override void AttackFarF()
    {
        base.AttackFarF();
    }

    public override void OnHurt()
    {
        base.OnHurt();
        //ɾ��rockSpawnPoint�¿��ܴ��ڵ�û���ӳ�ȥ����ʯ
        foreach (Transform rock in rockSpawnPoint)
        {
            Destroy(rock.gameObject);
        }
    }

    //�����¼�
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
