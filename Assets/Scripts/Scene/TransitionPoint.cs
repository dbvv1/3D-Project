using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    [SerializeField] private GameSceneSO sceneToGo;

    [SerializeField] private Vector3 positionToGo;
    
    private void OnTriggerEnter(Collider other)
    {
        TransitionAction();
    }

    private void TransitionAction()
    {
        SceneLoader.Instance.SceneTransition(sceneToGo, positionToGo, true);
    }
    
}
