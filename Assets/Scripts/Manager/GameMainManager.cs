using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.Open;
using QY.Guide;
using QY.UI;

public class GameMainManager {

    private static GameMainManager _instance;
    public static GameMainManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameMainManager();
            }
            return _instance;
        }
    }

    public IUIManager uiManager;
    public INetManager netManager;
    public IAudioManager audioManager;
    public IWebSocketMsgManager websocketMsgManager;
    public IOpenPlatform open;
    public ConfigManager configManager;
    public GameModel model;
	public IAPManager iap;
    public MonoBehaviour mono;//全局脚本，可以使用monobehaviour方法

    public GameMainManager()
    {
        netManager = new NetManager();
        websocketMsgManager = new WebSocketMsgManager();
        model = new GameModel();
        open = new OpenFacebook();
        configManager = new ConfigManager();
        iap = new IAPManager();

        EventDispatcher.instance.AddEventListener(EventEnum.REQUEST_ERROR, OnRequestErrorHandle);
    }

    ~GameMainManager()
    {

        EventDispatcher.instance.RemoveEventListener(EventEnum.REQUEST_ERROR, OnRequestErrorHandle);
    }

    public void Init()
    {
        uiManager = GameObject.Find("UIRoot").GetComponent<UIManager>();//暂时这样 之后改成UIRoot从UIManager自动创建
        audioManager = AudioManager.instance;
        audioManager.SetSoundPathProxy(FilePathTools.getAudioPath);
        audioManager.SetMusicPathProxy(FilePathTools.getAudioPath);

        GuideManager.instance.Init(model.userData.tutorial, configManager.guideDataConfig, OnProcessGuide, OnExecutedComplate);

        
        if(model.userData.last_action == 1 && model.userData.attackTargetUser != null)
        {
            Dictionary<UISettings.UIWindowID, object> stateData = new Dictionary<UISettings.UIWindowID, object>();
            stateData.Add(UISettings.UIWindowID.UIAttackWindow, model.userData.attackTargetUser);
            GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(stateData));
        }
        else if(model.userData.tutorial == 6 || model.userData.tutorial == 13)
        {
            Dictionary<UISettings.UIWindowID, object> stateData = new Dictionary<UISettings.UIWindowID, object>();
            stateData.Add(UISettings.UIWindowID.UIAttackWindow, model.userData.attackTarget.ToAttackTargetUserData());//新手教程中服务器发来的数据都是假的，造成部分数据或者逻辑不同 这里取不到model.userData.attackTargetUser
            GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(stateData));
        }
        else if(model.userData.last_action == 2 || model.userData.tutorial == 16)
        {
            Dictionary<UISettings.UIWindowID, object> stateData = new Dictionary<UISettings.UIWindowID, object>();
            stateData.Add(UISettings.UIWindowID.UIStealWindow, model.userData.stealIslands);
            GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(stateData));
        }
        else
        {
            if (model.userData.tutorial == 1)
            {
                GameMainManager.instance.uiManager.ChangeState(new MainState(0, 1));
            }
            else
            {
                GameMainManager.instance.uiManager.ChangeState(new MainState(0));
            }
        }

    }





    private void OnRequestErrorHandle(BaseEvent e)
    {
        RequestErrorEvent evt = e as RequestErrorEvent;
        if(evt.request.State != BestHTTP.HTTPRequestStates.TimedOut && evt.request.State != BestHTTP.HTTPRequestStates.ConnectionTimedOut)
        {
            Debug.Log(string.Format("请求失败：{0} ", evt.request.State.ToString()));
            return;
        }
        Debug.Log(string.Format("请求失败：{0} 正在尝试重新请求", evt.request.State.ToString()));
        if (GameMainManager.instance.uiManager != null)
        {
            GameMainManager.instance.uiManager.isWaiting = false;
            Alert.Show("连接失败：" + evt.request.State.ToString(), Alert.OK|Alert.CANCEL, (isOK) => {
                if(isOK == Alert.OK)
                {
                    HttpProxy.SendRequest(evt.request);
                }
               
            }, "重试");

            Waiting.Disable();
        }
        else
        {
            HttpProxy.SendRequest(evt.request);
        }
    }


    private void OnProcessGuide(GuideData guideData,Interactable interactable)
    {

       ShowGuide(guideData, interactable);
    }

    private void ShowGuide(GuideData guideData, Interactable interactable)
    {
        switch (guideData.actionType)
        {
            case GuideData.ActionType.click:
                uiManager.OpenWindow(UISettings.UIWindowID.UIGuideWindow, false, interactable);
                break;
            case GuideData.ActionType.tips:
                uiManager.OpenWindow(UISettings.UIWindowID.UIGuideTipsWindow, guideData.content);
                break;
            case GuideData.ActionType.open:
                UISettings.UIWindowID id = (UISettings.UIWindowID)System.Enum.Parse(typeof(UISettings.UIWindowID), guideData.content);
                uiManager.OpenWindow(id, guideData.content);
                break;
        }
    }

    private void OnExecutedComplate(GuideData guideData)
    {
        switch (guideData.actionType)
        {
            case GuideData.ActionType.click:
                uiManager.CloseWindow(UISettings.UIWindowID.UIGuideWindow);
                break;
            
        }
    }

}
