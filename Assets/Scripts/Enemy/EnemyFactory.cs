using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public abstract class EnemyFactory
{
    
    public abstract EnemyController CreateEnemy();

     ~EnemyFactory() { }
}

public class MetalonFactory : EnemyFactory
{
    private readonly MetalonController metalonPrefab;

    public MetalonFactory(MetalonController metalonPrefab) 
    {
        this.metalonPrefab = metalonPrefab;
    }
    public override EnemyController CreateEnemy()
    {
        var metalon = Object.Instantiate(metalonPrefab);
        metalon.transform.position = Vector3.zero; //后续转变为一个随机的位置
        metalon.SettingMaterials();
        return metalon;
    }
}


public class GolemFactory : EnemyFactory
{
    private readonly GolemController golemPrefab;
    
    public GolemFactory(GolemController golemPrefab)
    {
        this.golemPrefab = golemPrefab;
    }

    public override EnemyController CreateEnemy()
    {
        var golem = Object.Instantiate(golemPrefab);
        golem.transform.position = Vector3.zero; //后续转变为一个随机的位置
        return golem;
    }
}