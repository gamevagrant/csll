using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDatasManager {

    /// <summary>
    /// 使用帐号登录过的用户数据
    /// </summary>
    public static SimpleUserData loggedAccount
    {
        get
        {
            return GetData<SimpleUserData>("LOGGED_ACCOUNT");
        }
        set
        {
            SaveData("LOGGED_ACCOUNT",value);
        }
    }
    /// <summary>
    /// 游客登录的帐号信息
    /// </summary>
    public static SimpleUserData loggedGuest
    {
        get
        {
            return GetData<SimpleUserData>("LOGGED_GUEST");
        }
        set
        {
            SaveData("LOGGED_GUEST", value);
        }
    }
    /// <summary>
    /// 邀请过的好友
    /// </summary>
    public static Dictionary<string,string> invitedFriends
    {
        get
        {
            return GetData<Dictionary<string, string>>("INVITED_FRIENDS");
        }
        set
        {
            SaveData("INVITED_FRIENDS", value);
        }
    }
    /// <summary>
    /// 召回过的好友
    /// </summary>
    public static Dictionary<string,string> callbackedFriends
    {
        get
        {
            return GetData<Dictionary<string, string>>("CALLBACKED_FRIENDS");
        }
        set
        {
            SaveData("CALLBACKED_FRIENDS", value);
        }
    }
    /// <summary>
    /// 是否展示过隐私提示
    /// </summary>
    public static bool isShowPrivacyTips
    {
        get
        {
            bool b = PlayerPrefs.GetInt("IS_SHOW_PRIVACY_TIPS", 0)==1?true:false;
            return b;
        }
        set
        {
            PlayerPrefs.SetInt("IS_SHOW_PRIVACY_TIPS", value ?1:0);
        }
    }


    private static T GetData<T>(string name)
    {
        string json = PlayerPrefs.GetString(name);
        T obj = LitJson.JsonMapper.ToObject<T>(json);
        return obj;
    }

    private static void SaveData(string name,System.Object obj)
    {
        string json = LitJson.JsonMapper.ToJson(obj);
        PlayerPrefs.SetString(name, json);
    }
}
