using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalonController : EnemyController
{
    [Header("Metalon")]
    private static Material[] _metalonMaterials;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    protected override void Awake()
    {
        base.Awake();
        _metalonMaterials = GameManager.Instance.gameConfig.metalonMaterials;
    }

    protected override void SettingEnemyName()
    {
        enemyTypeName = "Metalon";
    }

    public void SettingMaterials()
    {
        meshRenderer.material = _metalonMaterials[Random.Range(0, _metalonMaterials.Length)];
    }

    protected override void Move()
    {
        if (anim.GetCurrentAnimatorStateInfo(animCombatLayer).IsTag("Attack")) return;
        if (Physics.Raycast(FocusTransform.position, FocusTransform.forward,  2.5f, playerLayer | barrierLayer))       return;
        characterController.Move(curSpeed * Time.deltaTime * transform.forward.normalized);
    }

}
