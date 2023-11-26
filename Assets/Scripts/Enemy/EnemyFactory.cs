using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public abstract class EnemyFactory
{
    
    public abstract EnemyController CreateEnemy();

     ~EnemyFactory() { }
}

public class MetalonFactory : EnemyFactory
{
    private MetalonController metalonPrefab;

    public MetalonFactory(MetalonController metalonPrefab) 
    {
        this.metalonPrefab = metalonPrefab;
    }
    public override EnemyController CreateEnemy()
    {
        MetalonController metalon = Object.Instantiate(metalonPrefab);
        metalon.transform.position = Vector3.zero; //后续转变为一个随机的位置
        metalon.SettingMaterials();
        return metalon;
    }
}

public class ChomperFactory : EnemyFactory
{
    private ChomperController chomperPrefab;
    
    public ChomperFactory(ChomperController chomperPrefab)
    {
        this.chomperPrefab = chomperPrefab;
    }

    public override EnemyController CreateEnemy()
    {
        ChomperController chomper = Object.Instantiate(chomperPrefab);
        chomper.transform.position = Vector3.zero; //后续转变为一个随机的位置
        return chomper;
    }
}

public class GolemFactory : EnemyFactory
{
    private GolemController golemPrefab;
    
    public GolemFactory(GolemController golemPrefab)
    {
        this.golemPrefab = golemPrefab;
    }

    public override EnemyController CreateEnemy()
    {
        GolemController golem = Object.Instantiate(golemPrefab);
        golem.transform.position = Vector3.zero; //后续转变为一个随机的位置
        return golem;
    }
}