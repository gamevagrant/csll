using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameModel model;

    public GameMainManager()
    {
        netManager = new NetManager();
        uiManager = GameObject.Find("UIRoot").GetComponent<UIManager>();//暂时这样 之后改成UIRoot从UIManager自动创建
        model = new GameModel();
    }

   
}
