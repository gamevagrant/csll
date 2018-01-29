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
    public Text loadingTips;
    public Slider slider;
    public Text userNameText;
    public Text levelText;
    public GameObject loginTipPanel;
    public GameObject updateAppPanel;
    public GameObject privacyTipsPanel;
    public InputNamePanel inputNamePanel;

    private IOpenPlatform open;
    private string updateUrl;
    private Action onStartLogin;
    private float progress;
    private string guestName;

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
        inputNamePanel.gameObject.SetActive(false);
        slider.value = 0;
        loadingTips.text = "";

        inputNamePanel.onConfirmName += OnLoginGuestHandle;
        inputNamePanel.onCancle += OnCancleLoginHandle;
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOADING_PROGRESS, OnLoadingProgress);
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_ASSETS_COMPLATE, OnUpdateAssetComplate);
        EventDispatcher.instance.RemoveEventListener(EventEnum.NEED_UPDATE_APP, OnUpdateApp);
        inputNamePanel.onConfirmName -= OnLoginGuestHandle;
        inputNamePanel.onCancle -= OnCancleLoginHandle;
    }

    private void Update()
    {
        if(slider.gameObject.activeSelf)
        {
            slider.value = Mathf.Min(slider.value + 1 * Time.deltaTime, progress);
        }
        
    }

    private void OnLoadingProgress(BaseEvent evt)
    {
        LoadingEvent e = evt as LoadingEvent;
        progress = e.progress;

        switch (e.name)
        {
            case "UpdateAssets":
                loadingTips.text = "正在加载本地资源，不会消耗流量";
                progress = progress*0.8f;
                break;
            case "Preloader":
                loadingTips.text = "正在预加载资源";
                progress = 0.8f + progress * 0.2f;
                break;
            case "Login":
                loadingTips.text = "正在登录服务器";
                progress = progress * 0.5f;
                break;
            case "LoadScene":
                loadingTips.text = "正在切换场景";
                progress = 0.5f + progress * 0.5f;
                break;
            default:
                break;
        }
       
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
        onStartLogin = () =>
        {
            LoinPlatform();
        };

        if (!LocalDatasManager.isShowPrivacyTips)
        {
           
            privacyTipsPanel.SetActive(true);
        }
        else
        {
            AccountManager.instance.LoinPlatform((issuccess) => {
                if (!issuccess)
                {
                    loadingTips.text = "登录平台失败请重试...";
                    facebookBtn.SetActive(true);
                    guesBtn.SetActive(true);
                }
                else
                {
                    loadingTips.text = "正在登录游戏服务器...";
                }
            });
        }
       
    }

    private void LoginGuest()
    {
        onStartLogin = () =>
        {
            LoginGuest();
        };

        if (!LocalDatasManager.isShowPrivacyTips)
        {
           
            privacyTipsPanel.SetActive(true);

        }else if (LocalDatasManager.loggedGuest == null && string.IsNullOrEmpty(guestName))
        {
            inputNamePanel.gameObject.SetActive(true);
        }
        else
        {
            AccountManager.instance.LoginGuest(guestName);
        }
       
    }

    private void OnLoginGuestHandle(string name)
    {
        guestName = name;
        onStartLogin();
    }
    private void OnCancleLoginHandle()
    {
        facebookBtn.SetActive(true);
        guesBtn.SetActive(true);
    }

    public void OnClickGuestLogin()
    {
        loadingTips.text = "";
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
        
        loadingTips.text = "";
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

    public void OnClickPrivacyTipsBtn()
    {
        LocalDatasManager.isShowPrivacyTips = true;

        privacyTipsPanel.SetActive(false);
        if(onStartLogin!=null)
        {
            onStartLogin();
        }
    }

}
