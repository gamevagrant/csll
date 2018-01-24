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

#if DEVELOPMENT_BUILD
        gameObject.AddComponent<QY.Debug.DebugTools>();
#elif !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
#endif
        GameMainManager.instance.mono = this;

        UpdateVersion updateVersion = new UpdateVersion();
        updateVersion.StartUpdate(() => {
            UpdateAssetsComplate();

        }, (url) => {

            EventDispatcher.instance.DispatchEvent(new UpdateAppEvent(url));
        });

    }

    private void UpdateAssetsComplate()
    {
        if(GameSetting.isUseAssetBundle)
        {
            GameMainManager.instance.preloader.StartPreloader(this, () =>
            {
                Debug.Log("预加载完成");
                LoadConfig();
            });
        }else
        {
            LoadConfig();
        }
        
        

    }

    private void LoadConfig()
    {
        GameMainManager.instance.configManager.LoadConfig(() =>
        {
            Debug.Log("配置文件加载完成");
            EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.UPDATE_ASSETS_COMPLATE));
        });

    }

    private void OnLoginHandle(BaseEvent e)
    {
        StartCoroutine(LoadMainScene());

    }


    private IEnumerator LoadMainScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Main")
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
            yield return asyncLoad;
            GameMainManager.instance.Init();
        }
        else
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
            float progress = 0;
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress != progress)
                {
                    progress = asyncLoad.progress / 2;
                    EventDispatcher.instance.DispatchEvent(new LoadingEvent("LoadScene", progress));
                }
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
            GameMainManager.instance.Init();

            yield return new WaitForSeconds(2);
            asyncLoad = SceneManager.UnloadSceneAsync("Login");
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress != progress)
                {
                    progress = 0.5f + asyncLoad.progress / 2;
                    EventDispatcher.instance.DispatchEvent(new LoadingEvent("LoadScene", progress));
                }
                yield return null;
            }
        }

        yield return null;
    }

}