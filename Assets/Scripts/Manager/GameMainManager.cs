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
    public GameModel model;
    public MonoBehaviour mono;//全局脚本，可以使用monobehaviour方法

    public GameMainManager()
    {
        netManager = new NetManager();
        websocketMsgManager = new WebSocketMsgManager();
        model = new GameModel();
        open = new OpenFacebook();
    }

    public void Init()
    {
        uiManager = GameObject.Find("UIRoot").GetComponent<UIManager>();//暂时这样 之后改成UIRoot从UIManager自动创建
        audioManager = AudioManager.instance;
        audioManager.SetSoundPathProxy(FilePathTools.getAudioPath);
        audioManager.SetMusicPathProxy(FilePathTools.getAudioPath);
       
    }
   
}
