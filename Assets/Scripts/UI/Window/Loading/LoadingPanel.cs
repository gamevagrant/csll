using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QY.Open;


public class LoadingPanel : MonoBehaviour {

    //public InputField inputField;
    public GameObject guesBtn;
    public GameObject facebookBtn;
    public Text facebookName;
    public Slider slider;
    private IOpenPlatform open;

    private void Awake()
    {
        EventDispatcher.instance.AddEventListener(EventEnum.LOADING_PROGRESS, OnLoadingProgress);
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_ASSETS_COMPLATE, OnUpdateAssetComplate);

        //inputField.gameObject.SetActive(false);
        guesBtn.SetActive(false);
        facebookBtn.SetActive(false);
        slider.value = 0;
        facebookName.text = "";


    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOADING_PROGRESS, OnLoadingProgress);
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_ASSETS_COMPLATE, OnUpdateAssetComplate);
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

    private void InitPlatform()
    {
        open.Init(() => {

            if (open.IsLoggedIn)
            {
                LoginOpenPlatformComplate();
            }
            else
            {
                facebookBtn.SetActive(true);
            }
        });
    }

    private void LoinPlatform()
    {
        open.Login(() =>
        {
            if (open.IsLoggedIn)
            {
                LoginOpenPlatformComplate();
            }
            else
            {
                facebookName.text = "登录平台失败请重试...";
                facebookBtn.SetActive(true);
            }
        });
    }

    private void LoginOpenPlatformComplate()
    {
        facebookName.text = "正在登录游戏服务器...";
        facebookBtn.SetActive(false);
        EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.LOGIN_START, open.Token.tokenString, GameUtils.DateTimeToTimestamp(open.Token.expirationTime)));

    }

    public void OnClickGuesLogin()
    {
        string guesOpenID = PlayerPrefs.GetString("openID");
        if(!string.IsNullOrEmpty(guesOpenID))
        {
            EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.LOGIN_START, guesOpenID));
        }else
        {
            GameMainManager.instance.netManager.AddGuest((ret, res) =>
            {
                if(res.result == "ok")
                {
                    guesOpenID = res.UserInfo.OpenId;
                    PlayerPrefs.SetString("openID", guesOpenID);
                    EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.LOGIN_START, guesOpenID));
                }
              
            });
        }
    }

    public void OnClickFacebookLogin()
    {
        facebookName.text = "";
        facebookBtn.SetActive(false);
        LoinPlatform();
    }

    

}
