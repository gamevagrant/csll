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
                DailyTaskAction(msg);
                break;
            case 12://获得邮件通知
                MailAction(msg);
                break;
            default:
                break;
        }



    }



    private void AttackAction(MessageResponseData msg)
    {
        string str = "";
        if((bool)msg.extra["isShielded"])
        {
            str = string.Format("你成功防御了<#1995BCFF>{0}</color>的攻击", msg.name);
        }else
        {
            int buildingIndex = (int)msg.extra["building_index"];
            int buildingStatus = (int)msg.extra["building"]["status"];
            if (buildingStatus == 2)
            {
                str = string.Format("<#1995BCFF>{0}</color>损坏了你的{1}", msg.name, GameMainManager.instance.configManager.islandConfig.GetBuildingName(buildingIndex));
            }
            else
            {
                str = string.Format("<#1995BCFF>{0}</color>摧毁了你的{1}", msg.name, GameMainManager.instance.configManager.islandConfig.GetBuildingName(buildingIndex));
            }
            GameMainManager.instance.model.userData.buildings[buildingIndex].status = buildingStatus;
        }
        PopupMessageData data = new PopupMessageData();
        data.headImg = msg.headImg;
        data.content = str;
        data.confirmHandle = () => {

            EventDispatcher.instance.DispatchEvent(new UpdateBuildingEvent());
        };

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
        long reward = (long)msg.extra["reward"];
        long money = (long)msg.extra["money"];

        string str = string.Format("{0}偷走了{1}金币", msg.name,GameUtils.GetCurrencyString(reward));
        PopupMessageData data = new PopupMessageData();
        data.headImg = msg.headImg;
        data.content = str;
        GameMainManager.instance.model.userData.money = money;

        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIPopupMessageWindow, data);
    }

    private void AddFriendAction(MessageResponseData msg)
    {

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

    }

    private void DailyTaskAction(MessageResponseData msg)
    {

    }

    private void MailAction(MessageResponseData msg)
    {

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

