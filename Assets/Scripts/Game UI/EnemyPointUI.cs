using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPointUI : MonoBehaviour
{
    [SerializeField] private GameObject executedPointPrefab;

    [SerializeField] private Image FocusPointImage;

    [SerializeField] private Transform pointParent;

    private Dictionary<EnemyController, GameObject> executedPoints = new();

    private EnemyController currentLockedEnemy;

    private void OnEnable()
    {
        GlobalEvent.EnemyEnterWeakState += CreateExecutedPoints;
        GlobalEvent.EnemyExitWeakState += DeleteExecutedPoints;

        GlobalEvent.EnterFocusOnEnemy += OnEnterFocusOnEnemy;
        GlobalEvent.ExitFocusOnEnemy += OnExitFocusOnEnemy;

    }

    private void OnDisable()
    {
        GlobalEvent.EnemyEnterWeakState -= CreateExecutedPoints;
        GlobalEvent.EnemyExitWeakState -= DeleteExecutedPoints;

        GlobalEvent.EnterFocusOnEnemy -= OnEnterFocusOnEnemy;
        GlobalEvent.ExitFocusOnEnemy -= OnExitFocusOnEnemy;
    }

    private void LateUpdate()
    {
        if (currentLockedEnemy != null)
        {
            FocusPointImage.transform.position = Camera.main.WorldToScreenPoint(currentLockedEnemy.transform.position);
        }
        foreach(var redPoint in executedPoints)
        {
            redPoint.Value.transform.position = Camera.main.WorldToScreenPoint(redPoint.Key.transform.position);
        }
    }

    //锁定敌人的事件
    public void OnEnterFocusOnEnemy(EnemyController enemy)
    {
        currentLockedEnemy = enemy;
        FocusPointImage.transform.position = Camera.main.WorldToScreenPoint(currentLockedEnemy.transform.position);
        FocusPointImage.gameObject.SetActive(true);
    }

    //取消锁定的事件
    public void OnExitFocusOnEnemy()
    {
        currentLockedEnemy = null;
        FocusPointImage.gameObject.SetActive(false);
    }

    //生成处决的points
    public void CreateExecutedPoints(EnemyController enemy)
    {
        if (enemy == null) return;
        if(!executedPoints.ContainsKey(enemy))
        {
            executedPoints.Add(enemy, Instantiate(executedPointPrefab, pointParent));
            executedPoints[enemy].transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position);
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
