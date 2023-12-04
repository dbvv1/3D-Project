using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalonController : EnemyController
{
    [Header("Metalon")]
    private static Material[] _metalonMaterials;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    private const string StaticName = "Metalon";
    private const EnemyLevelType StaticType = EnemyLevelType.Normal;

    public override string EnemyName => StaticName;
    public override EnemyLevelType EnemyLevel => StaticType;
    
    protected override void Awake()
    {
        base.Awake();
        _metalonMaterials = GameManager.Instance.gameConfig.metalonMaterials;
    }

    public override void InitAfterGenerate()
    {
        base.InitAfterGenerate();
        SettingMaterials();
    }
    
    public override EnemyFactory CreateFactory(EnemyController enemyPrefab)
    {
        return new MetalonFactory(enemyPrefab as MetalonController);
    }
    

    public void SettingMaterials()
    {
        meshRenderer.material = _metalonMaterials[Random.Range(0, _metalonMaterials.Length)];
    }

    protected override void Move()
    {
        if (anim.GetCurrentAnimatorStateInfo(animCombatLayer).IsTag("Attack")) return;
        if (Physics.Raycast(focusTransform.position, focusTransform.forward,  2.5f, playerLayer | barrierLayer))       return;
        characterController.Move(curSpeed * Time.deltaTime * transform.forward.normalized);
    }

}
