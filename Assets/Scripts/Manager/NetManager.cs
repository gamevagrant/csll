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
    private string _token;
    private string token
    {
        get
        {
            return _token;
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

    private void ConnectWebSocket(long uid)
    {
        WebSocketProxy wsp;
        //wsp = new WebSocketProxy(string.Format("ws://10.0.8.50:8080/ws/conn?uid={0}",uid.ToString()));
        wsp = new WebSocketProxy(string.Format(GameMainManager.instance.model.userData.broadcast, uid.ToString()));
        wsp.onOpen += () => {
            //wsp.send("{\"cmd\":\"info\",\"fromid\":\"1\",\"toid\":\"2\",\"content\":\"hello?\"}");
            Debug.Log("onOpen");
        };
        wsp.onMessage += (str) => 
        {
            Debug.Log("onMessage" + str);
            MessageResponseData msg = LitJson.JsonMapper.ToObject<MessageResponseData>(str);
            GameMainManager.instance.websocketMsgManager.SendMsg(msg);

            //Debug.Log("假数据");
            //string json = "{\"uid\":101,\"toid\":2,\"action\":1,\"result\":0,\"time\":\"2006-01-0215:04:05\",\"name\":\"Badlwin\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTIUKt2UaM9cibOM5RsVDyibbbr1tUHia1jR1eibsjGgmXm2BBAFQbosuBPx4sX4hY50j0Jzhu3y4hx2rQ/0\",\"Crowns\":100,\"extra\":{\"crows\":138,\"building\":{\"isShield\":false,\"level\":1,\"status\":1},\"building_index\":1,\"isShielded\":false},\"read\":false,\"isWanted\":false,\"isVip\":false}";
            //MessageResponseData m = LitJson.JsonMapper.ToObject<MessageResponseData>(json);
            //GameMainManager.instance.websocketMsgManager.SendMsg(m);
            //GameMainManager.instance.websocketMsgManager.SendMsg(m);
            //GameMainManager.instance.websocketMsgManager.SendMsg(m);
        };
        wsp.onError += (str) => 
        {
            Debug.Log("onError" + str);
        };

        
    }

    public bool AddGuest(Action<bool, AddGuesMessage> callBack)
    {
        string url = MakeUrl("http://h5.dodgame.com.cn", "api/add_guest");
        Debug.Log(url);
        Dictionary<string, object> data = new Dictionary<string, object>();
        return HttpProxy.SendPostRequest<AddGuesMessage>(url, data, (ret, res) => {
            if (res.result == "ok")
            {

                Debug.Log("注册游客："+res.UserInfo.OpenId);
                
            }

            callBack(ret, res);
        });
       
    }



    public bool Login(string openid, Action<bool,LoginMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/basic/login");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("userId", openid);
        return HttpProxy.SendPostRequest<LoginMessage>(url,data, (ret, res) => {
            if (res.isOK)
            {
                res.data.timeTag = Time.time;
                GameMainManager.instance.model.userData = res.data;

                ConnectWebSocket(res.data.uid);
            }
            else
            {
                Debug.Log("登录失败:" + res.errmsg);
            }
            callBack(ret, res);
        });

    }


    public bool TutorialComplete(Action<bool,NetMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/reward/tutorialcomplete");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<NetMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("TutorialComplete失败:" + res.errmsg);
            }

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
            //string jiashuju = "{\"data\":{\"money\":764158,\"maxEnergy\":50,\"energy\":30,\"recoverEnergy\":6,\"timeToRecover\":3600,\"shields\":1,\"stealIslands\":null,\"attackTarget\":{\"uid\":232,\"name\":\"原来如此\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTIUKt2UaM9cibOM5RsVDyibbbr1tUHia1jR1eibsjGgmXm2BBAFQbosuBPx4sX4hY50j0Jzhu3y4hx2rQ/0\",\"crowns\":66,\"gender\":1,\"isVip\":false,\"signature\":\"\",\"islandId\":3,\"buildings\":[{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":5,\"status\":2,\"isShield\":true},{\"level\":1,\"status\":1,\"isShield\":true},{\"level\":1,\"status\":0,\"isShield\":true}],\"head_frame\":0},\"FriendOfFriends\":null,\"rollerItem\":{\"index\":6,\"type\":\"shield\",\"value\":65,\"code\":1,\"name\":\"攻击\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
            //---能量---
            //string jiashuju = "{\"data\":{\"money\":764158,\"maxEnergy\":50,\"energy\":30,\"recoverEnergy\":6,\"timeToRecover\":3600,\"shields\":3,\"stealIslands\":null,\"attackTarget\":{\"uid\":232,\"name\":\"原来如此\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTIUKt2UaM9cibOM5RsVDyibbbr1tUHia1jR1eibsjGgmXm2BBAFQbosuBPx4sX4hY50j0Jzhu3y4hx2rQ/0\",\"crowns\":66,\"gender\":1,\"isVip\":false,\"signature\":\"\",\"islandId\":3,\"buildings\":[{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":5,\"status\":2,\"isShield\":true},{\"level\":1,\"status\":1,\"isShield\":true},{\"level\":1,\"status\":0,\"isShield\":true}],\"head_frame\":0},\"FriendOfFriends\":null,\"rollerItem\":{\"index\":3,\"type\":\"energy\",\"value\":65,\"code\":1,\"name\":\"攻击\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
            //---攻击---
            //string jiashuju = "{\"data\":{\"money\":764158,\"maxEnergy\":50,\"energy\":30,\"recoverEnergy\":6,\"timeToRecover\":3600,\"shields\":3,\"stealIslands\":null,\"attackTarget\":{\"uid\":232,\"name\":\"原来如此\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTIUKt2UaM9cibOM5RsVDyibbbr1tUHia1jR1eibsjGgmXm2BBAFQbosuBPx4sX4hY50j0Jzhu3y4hx2rQ/0\",\"crowns\":66,\"gender\":1,\"isVip\":false,\"signature\":\"\",\"islandId\":3,\"buildings\":[{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":5,\"status\":2,\"isShield\":true},{\"level\":1,\"status\":1,\"isShield\":true},{\"level\":1,\"status\":0,\"isShield\":true}],\"head_frame\":0},\"FriendOfFriends\":null,\"rollerItem\":{\"index\":8,\"type\":\"shoot\",\"value\":65,\"code\":1,\"name\":\"攻击\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
            //--偷窃--
            //string jiashuju = "{\"data\":{\"money\":2710788,\"maxEnergy\":50,\"energy\":49230,\"recoverEnergy\":6,\"timeToRecover\":3600,\"shields\":3,\"stealIslands\":[{\"islandId\":3,\"buildings\":[{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":2,\"isShield\":false},{\"level\":1,\"status\":2,\"isShield\":false},{\"level\":5,\"status\":2,\"isShield\":false}],\"crowns\":0},{\"islandId\":2,\"buildings\":[{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true}],\"crowns\":0},{\"islandId\":1,\"buildings\":[{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":0,\"isShield\":false},{\"level\":2,\"status\":0,\"isShield\":false}],\"crowns\":0}],\"attackTarget\":null,\"FriendOfFriends\":null,\"rollerItem\":{\"index\":1,\"type\":\"steal\",\"value\":40,\"code\":1,\"name\":\"偷取\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
            //--星星--
            //string jiashuju = "{\"data\":{\"money\":2891727,\"maxEnergy\":60,\"energy\":58,\"recoverEnergy\":8,\"timeToRecover\":3596,\"shields\":0,\"stealIslands\":null,\"attackTarget\":null,\"FriendOfFriends\":null,\"rollerItem\":{\"index\":0,\"type\":\"xcrowns\",\"value\":45,\"code\":400,\"name\":\"星星倍金钱\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
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
                user.timeTag = Time.time;
                user.shields = res.data.shields;
                user.stealIslands = res.data.stealIslands;
                user.timeToRecoverInterval = res.data.timeToRecoverInterval;

            }
            else
            {
                Debug.Log("roll点失败:" + res.errmsg);
            }
           
        });
    }
    public bool Build(int islandID,int buildIndex, Action<bool, BuildMessage> callBack)
    {
        //Debug.Log("假数据");
        //string str = "{\"data\":{\"money\":2732090199,\"maxEnergy\":50,\"energy\":48,\"recoverEnergy\":6,\"timeToRecover\":0,\"islandId\":4,\"buildings\":[{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true}],\"crowns\":75,\"buildingCost\":[[165000,423000,795000,970000,1580000],[205000,463000,805000,990000,1900000],[275000,518000,850000,1150000,2050000],[320000,560000,895000,1350000,2250000],[390000,690000,975000,1550000,2550000]],\"buildingRepairCost\":[[45000,175000,275000,350000,450000],[50000,225000,300000,375000,475000],[100000,250000,325000,400000,500000],[200000,275000,350000,425000,550000],[150000,300000,400000,450000,800000]],\"gainIslandReward\":false,\"canIslandShare\":false,\"playUpgradeAnimation\":true,\"upgradeEnergyAfterReward\":68,\"upgradeMoneyAfterReward\":2732590199,\"rollerItems\":[{\"index\":0,\"type\":\"xcrowns\",\"value\":65,\"code\":200,\"name\":\"星星倍金钱\"},{\"index\":1,\"type\":\"steal\",\"value\":40,\"code\":1,\"name\":\"偷取\"},{\"index\":2,\"type\":\"coin\",\"value\":200,\"code\":25000,\"name\":\"25k\"},{\"index\":3,\"type\":\"energy\",\"value\":5,\"code\":10,\"name\":\"能量\"},{\"index\":4,\"type\":\"coin\",\"value\":215,\"code\":5000,\"name\":\"5000\"},{\"index\":5,\"type\":\"coin\",\"value\":85,\"code\":50000,\"name\":\"50k\"},{\"index\":6,\"type\":\"shield\",\"value\":55,\"code\":1,\"name\":\"护盾\"},{\"index\":7,\"type\":\"coin\",\"value\":220,\"code\":15000,\"name\":\"15k\"},{\"index\":8,\"type\":\"shoot\",\"value\":65,\"code\":1,\"name\":\"攻击\"},{\"index\":9,\"type\":\"coin\",\"value\":50,\"code\":100000,\"name\":\"100k\"}],\"tutorial\":18,\"mapInfo\":{\"islandNames\":[\"北京\",\"呼和浩特\",\"沈阳\",\"天津\",\"黄山\",\"西安\",\"成都\",\"上海\",\"贵阳\",\"乐山\",\"武汉\",\"昆明\",\"香港\",\"台北\",\"三亚\",\"敬请期待\"],\"mines\":[{\"island\":1,\"costs\":[270000,360000,450000,540000,630000],\"produces\":[7200,9600,12000,14400,16800],\"miner\":0},{\"island\":2,\"costs\":[307500,397500,487500,577500,667500],\"produces\":[8200,10600,13000,15400,17800],\"miner\":0},{\"island\":3,\"costs\":[460000,580000,700000,820000,940000],\"produces\":[9200,11600,14000,16400,18800],\"miner\":0},{\"island\":4,\"costs\":[765000,945000,1125000,1305000,1485000],\"produces\":[10200,12600,15000,17400,19800],\"miner\":0}],\"moneyBox\":0,\"producePerSecond\":0,\"limit\":400000}},\"errcode\":0,\"errmsg\":\"\"}";
        //BuildMessage r = LitJson.JsonMapper.ToObject<BuildMessage>(str);
        //callBack(true, r);
        //return true;
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
                user.timeTag = Time.time;
                user.timeToRecover = res.data.timeToRecover;
                user.islandId = res.data.islandId;
                user.buildings = res.data.buildings;
                user.crowns = res.data.crowns;
                user.buildingCost = res.data.buildingCost;
                user.buildingRepairCost = res.data.buildingRepairCost;
                user.rollerItems = res.data.rollerItems==null? user.rollerItems: res.data.rollerItems;
                if(res.data.mapInfo!= null)
                {
                    user.mapInfo = res.data.mapInfo;
                }

            }
            else
            {
                Debug.Log("建造失败:" + res.errmsg);
            }
            
        });
    }

    public bool Attack(long puid,int buildIndex, Action<bool, AttackMessage> callBack)
    {
       
        string url = MakeUrl(APIDomain, "game/pvp/attack");
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
            }
            else
            {
                Debug.Log("攻击失败:" + res.errmsg);
            }
            
        });
    }

    public bool Steal(int idx, Action<bool, StealMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/pvp/steal");
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
               
            }
            else
            {
                Debug.Log("偷取失败:" + res.errmsg);
            }
            
        });
    }

    public bool Enemy(Action<bool, BadGuyMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/enemy");
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
        string url = MakeUrl(APIDomain, "game/rank/vengeance");
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


    public bool Show(long fid, Action<bool, ShowMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/island/show");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("fid", fid);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<ShowMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("获取玩家信息失败:" + res.errmsg);
            }

        });
    }

    public bool Friend(int needFoF, Action<bool, FriendMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/friend");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("needFoF", needFoF);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<FriendMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                GameMainManager.instance.model.userData.friendInfo = res.data.friends;
                GameMainManager.instance.model.userData.friendNotAgreeInfo = res.data.friendsNotAgree;
            }
            else
            {
                Debug.Log("获取好友列表数据:" + res.errmsg);
            }

        });
    }

    public bool AgreeAddFriend(long friendUid, Action<bool, AgreeAddFriendMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/agreeAddFriend");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("friendUid", friendUid);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<AgreeAddFriendMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                AgreeAddFriendData agreeAddFriendData = res.data;
                GameMainManager.instance.model.userData.friendInfo = agreeAddFriendData.friends;
                GameMainManager.instance.model.userData.friendNotAgreeInfo = agreeAddFriendData.friendsNotAgree;

                EventDispatcher.instance.DispatchEvent(new UpdateFriendsEvent(UpdateFriendsEvent.UpdateType.AgreeFriend));
            }
            else
            {
                Debug.Log("同意好友申请失败:" + res.errmsg);
            }

        });
    }

    public bool AgreeAddAllFriend( Action<bool, FriendMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/AddAllFriend");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<FriendMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                FriendsData friendData = res.data;
                GameMainManager.instance.model.userData.friendInfo = friendData.friends;
                GameMainManager.instance.model.userData.friendNotAgreeInfo = friendData.friendsNotAgree;
                EventDispatcher.instance.DispatchEvent(new UpdateFriendsEvent(UpdateFriendsEvent.UpdateType.AgreeFriend));
            }
            else
            {
                Debug.Log("同意所有添加好友失败:" + res.errmsg);
            }

        });
    }

    public bool AddFriend(string FriendshipCode, Action<bool, FriendMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/addFriend");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("FriendshipCode", FriendshipCode);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<FriendMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                FriendsData friendData = res.data;
                GameMainManager.instance.model.userData.friendInfo = friendData.friends;
                GameMainManager.instance.model.userData.friendNotAgreeInfo = friendData.friendsNotAgree;
            }
            else
            {
                Debug.Log("添加好友失败:" + res.errmsg);
            }

        });
    }



    public bool RemoveFriend(long friendUid, Action<bool, FriendMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/removeFriend");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("friendUid", friendUid);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<FriendMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                FriendsData friendData = res.data;
                GameMainManager.instance.model.userData.friendInfo = friendData.friends;
                GameMainManager.instance.model.userData.friendNotAgreeInfo = friendData.friendsNotAgree;
                EventDispatcher.instance.DispatchEvent(new UpdateFriendsEvent(UpdateFriendsEvent.UpdateType.IgnoreFriend ));
                EventDispatcher.instance.DispatchEvent(new UpdateFriendsEvent(UpdateFriendsEvent.UpdateType.RemoveFriend));
            }
            else
            {
                Debug.Log("忽略好友失败:" + res.errmsg);
            }

        });
    }

    public bool RemoveAllFriend(Action<bool, FriendMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/removeAllFriend");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<FriendMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                FriendsData friendData = res.data;
                GameMainManager.instance.model.userData.friendInfo = friendData.friends;
                GameMainManager.instance.model.userData.friendNotAgreeInfo = friendData.friendsNotAgree;
                EventDispatcher.instance.DispatchEvent(new UpdateFriendsEvent(UpdateFriendsEvent.UpdateType.IgnoreFriend));
            }
            else
            {
                Debug.Log("忽略所有好友失败:" + res.errmsg);
            }

        });
    }

    public bool SendEnergy(long friendUid, Action<bool, SendEnergyMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/send-to");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("friendUid", friendUid);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<SendEnergyMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                FriendData[] friends = res.data;
                for(int i = 0;i<GameMainManager.instance.model.userData.friendInfo.Length;i++)
                {
                    FriendData fd = GameMainManager.instance.model.userData.friendInfo[i];
                    if (fd.uid == res.data[0].uid)
                    {
                        GameMainManager.instance.model.userData.friendInfo[i] = friends[0];
                        break;
                    }
                }
                EventDispatcher.instance.DispatchEvent(new UpdateFriendsEvent(UpdateFriendsEvent.UpdateType.SendEnergy));
            }
            else
            {
                Debug.Log("赠送好友能量失败:" + res.errmsg);
            }

        });
    }

    public bool ReceiveEnergy(long friendUid, Action<bool, ReceiveEnergyMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/receive-from");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("friendUid", friendUid);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<ReceiveEnergyMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                ReceiveFromData receiveFromData = res.data;
                UserData ud = GameMainManager.instance.model.userData;
                ud.energy = receiveFromData.energy;
                ud.maxEnergy = receiveFromData.maxEnergy;
                ud.recoverEnergy = receiveFromData.recoverEnergy;
                ud.timeToRecover = receiveFromData.timeToRecover;
                ud.timeTag = Time.time;
                ud.dailyCount = receiveFromData.dailyCount;
                ud.dailyLimit = receiveFromData.dailyLimit;
                ud.friendInfo = receiveFromData.friends;

                for (int i = 0; i < GameMainManager.instance.model.userData.friendInfo.Length; i++)
                {
                    FriendData fd = GameMainManager.instance.model.userData.friendInfo[i];
                    if (fd.uid == receiveFromData.friends[0].uid)
                    {
                        GameMainManager.instance.model.userData.friendInfo[i] = receiveFromData.friends[0];
                        break;
                    }
                }

                EventDispatcher.instance.DispatchEvent(new UpdateFriendsEvent(UpdateFriendsEvent.UpdateType.ReceiveEnergy));
            }
            else
            {
                Debug.Log("接受好友能量失败:" + res.errmsg);
            }

        });
    }

    public bool ShopList(Action<bool, ShopMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/shop/list");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<ShopMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("接受好友能量失败:" + res.errmsg);
            }

        });
    }

    public bool AllRank(Action<bool, AllRankMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/all-rank");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<AllRankMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("获取世界排行榜失败:" + res.errmsg);
            }

        });
    }

    public bool FriendRank(Action<bool, SendEnergyMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/rank/friends-rank");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<SendEnergyMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("获取好友排行榜失败:" + res.errmsg);
            }

        });
    }

    public bool GetMap(Action<bool, GetMapMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/map/get-map");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<GetMapMessage>(url, data, (ret, res) => {
            callBack(ret, res);
            if (res.isOK)
            {
                GameMainManager.instance.model.userData.mapInfo = res.data.mapInfo;
            }
            else
            {
                Debug.Log("获取地图数据失败:" + res.errmsg);
            }

        });
    }

    public bool BuyMiner(int islanID,Action<bool, BuyMinerMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/map/buy-miner");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("island", islanID);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<BuyMinerMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                BuyMinerData buyMinerData = res.data;
                UserData user = GameMainManager.instance.model.userData;
                user.energy = buyMinerData.energy;
                user.money = buyMinerData.money;
                user.mapInfo = buyMinerData.mapInfo;

                EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.UPDATE_MAPINFO));
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money,0));
            }
            else
            {
                Debug.Log("购买矿工失败:" + res.errmsg);
            }

        });
    }

    public bool ReapMine(Action<bool, BuyMinerMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/map/reap-mine");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<BuyMinerMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                BuyMinerData buyMinerData = res.data;
                UserData user = GameMainManager.instance.model.userData;
                user.energy = buyMinerData.energy;
                user.money = buyMinerData.money;
                user.mapInfo = buyMinerData.mapInfo;
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money,0));
            }
            else
            {
                Debug.Log("获取采矿金币失败:" + res.errmsg);
            }

        });
    }

    public bool Wanted(long wid,Action<bool, WantedMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/pvp/wanted");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("wid", wid);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<WantedMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                GameMainManager.instance.model.userData.wantedCount = res.data.wantedCount;
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.wanted,0));
            }
            else
            {
                Debug.Log("通缉玩家失败:" + res.errmsg);
            }

        });
    }

    public bool Message(Action<bool, MessageMailMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/message");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<MessageMailMessage>(url, data, (ret, res) => {

            callBack(ret, res);
            if (res.isOK)
            {
                GameMainManager.instance.model.userData.messages = res.data.messages;
                GameMainManager.instance.model.userData.user_mail = res.data.user_mail;
            }
            else
            {
                Debug.Log("获取消息和邮件失败:" + res.errmsg);
            }

        });
    }

    public bool GetReward(int index,Action<bool, GetRewardMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/mail/getReward");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("idx", index);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<GetRewardMessage>(url, data, (ret, res) => {

            //string str = "{\"errcode\":0,\"errmsg\":\"\",\"data\":{\"user_mail\":[{\"type\":2,\"tittle\":\"VIP特权 每日能量10\",\"desc\":\"VIP特权 每日能量10\",\"reward\":[{\"type\":\"energy\",\"num\":10,\"name\":\"\"}],\"is_get\":2,\"time\":\"今天 09:43:48\"},{\"type\":2,\"tittle\":\"VIP特权 每日能量10\",\"desc\":\"VIP特权 每日能量10\",\"reward\":[{\"type\":\"energy\",\"num\":10,\"name\":\"\"}],\"is_get\":1,\"time\":\"昨天 2017-11-16 13:53:45\"},{\"type\":2,\"tittle\":\"VIP特权 每日能量10\",\"desc\":\"VIP特权 每日能量10\",\"reward\":[{\"type\":\"energy\",\"num\":10,\"name\":\"\"}],\"is_get\":1,\"time\":\"前天 2017-11-15 13:34:47\"},{\"type\":2,\"tittle\":\"VIP特权 每日能量10\",\"desc\":\"VIP特权 每日能量10\",\"reward\":[{\"type\":\"energy\",\"num\":10,\"name\":\"\"}],\"is_get\":2,\"time\":\"2017-11-14 15:15:20\"},{\"type\":2,\"tittle\":\"VIP特权 每日能量10\",\"desc\":\"VIP特权 每日能量10\",\"reward\":[{\"type\":\"energy\",\"num\":10,\"name\":\"\"}],\"is_get\":1,\"time\":\"2017-11-13 10:02:43\"}],\"user_rewards\":[{\"type\":\"energy\",\"count\":415,\"num\":10,\"name\":\"\"},{\"type\":\"money\",\"count\":415,\"num\":10,\"name\":\"\"}]}}";
            //res = LitJson.JsonMapper.ToObject<GetRewardMessage>(str);
            //Debug.Log("假数据");
            callBack(ret, res);
            if (res.isOK)
            {
                GameMainManager.instance.model.userData.user_mail = res.data.user_mail;
            }
            else
            {
                Debug.Log("获取消息和邮件失败:" + res.errmsg);
            }

        });
    }

    //-------------------------facebook接口------------------------------------
    /// <summary>
    /// 注册接口，不管是否注册过每册登录都调用 会返回用于登录的uid和token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="expirationTime"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    private bool Register(string accessToken, long expirationTime, Action<bool, RegisterMessage> callBack)
    {

        string url = MakeUrl(APIDomain, "game/basic/registerUnityFb");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("AccessToken", accessToken);
        data.Add("ExpirationTime", expirationTime);
        return HttpProxy.SendPostRequest<RegisterMessage>(url, data, (ret, res) => {
            if (res.isOK)
            {

                _token = res.data.token;
            }
            else
            {
                Debug.Log("注册失败:" + res.errmsg);
            }
            callBack(ret, res);
        });

    }

    public bool LoginFB(string accessToken, long expirationTime, Action<bool, LoginMessage> callBack)
    {
        return Register(accessToken, expirationTime, (ret, res) =>
        {
            if (res.isOK)
            {
                string openID = res.data.openid;
                string token = res.data.token;
                Login(openID, (rs, rt) =>
                {
                    callBack(rs, rt);
                });
            }

        });
    }


    public bool GetInviteProgress(Action<bool, InviteProgressMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/friend/getinvitetimesfb");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("uid", uid);
        data.Add("type", "get");
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<InviteProgressMessage>(url, data, (ret, res) => 
        {
            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("获取好友邀请进度失败:" + res.errmsg);
            }
            
        });
    }

    public bool SetInviteProgress(int limit, Action<bool, NetMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/friend/getinvitetimesfb");
        Dictionary<string, object> data = new Dictionary<string, object>(); 
        data.Add("limit", limit);
        data.Add("type", "set");
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<NetMessage>(url, data, (ret, res) =>
        {
            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("设置好友邀请进度失败:" + res.errmsg);
            }

        });
    }


    public bool GetRecallableFriends(Action<bool, RecallableFriendsMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/user/share");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("isShared", 0);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<RecallableFriendsMessage>(url, data, (ret, res) =>
        {
            callBack(ret, res);
            if (res.isOK)
            {

            }
            else
            {
                Debug.Log("获取可召回好友列表失败:" + res.errmsg);
            }

        });
    }

   public bool InviteFriends(string reqid,string[] to, Action<bool, InviteFriendsMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/friend/invitebyfb");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("reqid", reqid);
        data.Add("touids", LitJson.JsonMapper.ToJson(to));//json字符串 [xxxxx,xxxxx,xxxx]
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());
        return HttpProxy.SendPostRequest<InviteFriendsMessage>(url, data, (ret, res) =>
        {
            callBack(ret, res);
            if (res.isOK)
            {
                UserData ud = GameMainManager.instance.model.userData;
                ud.energy = res.data.energy;
                ud.money = res.data.money;
            }
            else
            {
                Debug.Log("邀请好友失败:" + res.errmsg);
            }

        });
    }

    public bool RecallFriends(int limit, string[] to, Action<bool, RecallFriendsMessage> callBack)
    {
        string url = MakeUrl(APIDomain, "game/user/recall");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("limit", limit);
        string touids = "";
        for(int i = 0;i<to.Length;i++)
        {
            if(i==to.Length-1)
            {
                touids += to[i];
            }else
            {
                touids += to[i] + ",";
            }
        }
        data.Add("touids", touids);
        data.Add("uid", uid);
        data.Add("token", token);
        data.Add("t", time.ToString());

        return HttpProxy.SendPostRequest<RecallFriendsMessage>(url, data, (ret, res) =>
        {
            callBack(ret, res);
            if (res.isOK)
            {
                UserData ud = GameMainManager.instance.model.userData;
                ud.energy = res.data.energy;
                ud.money = res.data.money;
            }
            else
            {
                Debug.Log("召回好友失败:" + res.errmsg);
            }

        });
    }
}
