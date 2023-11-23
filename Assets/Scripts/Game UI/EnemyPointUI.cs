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
            focusPointImage.transform.position = Camera.main.WorldToScreenPoint(currentLockedEnemy.transform.position);
        }
        foreach(var redPoint in executedPoints)
        {
            redPoint.Value.transform.position = Camera.main.WorldToScreenPoint(redPoint.Key.transform.position);
        }
    }

    //�������˵��¼�
    public void OnEnterFocusOnEnemy(EnemyController enemy)
    {
        currentLockedEnemy = enemy;
        focusPointImage.transform.position = Camera.main.WorldToScreenPoint(currentLockedEnemy.transform.position);
        focusPointImage.gameObject.SetActive(true);
    }

    //ȡ���������¼�
    public void OnExitFocusOnEnemy()
    {
        currentLockedEnemy = null;
        focusPointImage.gameObject.SetActive(false);
    }

    //���ɴ�����points
    public void CreateExecutedPoints(EnemyController enemy)
    {
        if (enemy == null) return;
        if(!executedPoints.ContainsKey(enemy))
        {
            executedPoints.Add(enemy, Instantiate(executedPointPrefab, pointParent));
            executedPoints[enemy].transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position);
        }
    }

    //ɾ��������points
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
