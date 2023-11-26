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
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, 2 * curSpeed * Time.deltaTime * transform.forward.normalized.magnitude + 2.4f, playerLayer | barrierLayer))       return;
        characterController.Move(curSpeed * Time.deltaTime * transform.forward.normalized);
    }

}
