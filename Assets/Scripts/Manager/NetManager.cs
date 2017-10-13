using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetManager
{
    private static NetManager _instance;

    public static NetManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new NetManager();
            }
            return _instance;
        }
    }


    private string APIDomain
    {
        get
        {
            if (GameSetting.isDebug)
                return GameSetting.serverPathTest;
            return GameSetting.serverPath;
        }
    }
    private string token
    {
        get
        {
            return GameSetting.token;
        }
    }

    private string MakeUrl(string domain, string api)
    {
        string str = string.Format("{0}/{1}", domain, api);
        return str;
    }


    public bool login(string userid, Action<bool,LoginMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/basic/login");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("userId", userid);
        return HttpProxy.SendPostRequest<LoginMessage>(url,data, callBack);

    }

    public bool roll(string uid, Action<bool, RollMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/roller/roll");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", GameUtils.ConvertDateTimeToInt(DateTime.Now).ToString());
        return HttpProxy.SendPostRequest<RollMessage>(url, data, callBack);
    }
}
