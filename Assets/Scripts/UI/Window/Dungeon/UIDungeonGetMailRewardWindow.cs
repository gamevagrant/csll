using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDungeonGetMailRewardWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIDungeonGetMailRewardWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public HeadIcon head;
    public TextMeshProUGUI content;
    public GameObjectPool pool;

    private MailData data;
    /*
     *{
        "type": 4,
        "tittle": "副本奖励",
        "desc": "蚌精掉落了奖励，快去看看！",
        "reward": [
          {
            "type": "energy",
            "num": 150,
            "name": ""
          },
          {
            "type": "money",
            "num": 5000000,
            "name": ""
          },
          {
            "type": "master_tile",
            "num": 2,
            "name": ""
          }
        ],
        "is_get": 1,
        "time": "今天 16:05:44",
        "extra": {
          "headImg": "https://scontent.xx.fbcdn.net/v/t1.0-1/c15.0.50.50/p50x50/1379841_10150004552801901_469209496895221757_n.jpg?oh=63882fb3fcd87e9433dcbb490913a150\u0026oe=5B064733",
          "name": "Margaret"
        },
        "btn_type": ""
      }, 
     **/
    protected override void StartShowWindow(object[] data)
    {
        this.data = data[0] as MailData;

        head.setData(string.Format("来自{0}的邮件", this.data.extra["name"].ToString()),
            this.data.extra["headImg"].ToString(),
            0,false);
        content.text = "Hi 我在副本中抽了你的牌，感谢你的贡献！这些是蚌精掉落的奖励，合作愉快！";
        pool.resetAllTarget();
        foreach (RewardData rd in this.data.reward)
        {
            pool.getIdleTarget<RewardItem>().SetData(rd);
        }
    }

    public void OnClickGetRewardBtn()
    {
        GameMainManager.instance.netManager.GetReward(data.index, (ret, res) =>
        {
            if (res.isOK)
            {
                EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.UPDATE_MAIL_DATA,res.data));
                OnClickClose();
            }
        });
    }
}
