using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏启动器
/// </summary>
public class GameStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void init()
    {
        gameObject.AddComponent<AssetBundleLoadManager>();
        gameObject.AddComponent<AssetBundleLoadManager>();
        login();
    }

    private void login()
    {
        GameMainManager.instance.netManager.Login(1, (res, data) => {
            if (data.isOK)
            {
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow);
            }
            else
            {
                Debug.Log("登录失败:" + data.errmsg);
            }
        });
    }
}
