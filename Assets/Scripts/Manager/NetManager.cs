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

    private long uid
    {
        get
        {
            return GameMainManager.instance.model.userData.uid;
        }
    }

    private long time
    {
        get
        {
            return GameUtils.DateTimeToTimestamp(DateTime.Now);
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
                EventDispatcher.instance.DispatchEvent(EventEnum.LOGIN_COMPLATE, res.data);

            }
            else
            {
                Debug.Log("登录失败:" + res.errmsg);
            }
            callBack(ret, res);
        });

    }

    public bool Roll(Action<bool, RollMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/roller/roll");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<RollMessage>(url, data, (ret, res) => {
            //---盾牌---
            //string jiashuju = "{\"data\":{\"money\":764158,\"maxEnergy\":50,\"energy\":30,\"recoverEnergy\":6,\"timeToRecover\":3600,\"shields\":3,\"stealIslands\":null,\"attackTarget\":{\"uid\":232,\"name\":\"原来如此\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTIUKt2UaM9cibOM5RsVDyibbbr1tUHia1jR1eibsjGgmXm2BBAFQbosuBPx4sX4hY50j0Jzhu3y4hx2rQ/0\",\"crowns\":66,\"gender\":1,\"isVip\":false,\"signature\":\"\",\"islandId\":3,\"buildings\":[{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":5,\"status\":2,\"isShield\":true},{\"level\":1,\"status\":1,\"isShield\":true},{\"level\":1,\"status\":0,\"isShield\":true}],\"head_frame\":0},\"FriendOfFriends\":null,\"rollerItem\":{\"index\":6,\"type\":\"shield\",\"value\":65,\"code\":1,\"name\":\"攻击\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
            //---能量---
            //string jiashuju = "{\"data\":{\"money\":764158,\"maxEnergy\":50,\"energy\":30,\"recoverEnergy\":6,\"timeToRecover\":3600,\"shields\":3,\"stealIslands\":null,\"attackTarget\":{\"uid\":232,\"name\":\"原来如此\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTIUKt2UaM9cibOM5RsVDyibbbr1tUHia1jR1eibsjGgmXm2BBAFQbosuBPx4sX4hY50j0Jzhu3y4hx2rQ/0\",\"crowns\":66,\"gender\":1,\"isVip\":false,\"signature\":\"\",\"islandId\":3,\"buildings\":[{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":5,\"status\":2,\"isShield\":true},{\"level\":1,\"status\":1,\"isShield\":true},{\"level\":1,\"status\":0,\"isShield\":true}],\"head_frame\":0},\"FriendOfFriends\":null,\"rollerItem\":{\"index\":3,\"type\":\"energy\",\"value\":65,\"code\":1,\"name\":\"攻击\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
            //---攻击---
            //string jiashuju = "{\"data\":{\"money\":764158,\"maxEnergy\":50,\"energy\":30,\"recoverEnergy\":6,\"timeToRecover\":3600,\"shields\":3,\"stealIslands\":null,\"attackTarget\":{\"uid\":232,\"name\":\"原来如此\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTIUKt2UaM9cibOM5RsVDyibbbr1tUHia1jR1eibsjGgmXm2BBAFQbosuBPx4sX4hY50j0Jzhu3y4hx2rQ/0\",\"crowns\":66,\"gender\":1,\"isVip\":false,\"signature\":\"\",\"islandId\":3,\"buildings\":[{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":5,\"status\":2,\"isShield\":true},{\"level\":1,\"status\":1,\"isShield\":true},{\"level\":1,\"status\":0,\"isShield\":true}],\"head_frame\":0},\"FriendOfFriends\":null,\"rollerItem\":{\"index\":8,\"type\":\"shoot\",\"value\":65,\"code\":1,\"name\":\"攻击\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
            //--偷窃--
            //string jiashuju = "{\"data\":{\"money\":2710788,\"maxEnergy\":50,\"energy\":49230,\"recoverEnergy\":6,\"timeToRecover\":3600,\"shields\":3,\"stealIslands\":[{\"islandId\":3,\"buildings\":[{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":2,\"isShield\":false},{\"level\":1,\"status\":2,\"isShield\":false},{\"level\":5,\"status\":2,\"isShield\":false}],\"crowns\":0},{\"islandId\":2,\"buildings\":[{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true}],\"crowns\":0},{\"islandId\":1,\"buildings\":[{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":2,\"status\":0,\"isShield\":false}],\"crowns\":0}],\"attackTarget\":null,\"FriendOfFriends\":null,\"rollerItem\":{\"index\":1,\"type\":\"steal\",\"value\":40,\"code\":1,\"name\":\"偷取\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
            //res = LitJson.JsonMapper.ToObject<RollMessage>(jiashuju);
            //Debug.Log("假数据");
            callBack(ret, res);
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
                Debug.Log(user.timeToRecover);
            }
            else
            {
                Debug.Log("roll点失败:" + res.errmsg);
            }
           
        });
    }
    public bool Build(int islandID,int buildIndex, Action<bool, BuildMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/island/build");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        data.Add("island",islandID);
        data.Add("building", buildIndex);
        return HttpProxy.SendPostRequest<BuildMessage>(url, data, (ret, res) => {
            callBack(ret, res);
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
            }
            else
            {
                Debug.Log("建造失败:" + res.errmsg);
            }
            
        });
    }

    public bool Attack(long puid,int buildIndex, Action<bool, AttackMessage> callBack)
    {
       
        string url = MakeUrl(APIDomain, "/game/pvp/attack");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t",time.ToString());
        data.Add("puid", puid);
        data.Add("building", buildIndex);
        return HttpProxy.SendPostRequest<AttackMessage>(url, data, (ret, res) => {
            //string str = "{\"data\":{\"money\":1162834,\"isShielded\":false,\"isMiniShielded\":false,\"isWanted\":false,\"isBear\":false,\"isSeal\":false,\"reward\":300000,\"isNewCannonContestScoreAward\":false,\"betCount\":0,\"attackTarget\":{\"uid\":18983,\"name\":\"杨重武\",\"headImg\":\"s\",\"crowns\":2,\"signature\":\"\",\"islandId\":1,\"buildings\":[{\"level\":1,\"status\":1,\"isShield\":false},{\"level\":0,\"status\":2,\"isShield\":false},{\"level\":0,\"status\":0,\"isShield\":false},{\"level\":0,\"status\":0,\"isShield\":false},{\"level\":0,\"status\":0,\"isShield\":false}]},\"tutorial\":0},\"errcode\":0,\"errmsg\":\"\"}";
            //res = LitJson.JsonMapper.ToObject<AttackMessage>(str);
            //res.data.isShielded = true;
            //Debug.Log("假数据");
            callBack(ret, res);
            if (res.isOK)
            {
                AttackData attackData = res.data;
                GameMainManager.instance.model.userData.money = attackData.money;
                EventDispatcher.instance.DispatchEvent(EventEnum.UPDATE_USERDATA);
            }
            else
            {
                Debug.Log("攻击失败:" + res.errmsg);
            }
            
        });
    }

    public bool Steal(int idx, Action<bool, StealMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "/game/pvp/steal");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        data.Add("idx", idx);
        return HttpProxy.SendPostRequest<StealMessage>(url, data, (ret, res) => {

            //string str = "{\"data\":{\"money\":3044355903,\"betCount\":0,\"targets\":[{\"uid\":225,\"gender\":1,\"name\":\"Lucas\",\"headImg\":\"http://wx.qlogo.cn/mmopen/yAICdTftjSmibHa8rytjyFK89EqHCictUKr9JCjyBTBxbwArefHuxR1paOQdbCVf5TbPhSmyasEfep9rbSRiajImQ/0\",\"crowns\":281,\"isVip\":true,\"head_frame\":1,\"money\":145089,\"isRichMan\":false},{\"uid\":83,\"gender\":2,\"name\":\"☒\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTLVSicU6zXExkDmURhjkchiaGiacPgicQxibh1kJJwjbjicH0Bf6vibISNFJ8zLicZqKbSeWUia4uqYic5ibibMLQ/0\",\"crowns\":2,\"isVip\":true,\"head_frame\":1,\"money\":748384,\"isRichMan\":true},{\"uid\":88,\"gender\":2,\"name\":\"啊，一只小怪\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTInIuedI6ibt6Dib4wSShf7nCvoM5oRmuy8ZE0ELC0CtvichicQHAqoEdvlfD7Mzia5eq9QGARedgMdSKQ/0\",\"crowns\":302,\"isVip\":false,\"head_frame\":0,\"money\":115413,\"isRichMan\":false}],\"stealTarget\":{\"uid\":224,\"gender\":1,\"name\":\"小野子\",\"headImg\":\"http://wx.qlogo.cn/mmopen/Q3auHgzwzM6ib6xP24BmM6icGJpwPic1e5DibCSOUSic1Wliad9pNr1EW2zl4CNELL8sxDoYfq4bM0niaEsnia538icAUWw/0\",\"crowns\":336,\"isVip\":false,\"head_frame\":0,\"money\":709412,\"isRichMan\":true},\"badLuck\":false,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
            //res = LitJson.JsonMapper.ToObject<StealMessage>(str);
            callBack(ret, res);
            if (res.isOK)
            {
                StealData stealData = res.data;
                GameMainManager.instance.model.userData.money = stealData.money;
                if(stealData.stealTarget != null)
                {
                    GameMainManager.instance.model.userData.stealTarget = stealData.stealTarget;
                }
               
                EventDispatcher.instance.DispatchEvent(EventEnum.UPDATE_USERDATA);
            }
            else
            {
                Debug.Log("偷取失败:" + res.errmsg);
            }
            
        });
    }

    public bool Enemy(Action<bool, BadGuyMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "/game/rank/enemy");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<BadGuyMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("获取恶人失败:" + res.errmsg);
            }
            
        });
    }

    public bool Vengeance(Action<bool, VengeanceMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "/game/rank/vengeance");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<VengeanceMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("获取仇人失败:" + res.errmsg);
            }
           
        });
    }
}
