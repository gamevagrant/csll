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
            str = string.Format("你成功防御了{0}的攻击", msg.name);
        }else
        {
            if((int)msg.extra["building"]["status"] == 2)
            {
                str = string.Format("{0}损坏了你的{1}", msg.name, GameEnumeConfig.GetBuildingName((int)msg.extra["building_index"]));
            }
            else
            {
                str = string.Format("{0}摧毁了你的{1}", msg.name, GameEnumeConfig.GetBuildingName((int)msg.extra["building_index"]));
            }
        }
        PopupMessageData data = new PopupMessageData();
        data.headImg = msg.headImg;
        data.content = str;
        data.confirmHandle = () => {


        };

        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIPopupMessageWindow, data);
    }

    private void StealAction(MessageResponseData msg)
    {
        string str = "";
        str = string.Format("{0}偷走了{1}金币", msg.name,msg.extra["reward"]);
        PopupMessageData data = new PopupMessageData();
        data.headImg = msg.headImg;
        data.content = str;
        GameMainManager.instance.model.userData.money = (int)msg.extra["money"];

        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIPopupMessageWindow, data);
    }

    private void AddFriendAction(MessageResponseData msg)
    {

    }

    private void PayAction(MessageResponseData msg)
    {

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

