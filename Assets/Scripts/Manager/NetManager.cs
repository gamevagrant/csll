using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetManager:INetManager
{



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


    public bool Login(long userid, Action<bool,LoginMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/basic/login");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("userId", userid);
        return HttpProxy.SendPostRequest<LoginMessage>(url,data, (ret, res) => {
            if (res.isOK)
            {
                GameMainManager.instance.model.userData = res.data;
                EventDispatcher.instance.DispatchEvent(EventEnum.UPDATE_USERDATA, res.data);
                //UIManager.Instance.openWindow(UISettings.UIWindowID.UIWheel);
            }
            else
            {
                Debug.Log("登录失败:" + res.errmsg);
            }
            callBack(ret, res);
        });

    }

    public bool Roll(long uid, Action<bool, RollMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/roller/roll");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", GameUtils.ConvertDateTimeToInt(DateTime.Now).ToString());
        return HttpProxy.SendPostRequest<RollMessage>(url, data, (ret, res) => {
            if (res.isOK)
            {
                UserData user = GameMainManager.instance.model.userData;
                user.money = res.data.money;
                user.maxEnergy = res.data.maxEnergy;
                user.energy = res.data.energy;
                user.recoverEnergy = res.data.recoverEnergy;
                user.timeToRecover = res.data.timeToRecover;
                user.shields = res.data.shields;
                user.stealIslands = res.data.stealIslands;
                user.timeToRecoverInterval = res.data.timeToRecoverInterval;
                EventDispatcher.instance.DispatchEvent(EventEnum.UPDATE_USERDATA, user);
                //UIManager.Instance.openWindow(UISettings.UIWindowID.UIWheel);
                
            }
            else
            {
                Debug.Log("roll点失败:" + res.errmsg);
            }
            callBack(ret, res);
        });
    }
    public bool Build(long uid,int islandID,int buildIndex, Action<bool, BuildMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/island/build");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", GameUtils.ConvertDateTimeToInt(DateTime.Now).ToString());
        data.Add("island",islandID);
        data.Add("building", buildIndex);
        return HttpProxy.SendPostRequest<BuildMessage>(url, data, (ret, res) => {
            if (res.isOK)
            {
                UserData user = GameMainManager.instance.model.userData;
                user.money = res.data.money;
                user.maxEnergy = res.data.maxEnergy;
                user.energy = res.data.energy;
                user.recoverEnergy = res.data.recoverEnergy;
                user.timeToRecover = res.data.timeToRecover;
                user.islandId = res.data.islandId;
                user.buildings = res.data.buildings;
                user.crowns = res.data.crowns;
                user.buildingCost = res.data.buildingCost;
                user.buildingRepairCost = res.data.buildingRepairCost;
                user.rollerItems = res.data.rollerItems==null? user.rollerItems: res.data.rollerItems;
       
                EventDispatcher.instance.DispatchEvent(EventEnum.UPDATE_USERDATA, user);
                //UIManager.Instance.openWindow(UISettings.UIWindowID.UIWheel);
            }
            else
            {
                Debug.Log("建造失败:" + res.errmsg);
            }
            callBack(ret, res);
        });
    }
}
