using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Test : MonoBehaviour {

    WebSocketProxy wsp;
    // Use this for initialization
    void Start ()
    {
        // testT();
      


        string url = "http://www.caishenlaile.com/game/basic/login";
        HttpProxy.SendPostRequest<LoginMessage>(url, new Dictionary<string, object>(), (res, msg) => {
           // Debug.Log(user.data.ToString());
           if(!msg.isOK)
            {
                Debug.Log(msg.errmsg);
            }else
            {
                Debug.Log(msg.ToString());

                wsp = new WebSocketProxy("ws://10.0.2.55:8080/ws/conn?uid=2");
                wsp.onOpen += () => {
                    Debug.Log("on open");
                    wsp.send("{\"cmd\":\"info\",\"fromid\":\"1\",\"toid\":\"2\",\"content\":\"hello?\"}");
                };
                wsp.onMessage += (s) => { Debug.Log("onMessage"+s); };
                wsp.onError += (s) => { Debug.Log("onError"); };
            }
            });


	}
	
    private void testT()
    {
        string s = Resources.Load("a").ToString();
        RollerItemData l = JsonMapper.ToObject<RollerItemData>(s);
        Debug.Log(l);
    }
	// Update is called once per frame
	void Update () {

	}

    private void OnGUI()
    {
        if (GUILayout.Button("Send", GUILayout.MaxWidth(170)))
        {
            // Send message to the server
            wsp.send("{\"uid\":1,\"toid\":2,\"extra\":\"hello\"}");
            Debug.Log("send");
        }
    }

    public void OnChangeVale(bool a)
    {
        Debug.Log("OnChangeVale");
    }

    public QY.UI.Button btn;
    public void OnEnableButton()
    {
        btn.isIgnoreLock = true;
        QY.UI.Interactable.isLock = !QY.UI.Interactable.isLock;
    }
}
