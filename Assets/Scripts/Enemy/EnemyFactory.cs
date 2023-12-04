using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public interface ICreateFactory
{
    public EnemyFactory CreateFactory(EnemyController enemyPrefab);
}

public abstract class EnemyFactory
{
    public abstract EnemyController CreateEnemy();

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
        metalon.InitAfterGenerate();
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
        golem.InitAfterGenerate();
        return golem;
    }
}