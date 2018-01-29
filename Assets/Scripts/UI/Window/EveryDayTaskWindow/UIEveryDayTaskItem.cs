using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEveryDayTaskItem : BaseItemView {

    [SerializeField]
    private Image icon;
    [SerializeField]
    private Selectable btn;
    [SerializeField]
    private TextMeshProUGUI taskName;
    [SerializeField]
    private TextMeshProUGUI taskDescribe;
    [SerializeField]
    private TextMeshProUGUI propess;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Sprite[] iconSprites;
    [SerializeField]
    private Sprite[] btnSprites;

    private TaskItemData data;
    // Use this for initialization
    void Start () {
		
	}

    public override void SetData(object data)
    {
        this.data = data as TaskItemData;
        int iconIndex = this.data.reward.type == "gold" ? 0 : 1;
        icon.sprite = iconSprites[iconIndex];
        if(this.data.status==2)
        {
            btn.interactable = false;
        }
        else
        {
            btn.interactable = true;
            btn.image.sprite = btnSprites[this.data.status];
        }
       
        taskName.text = this.data.name;
        taskDescribe.text = this.data.desc;
        propess.text = GameUtils.GetShortMoneyStr(this.data.progress) + "/" + GameUtils.GetShortMoneyStr(this.data.totalProgress);
        slider.value = (float)this.data.progress / this.data.totalProgress;
    }

    public void OnClickBtn()
    {
        if(data.status == 1)
        {
            GetReward();
        }
        else
        {
            
            switch (data.type)
            {
                case 1:
                case 2:
                case 8:
                case 9:
                case 10:
                    //跳转到转盘
                    GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow, 0);
                    break;
                case 3:
                    //跳转到好友界面，没有好友的跳转到邀请好友界面
                    if (GameMainManager.instance.model.userData.friendInfo!=null && GameMainManager.instance.model.userData.friendInfo.Length>0)
                    {
                        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFriendsWindow);
                    }
                    else
                    {
                        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIInviteWindow);
                    }
                   
                    break;
                case 4:
                case 5:
                    //跳转到建筑界面
                    GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow, 1);
                    break;
                case 6:
                    //召回
                    GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIInviteWindow,1);
                    break;
                case 7:
                    //前往邀请好友界面
                    GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIInviteWindow);
                    break;
            }

            GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UIEveryDayTaskWindow);
        }
    }

    private void GetReward()
    {
        if(this.data.reward.type == "energy" && GameMainManager.instance.model.userData.energy>= GameMainManager.instance.model.userData.maxEnergy)
        {
            Alert.Show("能量已满，用掉一些再来领取吧!");
            return;
        }
        GameMainManager.instance.netManager.GetDailyTaskReward(data.type, (ret, res) =>
        {
            if (res.isOK)
            {
                GetRewardWindowData rewardData = new GetRewardWindowData();
                rewardData.reward = this.data.reward;
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, rewardData);
                foreach (TaskItemData item in res.data.daily_task.tasks)
                {
                    if (item.type == data.type)
                    {
                        data.status = item.status;
                        SetData(data);
                        break;
                    }
                }

            }
        });
    }
}
