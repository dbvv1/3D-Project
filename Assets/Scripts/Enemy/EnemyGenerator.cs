using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private MetalonController metalonPrefab;

    [SerializeField] private GolemController golemPrefab;
    
    private MetalonFactory metalonFactory;

    private GolemFactory golemFactory;
    
    private void Awake()
    {
        metalonFactory = new MetalonFactory(metalonPrefab);
        golemFactory = new GolemFactory(golemPrefab);
    }
    
}
