using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-50)]
public class SceneLoader : Singleton<SceneLoader>,ISavable
{
    public GameSceneSO firstScene;
    
    public float fadeDuration;

    [SerializeField] private Transform player;

    [SerializeField] private GameSceneSO restScene;

    [SerializeField] private GameSceneSO currentScene;

    private GameSceneSO sceneToGo;

    private Vector3 posToGo;

    private bool needFade;

    public bool IsLoading { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        sceneToGo = firstScene;
        currentScene = firstScene;
        LoadNewScene();
    }

    private void Start()
    {
        AudioManager.Instance.PlayBgmBySceneType(currentScene.sceneType);
        MouseManager.Instance.SetMouseCursorBySceneType(currentScene.sceneType);
        GlobalEvent.CallEnterMenuSceneEvent();
    }
    
    private void OnEnable()
    {
        (this as ISavable).RegisterSaveData();
        GlobalEvent.newGameEvent += TransitionToRestScene;
    }

    private void OnDisable()
    {
        (this as ISavable).UnRegisterSaveData();
        GlobalEvent.newGameEvent -= TransitionToRestScene;
    }

    public SceneType GetCurrentSceneType()
    {
        if (currentScene != null) return currentScene.sceneType;
        return SceneType.Menu;
    }
    
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
            //player.gameObject.SetActive(currentScene.sceneType != SceneType.Persistent);
            IsLoading = false;
            GlobalEvent.CallAfterSceneLoadEvent(posToGo);
            if (needFade)
            {
                UIManager.Instance.fadeCanvas.FadeOut(fadeDuration);
            }
            SceneManager.SetActiveScene(obj.Result.Scene);
            if(currentScene.sceneType!=SceneType.Persistent)AudioManager.Instance.PlayBgmBySceneType(currentScene.sceneType);
            MouseManager.Instance.SetMouseCursorBySceneType(currentScene.sceneType);
            UIManager.Instance.SetPanelAfterLoad(currentScene.sceneType);
            
            if (currentScene.sceneType == SceneType.Menu) GlobalEvent.CallEnterMenuSceneEvent();
            else GlobalEvent.CallExitMenuSceneEvent();
        }
        else
        {
            Debug.LogError("Failed to load scene: " + obj.OperationException.Message);
        }
        
    }

    private void TransitionToRestScene()
    {
        SceneTransition(restScene, Vector3.zero, true);
    }


    #region Save And Load
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
    

    #endregion


}
