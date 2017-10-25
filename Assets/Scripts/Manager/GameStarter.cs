using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏启动器
/// </summary>
public class GameStarter : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        
        init();
	}

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}

    private void init()
    {
        gameObject.AddComponent<AssetBundleLoadManager>();
        gameObject.AddComponent<AssetLoadManager>();
       
        login();
    }

    private void login()
    {
        GameMainManager.instance.netManager.Login(899836, (res, data) => {
            if (data.isOK)
            {
                StartCoroutine(OpenUI());
            }
            else
            {
                Debug.Log("登录失败:" + data.errmsg);
            }
        });
    }

    private IEnumerator OpenUI()
    {
        yield return new WaitForSeconds(1);
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow);
    }
}
