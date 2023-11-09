using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Character/Enemy Data")]
public class EnemyData_SO : CharacterData_SO
{
    [Header("���˹����ж�")]
    public float attackDistanceNear;       //�̾���
    public float attackDistanceFar;        //������
    public float attackStateDistance;      //���빥��״̬�ľ���
}
