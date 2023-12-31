using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnemyPointUI : MonoBehaviour
{
    [SerializeField] private GameObject executedPointPrefab;

    [SerializeField] private Image focusPointImage;

    [SerializeField] private Transform pointParent;
    
    [SerializeField] private Camera mainCamera;

    private Dictionary<EnemyController, GameObject> executedPoints = new();

    private EnemyController currentLockedEnemy;

    private void OnEnable()
    {
        GlobalEvent.enemyEnterWeakState += CreateExecutedPoints;
        GlobalEvent.enemyExitWeakState += DeleteExecutedPoints;

        GlobalEvent.enterFocusOnEnemy += OnEnterFocusOnEnemy;
        GlobalEvent.exitFocusOnEnemy += OnExitFocusOnEnemy;

    }

    private void OnDisable()
    {
        GlobalEvent.enemyEnterWeakState -= CreateExecutedPoints;
        GlobalEvent.enemyExitWeakState -= DeleteExecutedPoints;

        GlobalEvent.enterFocusOnEnemy -= OnEnterFocusOnEnemy;
        GlobalEvent.exitFocusOnEnemy -= OnExitFocusOnEnemy;
    }

    private void LateUpdate()
    {
        if (currentLockedEnemy != null)
        {
            focusPointImage.transform.position =
                mainCamera.WorldToScreenPoint(currentLockedEnemy.focusTransform.position);
        }
        foreach(var redPoint in executedPoints)
        {
            redPoint.Value.transform.position = mainCamera.WorldToScreenPoint(redPoint.Key.focusTransform.position);
        }
    }

    //锁定敌人的事件
    public void OnEnterFocusOnEnemy(EnemyController enemy)
    {
        currentLockedEnemy = enemy;
        focusPointImage.transform.position =
            mainCamera.WorldToScreenPoint(currentLockedEnemy.focusTransform.position);
        focusPointImage.gameObject.SetActive(true);
    }

    //取消锁定的事件
    public void OnExitFocusOnEnemy()
    {
        currentLockedEnemy = null;
        focusPointImage.gameObject.SetActive(false);
    }

    //生成处决的points
    public void CreateExecutedPoints(EnemyController enemy)
    {
        if (enemy == null) return;
        if(!executedPoints.ContainsKey(enemy))
        {
            executedPoints.Add(enemy, Instantiate(executedPointPrefab, pointParent));
            executedPoints[enemy].transform.position = mainCamera.WorldToScreenPoint(enemy.focusTransform.position);
        }
    }

    //删除处决的points
    public void DeleteExecutedPoints(EnemyController enemy)
    {
        if (enemy == null) return;
        if(executedPoints.ContainsKey(enemy))
        {
            Destroy(executedPoints[enemy].gameObject);
            executedPoints.Remove(enemy);
        }
    }
}
