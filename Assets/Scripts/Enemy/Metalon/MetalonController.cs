using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalonController : EnemyController
{
    //Metalon��anim�ڸ��������� ɾ��ʱҪɾ��������
    protected override void Awake()
    {
        base.Awake();
        originalPosition = transform.position;
    }

    protected override void SettingEnemyName()
    {
        enemyTypeName = "Metalon";
    }

    protected override void Move()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, 2 * curSpeed * Time.deltaTime * transform.forward.normalized.magnitude + 2.4f, playerLayer | barrierLayer))       return;
        characterController.Move(curSpeed * Time.deltaTime * transform.forward.normalized);
    }

}
