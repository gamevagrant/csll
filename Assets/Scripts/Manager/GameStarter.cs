using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
using QY.Open;
/// <summary>
/// 游戏启动器
/// </summary>
public class GameStarter : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        EventDispatcher.instance.AddEventListener(EventEnum.LOGIN_COMPLATE, OnLoginHandle);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Login");
    }

    private void Start()
    {
       
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOGIN_COMPLATE, OnLoginHandle);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Login")
        {
            init();
        }
    }

    private void init()
    {
        gameObject.AddComponent<AssetBundleLoadManager>();
        gameObject.AddComponent<AssetLoadManager>();
        if (!GameSetting.isRelease)
            gameObject.AddComponent<QY.Debug.DebugTools>();

        GameMainManager.instance.mono = this;

        UpdateVersion updateVersion = new UpdateVersion(()=> {
            UpdateAssetsComplate();

        },(url)=> {

            EventDispatcher.instance.DispatchEvent(new UpdateAppEvent(url));
        });
        updateVersion.StartUpdate();

    }

    private void UpdateAssetsComplate()
    {

        LoadConfig();

    }

    private void LoadConfig()
    {
        GameMainManager.instance.configManager.LoadConfig(() =>
        {
            EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.UPDATE_ASSETS_COMPLATE));
        });

    }

    private void OnLoginHandle(BaseEvent e)
    {
        StartCoroutine(LoadMainScene());

    }

   
    private IEnumerator LoadMainScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
        float progress = 0;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress != progress)
            {
                progress = asyncLoad.progress;
                EventDispatcher.instance.DispatchEvent(new LoadingEvent("LoadScene", progress));
            }
            yield return null;
        }

        GameMainManager.instance.Init();
       

    }
}