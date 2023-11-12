using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public GameSceneSO firstScene;

    public float fadeDuration;

    [SerializeField] private Transform player;

    private GameSceneSO currentScene;

    private GameSceneSO sceneToGo;

    private Vector3 posToGo;

    private bool needFade;

    private bool isLoading;
    
    protected override void Awake()
    {
        base.Awake();
        firstScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        currentScene = firstScene;

    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ǰ���ĳ���"></param>
    /// <param name="�³����ĳ�ʼ��λ��"></param>
    /// <param name="�Ƿ���Ҫ���뵭��"></param>
    public void SceneTransition(GameSceneSO sceneToGo, Vector3 posToGo, bool needFade)
    {
        if (isLoading) return;
        //�趨���ص�״̬����Ϣ
        isLoading = true;
        this.sceneToGo = sceneToGo;
        this.posToGo = posToGo;
        this.needFade = needFade;
        GlobalEvent.CallBeforeSceneLoadEvent();
        StartCoroutine(SceneTransition());
    }

    private IEnumerator SceneTransition()
    {
        if (needFade)
        {
            UIManager.Instance.fadeCanvas.Fadein(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);
        
        //�ȴ���ǰ����ж����
        yield return currentScene.sceneReference.UnLoadScene();
        
        //�����µĳ���
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOperation=sceneToGo.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOperation.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentScene = sceneToGo;
        isLoading = false;
        GlobalEvent.CallAfterSceneLoadEvent(posToGo);
        if (needFade)
        {
            UIManager.Instance.fadeCanvas.FadeOut(fadeDuration);
        }
        


    }
}
