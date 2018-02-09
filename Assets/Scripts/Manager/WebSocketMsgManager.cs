using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSocketMsgManager :IWebSocketMsgManager{



    public void SendMsg(MessageResponseData msg)
    {

        switch (msg.action)
        {
            case 1://攻击
                AttackAction(msg);
                break;
            case 2://偷窃
                StealAction(msg);
                break;
            case 3://添加好友
                AddFriendAction(msg);
                break;
            case 4://支付
                PayAction(msg);
                break;
            case 7://获得碎片
                PieceAction(msg);
                break;
            case 10://系统通知
                NoticeAction(msg);
                break;
            case 11://完成每日任务
                //DailyTaskAction(msg);
                break;
            case 12://获得邮件通知
                MailAction(msg);
                break;
            case 18://收到副本邀请
                break;
            case 19://好友帮助抽到牌
                break;
            default:
                Debug.LogAssertion(msg.action.ToString()+" websocket的返回没实现");
                break;
        }



    }



    private void AttackAction(MessageResponseData msg)
    {
        PopupMessageData data = new PopupMessageData();
        data.headImg = msg.headImg;

        string str = "";
        if((bool)msg.extra["isShielded"])
        {
            str = string.Format("你成功防御了<#1995BCFF>{0}</color>的攻击", msg.name);
            GameMainManager.instance.model.userData.shields = Mathf.Max(0, GameMainManager.instance.model.userData.shields - 1);
            data.confirmHandle = () => {

                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.sheild, 0));
            };
           
        }
        else
        {
            int buildingIndex = int.Parse(msg.extra["building_index"].ToString());
            int buildingStatus = int.Parse(msg.extra["building"]["status"].ToString());
            int level = int.Parse(msg.extra["building"]["level"].ToString());
            if (buildingStatus == 2)
            {
                str = string.Format("<#1995BCFF>{0}</color>损坏了你的{1}", msg.name, GameMainManager.instance.configManager.islandConfig.GetBuildingName(buildingIndex));
            }
            else
            {
                str = string.Format("<#1995BCFF>{0}</color>摧毁了你的{1}", msg.name, GameMainManager.instance.configManager.islandConfig.GetBuildingName(buildingIndex));
            }
            GameMainManager.instance.model.userData.buildings[buildingIndex].status = buildingStatus;
            GameMainManager.instance.model.userData.buildings[buildingIndex].level = level;
            data.confirmHandle = () => {

                EventDispatcher.instance.DispatchEvent(new UpdateBuildingEvent());
            };
        }

        data.content = str;
       

        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIPopupMessageWindow, data);
    }

    /*
     * {
          "uid": 209, 
          "toid": 315, 
          "action": 2, 
          "result": 0, 
          "time": "今天 17:53:47", 
          "name": "岳松", 
          "headImg": "https://fb-s-c-a.akamaihd.net/h-ak-fbx/v/t1.0-1/p50x50/17799337_1013087895489876_7747316675865790869_n.jpg?oh=1f7e2a1c9b826d7cfdc5f3f51209ea55&oe=5AC84270&__gda__=1521541416_9c07b70aa926a7a1ff274119a0a13fbc", 
          "crowns": 75, 
          "extra": 
              {
                "money": 318144985, 
                "reward": 83039102
              }, 
          "read": false, 
          "isWanted": false, 
          "isVip": false, 
          "head_frame": 0
        }
 */
    private void StealAction(MessageResponseData msg)
    {
        long reward = long.Parse(msg.extra["reward"].ToString());
        long money = long.Parse(msg.extra["money"].ToString());

        string str = string.Format("<#1995BCFF>{0}</color>偷走了你<#1995BCFF>{1}</color>金币", msg.name,GameUtils.GetCurrencyString(reward));
        PopupMessageData data = new PopupMessageData();
        data.headImg = msg.headImg;
        data.content = str;
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIPopupMessageWindow, data);

        GameMainManager.instance.model.userData.money = money;
        EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money, 1));
    }
    /*
     * {
    "uid": 10411,
    "toid": 1125,
    "action": 3,
    "result": 0,
    "time": "今天 18:15:14",
    "name": "Linda",
    "headImg": "https://scontent.xx.fbcdn.net/v/t1.0-1/c15.0.50.50/p50x50/1379841_10150004552801901_469209496895221757_n.jpg?oh=ef2ea8eb8c792b56ff67f460f47f79dd&oe=5ADEBA33",
    "crowns": 6,
    "extra": {
        "friend": {
            "uid": 10411,
            "crowns": 6,
            "crownsUpdateTime": 0,
            "friendName": "Linda",
            "gender": 2,
            "headImg": "https://scontent.xx.fbcdn.net/v/t1.0-1/c15.0.50.50/p50x50/1379841_10150004552801901_469209496895221757_n.jpg?oh=ef2ea8eb8c792b56ff67f460f47f79dd&oe=5ADEBA33",
            "isEmpty": false,
            "isVip": false,
            "name": "Linda",
            "openId": "",
            "rank": 0,
            "recallCount": 0,
            "signature": "",
            "updateTime": "",
            "guild": null,
            "status": 0,
            "sendStatus": 0,
            "receiveStatus": 0,
            "buildings": [
                {
                    "level": 2,
                    "status": 0,
                    "isShield": true
                },
                {
                    "level": 1,
                    "status": 0,
                    "isShield": true
                },
                {
                    "level": 0,
                    "status": 0,
                    "isShield": true
                },
                {
                    "level": 0,
                    "status": 0,
                    "isShield": true
                },
                {
                    "level": 3,
                    "status": 0,
                    "isShield": true
                }
            ],
            "islandId": 1,
            "head_frame": 0
        }
    },
    "read": false,
    "isWanted": false,
    "isVip": false,
    "head_frame": 0
}
     * */
    private void AddFriendAction(MessageResponseData msg)
    {
        GameMainManager.instance.netManager.Friend(0, (ret,res) => {

            GameMainManager.instance.model.userData.friendInfo = res.data.friends;
            GameMainManager.instance.model.userData.friendNotAgreeInfo = res.data.friendsNotAgree;
            EventDispatcher.instance.DispatchEvent(new UpdateFriendsEvent(UpdateFriendsEvent.UpdateType.AgreeFriend));

        });
        
    }

    private void PayAction(MessageResponseData msg)
    {
        //{"itemId": Item_id, "goods": [{"type": itemType, "count": count_extra, "num": count}]}
        //itemType: energy, money, props
        //count: 获得物品后的 物品总量
        //num：购买单件商品面值
        Debug.Log("购买发放物品："+ LitJson.JsonMapper.ToJson(msg.extra));
        RewardData[] rewards = LitJson.JsonMapper.ToObject<RewardData[]>(LitJson.JsonMapper.ToJson(msg.extra["goods"]));
        for (int i = 0;i< rewards.Length;i++)
        {
            RewardData rewardData = rewards[i];
            switch (rewardData.type)
            {
                case "energy":
                    GameMainManager.instance.model.userData.energy = (int)rewardData.count;
                    EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Energy, 1));
                    break;
                case "money":
                    GameMainManager.instance.model.userData.money = rewardData.count;
                    EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money, 1));
                    break;
                case "props"://通缉令
                    GameMainManager.instance.model.userData.wantedCount = (int)rewardData.count;
                    EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.wanted, 1));
                    break;
                case "vip":
                    GameMainManager.instance.model.userData.isVip = true;
                    GameMainManager.instance.model.userData.vip_days = (int)rewardData.count;
                    GameMainManager.instance.model.userData.maxEnergy = 60;
                    GameMainManager.instance.model.userData.recoverEnergy = 8;
                    EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.vip, 1));
                    break;
            }

            GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UIShopWindow);

            GetRewardWindowData getRewardWindowData = new GetRewardWindowData();
            getRewardWindowData.reward = rewardData;
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, getRewardWindowData);
            
        }
       

    }

    private void PieceAction(MessageResponseData msg)
    {

    }

    private void NoticeAction(MessageResponseData msg)
    {
        Debug.LogAssertion("公告wetsocket的返回没实现/n" + LitJson.JsonMapper.ToJson(msg));
    }
    /*
 {
  "uid": 0, 
  "toid": 1125, 
  "action": 11, 
  "result": 0, 
  "time": "", 
  "name": "", 
  "headImg": "", 
  "crowns": 0, 
  "extra": {
    "task": {
      "type": 10, 
      "status": 1, 
      "totalProgress": 800000, 
      "progress": 1011874, 
      "reward": {
        "type": "gold", 
        "num": 50000, 
        "name": ""
      }, 
      "name": "一个小目标", 
      "desc": "累计获得800K金币"
    }
  }, 
  "read": false, 
  "isWanted": false, 
  "isVip": false, 
  "head_frame": 0
} 
     */
    private void DailyTaskAction(MessageResponseData msg)
    {
       
        string str = string.Format("您已经完成了每日任务【{0}】快去领奖吧！",msg.extra["task"]["name"].ToString());
        PopupMessageData data = new PopupMessageData();
        data.headImg = msg.headImg;
        data.content = str;

        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIPopupMessageWindow, data);

        
    }

    private void MailAction(MessageResponseData msg)
    {
        Debug.LogAssertion("邮件的wetsocket的返回没实现/n" + LitJson.JsonMapper.ToJson(msg));
    }



}
/*
public class WebSocketMessage
{
    public long uid;
    public long toid;
    public int action;
    public int result;
    public string time;
    public string name;
    public string headImg;
    public int crowns;
    public LitJson.JsonData extra;
    public bool read;
    public bool isWanted;
    public bool isVip;

}
*/

