using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
/// <summary>
/// 游戏启动器
/// </summary>
public class GameStarter : MonoBehaviour {

	// Use this for initialization
	void Awake () {

        GameObject.DontDestroyOnLoad(gameObject);
        EventDispatcher.instance.AddEventListener(EventEnum.LOGIN_START, OnLoginHandle);
	}

    private void Start()
    {
        init();
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOGIN_START, OnLoginHandle);
    }
    private void init()
    {
        gameObject.AddComponent<AssetBundleLoadManager>();
        gameObject.AddComponent<AssetLoadManager>();
        GameMainManager.instance.mono = this;

        if(GameSetting.isUseAssetBundle)
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
        string openID = evt.datas[0].ToString();
        GameMainManager.instance.netManager.Login(openID, (res, data) => {
            if (data.isOK)
            {
                StartCoroutine(LoadMainScene());
                PlayerPrefs.SetString("OpenID",openID);

            }
            else
            {
                Debug.Log("登录失败:" + data.errmsg);
            }
        });

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
