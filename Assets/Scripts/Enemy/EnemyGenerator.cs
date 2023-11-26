using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private MetalonController metalonPrefab;
    
    private MetalonFactory metalonFactory;
    
    private void Awake()
    {
        metalonFactory = new MetalonFactory(metalonPrefab);
    }

    private void Start()
    {
        metalonFactory.CreateEnemy();
    }
}
