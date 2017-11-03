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

    private void ConnectWebSocket(long uid)
    {
        WebSocketProxy wsp;
        wsp = new WebSocketProxy(string.Format("ws://10.0.8.50:8080/ws/conn?uid={0}",uid.ToString()));
        wsp.onOpen += () => {
            //wsp.send("{\"cmd\":\"info\",\"fromid\":\"1\",\"toid\":\"2\",\"content\":\"hello?\"}");

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

    public bool Login(long userid, Action<bool,LoginMessage> callBack)
    {
        //string jiashuju = "{\"data\":{\"uid\":232,\"name\":\"原来如此\",\"gender\":1,\"read_announcement\":0,\"daily_energy\":2,\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTIUKt2UaM9cibOM5RsVDyibbbr1tUHia1jR1eibsjGgmXm2BBAFQbosuBPx4sX4hY50j0Jzhu3y4hx2rQ/0\",\"isAuthed\":true,\"IsSubscribed\":true,\"showSubscribed\":true,\"platformId\":\"otWhJs9OEi5W79121ivexOD_8Qc4\",\"platform\":0,\"tutorial\":18,\"money\":900740016,\"maxEnergy\":50,\"energy\":62,\"recoverEnergy\":6,\"timeToRecover\":3600,\"timeToRecoverInterval\":3600,\"islandId\":4,\"buildings\":[{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true}],\"crowns\":75,\"shields\":3,\"maxBet\":1,\"rollerItems\":[{\"index\":0,\"type\":\"xcrowns\",\"value\":65,\"code\":200,\"name\":\"星星倍金钱\"},{\"index\":1,\"type\":\"steal\",\"value\":40,\"code\":1,\"name\":\"偷取\"},{\"index\":2,\"type\":\"coin\",\"value\":200,\"code\":25000,\"name\":\"25k\"},{\"index\":3,\"type\":\"energy\",\"value\":5,\"code\":10,\"name\":\"能量\"},{\"index\":4,\"type\":\"coin\",\"value\":215,\"code\":5000,\"name\":\"5000\"},{\"index\":5,\"type\":\"coin\",\"value\":85,\"code\":50000,\"name\":\"50k\"},{\"index\":6,\"type\":\"shield\",\"value\":55,\"code\":1,\"name\":\"护盾\"},{\"index\":7,\"type\":\"coin\",\"value\":220,\"code\":15000,\"name\":\"15k\"},{\"index\":8,\"type\":\"shoot\",\"value\":65,\"code\":1,\"name\":\"攻击\"},{\"index\":9,\"type\":\"coin\",\"value\":50,\"code\":100000,\"name\":\"100k\"}],\"stealTarget\":{\"uid\":227,\"gender\":0,\"name\":\"年近三旬的郭老汉\",\"headImg\":\"http://wx.qlogo.cn/mmopen/pHTuw6N2F89Q4MyiamyPXia5Ro0t4GlGV0JLmzRxnEdia8ticqnuhu6VAWLeJ2yB59qf2alllBibmb80rYp9zPhdP5qvX4iaabrB97/0\",\"crowns\":355,\"isVip\":false,\"head_frame\":0,\"money\":1317289486,\"isRichMan\":true},\"lastRollerType\":0,\"shareCount\":0,\"shareTime\":0,\"daily_prize_confs\":[{\"day\":1,\"type\":\"gold\",\"num\":100000,\"status\":0},{\"day\":2,\"type\":\"energy\",\"num\":15,\"status\":0},{\"day\":3,\"type\":\"gold\",\"num\":300000,\"status\":0},{\"day\":4,\"type\":\"energy\",\"num\":25,\"status\":0},{\"day\":5,\"type\":\"gold\",\"num\":500000,\"status\":0},{\"day\":6,\"type\":\"energy\",\"num\":30,\"status\":0},{\"day\":7,\"type\":\"energy\",\"num\":40,\"status\":0}],\"weekly_prize_confs\":[{\"day\":8,\"type\":\"energy\",\"num\":30,\"status\":0},{\"day\":15,\"type\":\"gold\",\"num\":800000,\"status\":0},{\"day\":22,\"type\":\"energy\",\"num\":30,\"status\":0},{\"day\":30,\"type\":\"wanted\",\"num\":1,\"status\":0}],\"dailyPrizeDay\":2,\"daily_prize_limit\":false,\"week_prize_day\":3,\"wantedCount\":0,\"ShipwreckCount\":0,\"cookieCount\":0,\"potionCount\":0,\"hatchCount\":0,\"hornCount\":1,\"miniShieldCount\":0,\"monthCardExpired\":0,\"gotNewbieGift\":false,\"gotOccasionalGift\":true,\"gotDailyShop\":true,\"allInOnePiece\":0,\"killTitanCannonBall\":0,\"summonStone\":0,\"puffer\":0,\"lolly\":0,\"guildMedal\":1,\"dailyEventInfo\":null,\"mapInfo\":{\"islandNames\":[\"北京\",\"呼和浩特\",\"沈阳\",\"天津\",\"黄山\",\"西安\",\"成都\",\"上海\",\"贵阳\",\"乐山\",\"武汉\",\"昆明\",\"香港\",\"台北\",\"三亚\",\"敬请期待\"],\"mines\":[{\"island\":1,\"costs\":[270000,360000,450000,540000,630000],\"produces\":[7200,9600,12000,14400,16800],\"miner\":0},{\"island\":2,\"costs\":[307500,397500,487500,577500,667500],\"produces\":[8200,10600,13000,15400,17800],\"miner\":4},{\"island\":3,\"costs\":[460000,580000,700000,820000,940000],\"produces\":[9200,11600,14000,16400,18800],\"miner\":3},{\"island\":4,\"costs\":[765000,945000,1125000,1305000,1485000],\"produces\":[10200,12600,15000,17400,19800],\"miner\":0}],\"moneyBox\":39331840564780045,\"producePerSecond\":778,\"limit\":400000},\"guild\":null,\"jigsawInfo\":{\"openTime\":0,\"closeTime\":1876317,\"series\":2,\"pieces\":[0,0,0,2,0,0,0,0,0],\"allInOnes\":null,\"bags\":[188,78,24,6,60,58,23,27,28,36],\"rewardCount\":2,\"rewardLimit\":10,\"rewardList\":null,\"donate_jigsaw_count_daily\":0,\"donate_jigsaw_count_limit_daily\":10,\"hasUnReceivedPiece\":false},\"cashInfo\":\"\",\"friendInfo\":null,\"friendNotAgreeInfo\":null,\"friendshipCode\":\"6g\",\"friendRewards\":\"\",\"friendRewardMaxLimit\":100,\"recallRewards\":\"\",\"recallRewardCount\":0,\"gainRecallReward\":\"\",\"buildingCost\":[[165000,423000,795000,970000,1580000],[205000,463000,805000,990000,1900000],[275000,518000,850000,1150000,2050000],[320000,560000,895000,1350000,2250000],[390000,690000,975000,1550000,2550000]],\"buildingRepairCost\":[[82500,211500,397500,485000,790000],[102500,231500,402500,495000,950000],[137500,259000,425000,575000,1025000],[160000,280000,447500,675000,1125000],[195000,345000,487500,775000,1275000]],\"stealIslands\":null,\"attackTarget\":{\"uid\":2017811,\"name\":\"头发以下皆瘫痪\",\"headImg\":\"http://www.caishenlaile.com/upload/tfyxjth.png\",\"crowns\":91,\"signature\":\"\",\"islandId\":4,\"buildings\":[{\"level\":5,\"status\":0,\"isShield\":false},{\"level\":3,\"status\":0,\"isShield\":false},{\"level\":2,\"status\":0,\"isShield\":false},{\"level\":5,\"status\":0,\"isShield\":false},{\"level\":1,\"status\":0,\"isShield\":false}]},\"attackTargetUser\":null,\"betCount\":0,\"islandCount\":44,\"isTutorialMiner\":false,\"gainIslandReward\":false,\"canIslandShare\":false,\"petName\":\"\",\"petSleepRemain\":0,\"petExpRemain\":0,\"loginRewardRemain\":0,\"speedGiftRemain\":\"\",\"gotSubscribedReward\":true,\"gotMonthCardReward\":true,\"forbiddenPush\":false,\"nightAllowPush\":false,\"broadcastOff\":false,\"worldChatOff\":false,\"musicOff\":false,\"signature\":\"\",\"secret\":\"15a2d6d03e83a75a\",\"broadcast\":\"ws://59.110.14.135:13334/ws\",\"broadcastChannel\":2,\"activityInfos\":null,\"isExistConnonContestAward\":false,\"inviteCode\":\"WAQX2A6\",\"messages\":[{\"uid\":106,\"toid\":0,\"action\":2,\"result\":1828775183,\"time\":\"今天 17:53:25\",\"name\":\"tonysu\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/DYAIOgq83eqCDzRGawr2Mqcnb6iaQJK7gTtF95ggXa5jDB2tkoC8O9nliaO6ZLgwEePSyUpnGfnE4tRC5vllFdZg/0\",\"crowns\":109,\"extra\":{\"money\":900740016,\"reward\":1828775183},\"read\":true,\"isWanted\":false,\"isVip\":false,\"head_frame\":0}],\"dailyCount\":0,\"dailyLimit\":100,\"ActivityNotices\":{\"jigsaw\":{\"text\":\"武财神提醒您：谨防交易诈骗，请不要相信任何售卖碎片、能量信息，不要转账或发红包给不认识的人\",\"send_time\":1504792318,\"start_time\":0,\"end_time\":1567229182,\"interval\":1567229182,\"count\":3,\"type\":\"jigsaw\",\"speed\":3,\"priority\":0},\"main\":{\"text\":\"文财神悄悄告诉你，关注微信公众账号【海狸游戏】即可领取一大堆免费能量哦！\",\"send_time\":1505182696,\"start_time\":0,\"end_time\":1567229182,\"interval\":10,\"count\":3,\"type\":\"main\",\"speed\":4,\"priority\":0}},\"newbie_attack_target\":{\"headImg\":\"http://www.caishenlaile.com/upload/shla.png\",\"name\":\"是狐狸啊~\"},\"daily_task\":{\"extra_reward\":{\"type\":0,\"status\":0,\"totalProgress\":5,\"progress\":1,\"reward\":{\"type\":\"energy\",\"num\":50,\"name\":\"\"},\"name\":\"额外奖励\",\"desc\":\"完成每日所有任务，奖励体力50\"},\"tasks\":[{\"type\":10,\"status\":1,\"totalProgress\":330000,\"progress\":330000,\"reward\":{\"type\":\"gold\",\"num\":50000,\"name\":\"\"},\"name\":\"一个小目标\",\"desc\":\"累计获得330K金币\"},{\"type\":9,\"status\":0,\"totalProgress\":15,\"progress\":0,\"reward\":{\"type\":\"gold\",\"num\":50000,\"name\":\"\"},\"name\":\"收集狂人\",\"desc\":\"获得碎片15块\"},{\"type\":8,\"status\":0,\"totalProgress\":1,\"progress\":0,\"reward\":{\"type\":\"energy\",\"num\":10,\"name\":\"\"},\"name\":\"神算子\",\"desc\":\"猜中大富豪1次\"},{\"type\":1,\"status\":0,\"totalProgress\":800000,\"progress\":0,\"reward\":{\"type\":\"gold\",\"num\":20000,\"name\":\"\"},\"name\":\"飞来之财\",\"desc\":\"偷取800K金币\"},{\"type\":4,\"status\":0,\"totalProgress\":11800000,\"progress\":3510000,\"reward\":{\"type\":\"gold\",\"num\":50000,\"name\":\"\"},\"name\":\"否极泰来\",\"desc\":\"累积消耗11.8M金币\"}]},\"announcement\":{\"click_img_to\":\"\",\"img_url\":\"http://www.jianguoyouxi.com/resource/assets/notice/notice_banner_001.png\",\"sections\":[{\"sub_title\":\"免费体力送送送\",\"content\":[{\"color\":1,\"text\":\"关注【海狸游戏】公众账号，赢能量大礼包，还能每日领取免费体力哦！\"}]}]},\"one_yuan_buying\":{\"timeliness\":172800,\"gift_bag\":[{\"type\":\"gold\",\"num\":400000},{\"type\":\"energy\",\"num\":30}],\"countdown\":0,\"price\":100,\"original_price\":3000,\"itemId\":303,\"buy_status\":0},\"event20171001active\":0,\"show_event20171001page\":0,\"isVip\":false,\"head_frame\":0,\"vip_days\":0,\"user_mail\":[{\"type\":3,\"tittle\":\"关注领取奖励\",\"desc\":\"关注领取奖励20能量\",\"reward\":[{\"type\":\"energy\",\"num\":20,\"name\":\"\"}],\"is_get\":1,\"time\":\"今天 14:46:35\"}],\"achieve_red_hot\":{\"total_dot\":8,\"achieve_dot\":[0,0,1,7,0,0],\"grade_dot\":0},\"start_achieve\":false,\"last_action\":0,\"dungeon_keys\":0,\"dungeon_info\":null,\"dungeon_reward\":0},\"errcode\":0,\"errmsg\":\"\"}";
        //LoginMessage r = LitJson.JsonMapper.ToObject<LoginMessage>(jiashuju);
        //Debug.Log("假数据");
        //callBack(true, r);
        //GameMainManager.instance.model.userData = r.data;
        //return true;

        string url = MakeUrl(APIDomain, "game/basic/login");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("userId", userid);
        return HttpProxy.SendPostRequest<LoginMessage>(url,data, (ret, res) => {
            if (res.isOK)
            {
               
                GameMainManager.instance.model.userData = res.data;
                EventDispatcher.instance.DispatchEvent(EventEnum.LOGIN_COMPLATE, res.data);
                ConnectWebSocket(res.data.uid);
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
            //string jiashuju = "{\"data\":{\"money\":764158,\"maxEnergy\":50,\"energy\":30,\"recoverEnergy\":6,\"timeToRecover\":3600,\"shields\":1,\"stealIslands\":null,\"attackTarget\":{\"uid\":232,\"name\":\"原来如此\",\"headImg\":\"http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTIUKt2UaM9cibOM5RsVDyibbbr1tUHia1jR1eibsjGgmXm2BBAFQbosuBPx4sX4hY50j0Jzhu3y4hx2rQ/0\",\"crowns\":66,\"gender\":1,\"isVip\":false,\"signature\":\"\",\"islandId\":3,\"buildings\":[{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":0,\"status\":0,\"isShield\":true},{\"level\":5,\"status\":2,\"isShield\":true},{\"level\":1,\"status\":1,\"isShield\":true},{\"level\":1,\"status\":0,\"isShield\":true}],\"head_frame\":0},\"FriendOfFriends\":null,\"rollerItem\":{\"index\":6,\"type\":\"shield\",\"value\":65,\"code\":1,\"name\":\"攻击\"},\"timeToRecoverInterval\":3600,\"tutorial\":18},\"errcode\":0,\"errmsg\":\"\"}";
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
                //EventDispatcher.instance.DispatchEvent(EventEnum.UPDATE_USERDATA, user);
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
