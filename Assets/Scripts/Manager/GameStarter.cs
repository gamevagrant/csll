using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏启动器
/// </summary>
public class GameStarter : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        
        
	}

    private void Start()
    {
        init();
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
        GameMainManager.instance.netManager.Login(12, (res, data) => {
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
        //GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow);

        Dictionary<UISettings.UIWindowID, object> stateData = new Dictionary<UISettings.UIWindowID, object>();
        stateData.Add(UISettings.UIWindowID.UITopBarWindow, null);
        stateData.Add(UISettings.UIWindowID.UIWheelWindow, null);
        stateData.Add(UISettings.UIWindowID.UISideBarWindow, null);
        GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(stateData, null));
    }
}
