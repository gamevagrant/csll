using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.Open;

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
        

    }





    private void OnRequestErrorHandle(BaseEvent e)
    {
        RequestErrorEvent evt = e as RequestErrorEvent;
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
        }
        else
        {
            HttpProxy.SendRequest(evt.request);
        }
    }


}
