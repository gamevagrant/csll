using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        UpdateAssets updateAsset = new UpdateAssets();
        updateAsset.onComplate += UpdateAssetsComplate;
        updateAsset.StartUpdate();
    }

    private void UpdateAssetsComplate()
    {
        EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.UPDATE_ASSETS_COMPLATE));
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
        //StartCoroutine(OpenUI());
        //yield return new WaitForSeconds(1);
        GameMainManager.instance.Init();
        GameMainManager.instance.uiManager.ChangeState(new MainState(0));
        
    }
    /*
    private IEnumerator OpenUI()
    {
        yield return new WaitForSeconds(1);
        //GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow);

        //Dictionary<UISettings.UIWindowID, object> stateData = new Dictionary<UISettings.UIWindowID, object>();
        //stateData.Add(UISettings.UIWindowID.UITopBarWindow, null);
        //stateData.Add(UISettings.UIWindowID.UIWheelWindow, null);
        //stateData.Add(UISettings.UIWindowID.UISideBarWindow, null);
        //GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(stateData, null));

        GameMainManager.instance.uiManager.ChangeState(new MainState());
    }
    */
}
