using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdLockEnemyCamera : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachine;

    private EnemyController lockedEnemy;

    private void Awake()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
    }

    public void Init(EnemyController currentEnemy)
    {
        lockedEnemy = currentEnemy;
        cinemachine.LookAt = lockedEnemy.transform;
    }


}
