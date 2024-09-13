using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable
{
    public Transform playerTrans;
    public Vector3 menuPosition;
    public Vector3 firstPosition;

    [Header("监听")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;

    [Header("广播")]
    public SceneLoadEventSO unLoadedSceneEvent;
    public FadeEventSO fadeEvent;
    public VoidEventSO afterSceneLoadEvent;

    [Header("场景")]
    public GameSceneSO menuScene;
    public GameSceneSO firstLoadScene;
    private GameSceneSO scene2Load;
    private GameSceneSO currentLoadScene;

    private Vector3 position2Go;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;
    private void Awake()
    {
        // Addressables.LoadSceneAsync(gameScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = gameScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame(); 
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnBackToMenu;


        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackToMenu;


        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }



    private void NewGame()
    {
        scene2Load = firstLoadScene;
        //OnLoadRequestEvent(scene2Load, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(scene2Load,firstPosition,true);
    }

    /// <summary>
    /// 场景加载请求
    /// </summary>
    /// <param name="location2Load"></param>
    /// <param name="ply2Go"></param>
    /// <param name="fadeScreen"></param>
    private void OnLoadRequestEvent(GameSceneSO location2Load, Vector3 ply2Go, bool fadeScreen)
    {
        if (isLoading)
            return; 


        isLoading = true;   
        scene2Load = location2Load; 
        position2Go = ply2Go;
        this.fadeScreen = fadeScreen;

        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene(); 
        }

 
        //Addressables.LoadSceneAsync(gameScene.sceneReference, LoadSceneMode.Additive);
    }
    private void OnBackToMenu()
    {
        scene2Load = menuScene;
        loadEventSO.RaiseLoadRequestEvent(scene2Load, menuPosition, true);
    }
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeDuration > 0)
        {
            fadeEvent.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);

        //广播血条显示
        unLoadedSceneEvent.RaiseLoadRequestEvent(scene2Load, position2Go, true);

        yield return currentLoadScene.sceneReference.UnLoadScene();

        playerTrans.gameObject.SetActive(false);    
        LoadNewScene(); 
    }

    private void LoadNewScene()
    {
        var loadingOption = scene2Load.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    /// <summary>
    /// 加载场景后
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadScene = scene2Load;

        playerTrans.position = position2Go;


        playerTrans.gameObject.SetActive(true); 
        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);    
        }

        isLoading = false;

        if(currentLoadScene.sceneType == SceneType.Location)
            afterSceneLoadEvent.RaiseEvent();
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();  
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefination>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            position2Go = data.characterPosDict[playerID].ToVector3();  
            scene2Load = data.GetSavedScene();

            OnLoadRequestEvent(scene2Load, position2Go,true);
        }

    }
}
