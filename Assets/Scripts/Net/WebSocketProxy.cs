using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BestHTTP;
using BestHTTP.WebSocket;

public class WebSocketProxy {

    public Action onOpen;
    public Action<string> onMessage;
    public Action<UInt16, string> onClose;
    public Action<string> onError;

    //string address;
    WebSocket webSocket;


    public WebSocketProxy(string address)
    {
        //this.address = address;
        webSocket = new WebSocket(new Uri(address));
#if !BESTHTTP_DISABLE_PROXY && !UNITY_WEBGL
        if (HTTPManager.Proxy != null)
            webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
#endif

        webSocket.OnOpen += (ws)=>{
            Debug.Log("onOpen");
            if(onOpen!=null)
            {
                onOpen();
            }
            
        };
        webSocket.OnMessage += (ws,message)=>
        {
            Debug.Log("onMessage");
            if(onMessage!=null)
            {
                onMessage(message);
            }
        };
        webSocket.OnClosed += (ws,code,message)=>
        {
            Debug.Log("onClose");
            if(onClose!=null)
            {
                onClose(code,message);
            }
            webSocket = null;
        };
        webSocket.OnError += (ws,ex)=>
        {
            Debug.Log("onError");
            string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
            if (ws.InternalRequest.Response != null)
                errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
#endif
            if(onError!=null)
            {
                onError(errorMsg);
            }
            webSocket = null;
        };
        open();
    }

    public void open()
    {
        webSocket.Open();
    }

    public void close()
    {
        webSocket.Close(1000, "Closed");
    }

    public void send(string msgToSend)
    {
        webSocket.Send(msgToSend);
    }


 
}
