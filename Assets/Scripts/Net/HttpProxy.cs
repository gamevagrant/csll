using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using BestHTTP;
using System.Text;

/// <summary>
/// http代理类，通过这个类进行http请求，用以封装http具体实现
/// </summary>
public class HttpProxy {

    public static readonly string JsonHeaderType = "Content-Type";
    public static readonly string JsonHeaderValue = "application/json; charset=utf-8";
    public static float MaxInterval = 1 / 10;

    private static string LastUrl = "";
    private static DateTime LastTime = DateTime.MinValue;

    private static bool CheckMinTime(string url)
    {
        if (LastUrl.CompareTo(url) == 0)
        {
            if (DateTime.Now < LastTime.AddSeconds(MaxInterval))
            {
                return false;
            }
        }
        return true;
    }

    private static void MarkCheck(string url)
    {
        LastUrl = url;
        LastTime = DateTime.Now;
    }

    private static HTTPRequest MakePostRequest<T>(string url, Dictionary<string,object> data, Action<bool, T> callback = null) where T : NetMessage
    {
        HTTPRequest req = new HTTPRequest(new Uri(url), HTTPMethods.Post, (request, reponse) =>
        {
            bool ret = request.State == HTTPRequestStates.Finished;
            if (!ret)
            {
                Debug.LogErrorFormat("消息发送失败 request.State = {0} [{1}]", request.State.ToString(), url);
            }else
            {
                string msgStr = (reponse != null && !string.IsNullOrEmpty(reponse.DataAsText)) ? reponse.DataAsText : "";

                if (GameSetting.isDebug)
                    Debug.Log(msgStr);

                if (callback != null)
                {
                    try
                    {
                        T msg = JsonMapper.ToObject<T>(msgStr);
                        callback(ret, msg);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                }
            }

        });
        //req.AddHeader(JsonHeaderType, JsonHeaderValue);
        //req.RawData = Encoding.UTF8.GetBytes("6");
        //req.AddField("userId","6");
        foreach(string key in data.Keys)
        {
            req.AddField(key, data[key].ToString(), Encoding.UTF8);
        }
        return req;
    }

    private static HTTPRequest MakeGetRequest<T>(string url, Action<bool, T> callback = null) where T : NetMessage
    {
        HTTPRequest req = new HTTPRequest(new Uri(url), (request, reponse) =>
        {
            bool ret = request.State == HTTPRequestStates.Finished;
            if (!ret)
            {
                Debug.LogErrorFormat("request.State = {0} [{1}]", request.State.ToString(), url);
            }else
            {
                string msgStr = (reponse != null && !string.IsNullOrEmpty(reponse.DataAsText)) ? reponse.DataAsText : "";
                Debug.Log("<<=get=" + url + "==>>" + msgStr);
                if (callback != null)
                {
                    try
                    {
                        T msg = JsonMapper.ToObject<T>(msgStr);
                        callback(ret, msg);
                    }
                    catch (JsonException ex)
                    {
                        Debug.LogWarning(ex.ToString());
                    }
                }
            }

            
        });
        req.AddHeader(JsonHeaderType, JsonHeaderValue);
        req.Timeout = new TimeSpan(0, 0, 0, 5);
        return req;
    }

    public static bool SendPostRequest<T>(string url, Dictionary<string,object> data, Action<bool, T> callback = null) where T : NetMessage
    {
        if (!CheckMinTime(url))
        {
            Debug.LogError("Send too fast = " + url);
            return false;
        }

        HTTPRequest req = MakePostRequest<T>(url, data, callback);

        MarkCheck(url);
        return HTTPManager.SendRequest(req) != null;
    }

    public static bool SendGetRequest<T>(string url, Action<bool, T> callback = null) where T : NetMessage
    {
        if (!CheckMinTime(url))
        {
            Debug.LogError("Send too fast = " + url);
            return false;
        }
        Debug.Log("<<=send=" + url + "==>>");
        HTTPRequest req = MakeGetRequest<T>(url, callback);

        MarkCheck(url);
        return HTTPManager.SendRequest(req) != null;
    }


}
