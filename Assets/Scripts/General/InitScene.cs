using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    public GameSceneSO persistentScene;

    private void Awake()
    {
        persistentScene.sceneReference.LoadSceneAsync(LoadSceneMode.Single, true);
    }
}
