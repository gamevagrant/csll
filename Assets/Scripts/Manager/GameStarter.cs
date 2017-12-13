using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
using QY.Open;
/// <summary>
/// 游戏启动器
/// </summary>
public class GameStarter : MonoBehaviour {

	// Use this for initialization
	void Awake () {

        GameObject.DontDestroyOnLoad(gameObject);
        EventDispatcher.instance.AddEventListener(EventEnum.LOGIN_START, OnLoginHandle);
        EventDispatcher.instance.AddEventListener(EventEnum.REQUEST_ERROR, OnRequestErrorHandle);
    }

    private void Start()
    {
        init();
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOGIN_START, OnLoginHandle);
        EventDispatcher.instance.RemoveEventListener(EventEnum.REQUEST_ERROR, OnRequestErrorHandle);
    }
    private void init()
    {
        gameObject.AddComponent<AssetBundleLoadManager>();
        gameObject.AddComponent<AssetLoadManager>();
        if (GameSetting.isDebug)
            gameObject.AddComponent<QY.Debug.DebugTools>();

        GameMainManager.instance.mono = this;
        GameMainManager.instance.open = new OpenFacebook();

        if (GameSetting.isUseAssetBundle)
        {
            UpdateAssets updateAsset = new UpdateAssets();
            updateAsset.onComplate += UpdateAssetsComplate;
            updateAsset.StartUpdate();
        }else
        {
            UpdateAssetsComplate();
        }
        
    }

    private void UpdateAssetsComplate()
    {
        EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.UPDATE_ASSETS_COMPLATE));
        LoadConfig();
        
    }


    private void OnLoginHandle(BaseEvent evt)
    {
        
        string token = evt.datas[0].ToString();
        long expirationTime = (long)evt.datas[1];
        GameMainManager.instance.netManager.LoginFB(token, expirationTime,(res, data) => 
        {
            if (data.isOK)
            {
                if(data.data.tutorial<18)
                {
                    JumpOverTutorial(18 - (int)data.data.tutorial,()=> {

                        StartCoroutine(LoadMainScene());
                    });
                }
                else
                {
                    StartCoroutine(LoadMainScene());
                }
                

            }
            else
            {
                Debug.Log("登录失败:" + data.errmsg);
            }
        });

    }

    private void OnRequestErrorHandle(BaseEvent e)
    {
        RequestErrorEvent evt = e as RequestErrorEvent;
        if(GameMainManager.instance.uiManager!=null)
        {
            GameMainManager.instance.uiManager.isWaiting = false;
            Alert.Show("连接失败："+evt.request.State.ToString(),Alert.OK,(isOK)=> {
                HttpProxy.SendRequest(evt.request);
            },"重试");
        }else
        {
            HttpProxy.SendRequest(evt.request);
        }
    }

    private void JumpOverTutorial(int count,System.Action callback = null)
    {
        if(count>0)
        {
            Debug.Log("正在跳跃新手引导："+count);
            GameMainManager.instance.netManager.TutorialComplete((ret, res) =>
            {
                if (res.isOK)
                {
                    count--;
                    JumpOverTutorial(count, callback);
                    
                }
            });
        }else
        {
            if (callback != null)
            {
                Debug.Log("跳跃新手引导结束");
                callback();
            }
        }
       
    }

    private void LoadConfig()
    {
        AssetBundleLoadManager.Instance.LoadAsset<Object>(FilePathTools.getConfigPath("IslandConfig"), (data) =>
        {
            string json =  data.ToString();
            IslandConfig config = JsonMapper.ToObject<IslandConfig>(json);
            GameMainManager.instance.model.islandConfig = config;
        });

    }

    private IEnumerator LoadMainScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
        float progress = 0;
        while(!asyncLoad.isDone)
        {
            if(asyncLoad.progress != progress)
            {
                progress = asyncLoad.progress;
                EventDispatcher.instance.DispatchEvent(new LoadingEvent("LoadScene",progress));
            }
            yield return null;
        }

        GameMainManager.instance.Init();
        GameMainManager.instance.uiManager.ChangeState(new MainState(0));
        
    }

}
