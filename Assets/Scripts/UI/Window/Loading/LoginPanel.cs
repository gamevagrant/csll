using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using QY.Open;

/// <summary>
/// 游客帐号单独保存，点facebook帐号正常走facebook登录流程。
/// 点游客登录检查之前是否登录过facebook帐号，如果有提示用户是否用facebook帐户登录，如果没有正常走游客登录流程。
/// 游客登录：如果本地没有保存游客帐号，创建GUID游客帐号登录并将GUID保存在本地。
/// 游客绑定facebook时，如果此facebook没有绑定任何帐号则绑定帐号播放奖励动画并清除绑定在本地的UGID
/// 如果此facebook绑定过帐号则跳转登录界面用此facebook帐号重新走登录流程，本地游客UGID保留
/// </summary>
public class LoginPanel : MonoBehaviour {

    //public InputField inputField;
    public GameObject guesBtn;
    public GameObject facebookBtn;
    public Text facebookName;
    public Slider slider;
    public GameObject loginTipPanel;
    public GameObject updateAppPanel;
    public Text userNameText;
    public Text levelText;
    

    private IOpenPlatform open;
    private string updateUrl;

    private void Awake()
    {
        EventDispatcher.instance.AddEventListener(EventEnum.LOADING_PROGRESS, OnLoadingProgress);
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_ASSETS_COMPLATE, OnUpdateAssetComplate);
        EventDispatcher.instance.AddEventListener(EventEnum.NEED_UPDATE_APP, OnUpdateApp);
        //inputField.gameObject.SetActive(false);
        guesBtn.SetActive(false);
        facebookBtn.SetActive(false);
        loginTipPanel.SetActive(false);
        updateAppPanel.SetActive(false);
        slider.value = 0;
        facebookName.text = "";


    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOADING_PROGRESS, OnLoadingProgress);
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_ASSETS_COMPLATE, OnUpdateAssetComplate);
        EventDispatcher.instance.RemoveEventListener(EventEnum.NEED_UPDATE_APP, OnUpdateApp);
    }

    private void OnLoadingProgress(BaseEvent evt)
    {
        LoadingEvent e = evt as LoadingEvent;
        slider.value = e.progress;
        slider.gameObject.SetActive(true);
    }

    private void OnUpdateAssetComplate(BaseEvent evt)
    {
        open = GameMainManager.instance.open;
        slider.gameObject.SetActive(false);
        
        InitPlatform();
    }

    private void OnUpdateApp(BaseEvent evt)
    {
        UpdateAppEvent e = evt as UpdateAppEvent;
        updateUrl = e.url;
        updateAppPanel.SetActive(true);

    }

    private void InitPlatform()
    {
        AccountManager.instance.Init(()=> {

            facebookBtn.SetActive(true);
            guesBtn.SetActive(true);
        });
       

    }

    private void LoinPlatform()
    {
        AccountManager.instance.LoinPlatform((issuccess)=> {
            if(!issuccess)
            {
                facebookName.text = "登录平台失败请重试...";
                facebookBtn.SetActive(true);
                guesBtn.SetActive(true);
            }else
            {
                facebookName.text = "正在登录游戏服务器...";
            }
        });
    }

    private void LoginGuest()
    {
        AccountManager.instance.LoginGuest();
    }


    public void OnClickGuestLogin()
    {
        facebookName.text = "";
        facebookBtn.SetActive(false);
        guesBtn.SetActive(false);

        SimpleUserData accountData = LocalDatasManager.loggedAccount;
        if (accountData!=null)
        {
            loginTipPanel.SetActive(true);
            userNameText.text = string.Format("姓名：{0}", accountData.name);
            levelText.text = string.Format("等级：{0}", accountData.level);
        }else
        {
            LoginGuest();
        }

    }

    public void OnClickFacebookLogin()
    {
       
        facebookName.text = "";
        facebookBtn.SetActive(false);
        guesBtn.SetActive(false);
        loginTipPanel.SetActive(false);
        LoinPlatform();
       
    }

    public void OnClickCloseTipsPanel()
    {
        loginTipPanel.SetActive(false);
    }
    
    public void OnClickContinueGuestLogin()
    {
        loginTipPanel.SetActive(false);
        LoginGuest();
    }

    public void OnClickUpdateApp()
    {
        if(!string.IsNullOrEmpty(updateUrl))
        {
            Application.OpenURL(updateUrl);
        }
    }

}
