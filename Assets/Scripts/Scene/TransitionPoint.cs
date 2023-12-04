using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    [SerializeField] private GameSceneSO sceneToGo;

    [SerializeField] private Vector3 positionToGo;

    [SerializeField] private bool hideAfterTransition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            TransitionAction();
    }

    private void TransitionAction()
    {
        SceneLoader.Instance.SceneTransition(sceneToGo, positionToGo, true);
        if(hideAfterTransition) gameObject.SetActive(false);
    }
}