using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void init()
    {
        login();
    }

    private void login()
    {
        NetManager.instance.login("899836", (res, data) => {
            if(data.isOK)
            {
                GameModel.userData = data.data;
                EventDispatcher.instance.dispatchEvent(EventEnum.UPDATE_USERDATA, data.data);
            }else
            {
                Debug.Log("登录失败:"+data.errmsg);
            }
        });
    }
}
