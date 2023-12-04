using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>,ISavable
{
    public GameSceneSO firstScene;
    
    public float fadeDuration;

    [SerializeField] private Transform player;

    private GameSceneSO currentScene;

    private GameSceneSO sceneToGo;

    private Vector3 posToGo;

    private bool needFade;

    public bool IsLoading { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        sceneToGo = firstScene;
        LoadNewScene();

    }

    private void OnEnable()
    {
        (this as ISavable).RegisterSaveData();
    }

    private void OnDisable()
    {
        (this as ISavable).UnRegisterSaveData();
    }

    public SceneType GetCurrentSceneType => currentScene.sceneType;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneToGo">ǰ���ĳ���</param>
    /// <param name="posToGo">�³����ĳ�ʼ��λ��</param>
    /// <param name="needFade">�Ƿ���Ҫ���뵭��</param>
    public void SceneTransition(GameSceneSO sceneToGo, Vector3 posToGo, bool needFade)
    {
        if (IsLoading) return;
        //�趨���ص�״̬����Ϣ
        IsLoading = true;
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
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            currentScene = sceneToGo;
            IsLoading = false;
            GlobalEvent.CallAfterSceneLoadEvent(posToGo);
            if (needFade)
            {
                UIManager.Instance.fadeCanvas.FadeOut(fadeDuration);
            }
            SceneManager.SetActiveScene(obj.Result.Scene);
        }
        else
        {
            Debug.LogError("Failed to load scene: " + obj.OperationException.Message);
        }
        


    }

    public string GetDataID()
    {
        return "Scene";
    }

    public void SaveData(Data data)
    {
        data.SaveScene(currentScene);
        data.playerPos = new SerializeVector3(player.position);
    }

    public void LoadData(Data data)
    {
        IsLoading = false;
        SceneTransition(data.LoadScene(), data.playerPos.ToVector3(), true);


    }

}
