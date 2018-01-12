using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIEveryDayRewardWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIEveryDayRewardWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    [SerializeField]
    private UIEveryDayRewardDailyItem[] dailyItems;
    [SerializeField]
    private UIEveryDayRewardWeekilyItem[] weeklyItems;
    [SerializeField]
    private Slider weeklySlider;
    [SerializeField]
    private QY.UI.Button getRewardBtn;
    [SerializeField]
    private Image getRewardBtnText;
    [SerializeField]
    private Sprite[] getRewardBtnTextSprites;

    protected override void StartShowWindow(object[] data)
    {
        UserData user = GameMainManager.instance.model.userData;
        SetData(user.daily_prize_confs,user.weekly_prize_confs,user.dailyPrizeDay,user.week_prize_day,user.daily_prize_limit);
    }

    private void SetData(DailyPrizeConfData[] dailyConfig, DailyPrizeConfData[]weeklyConfig,int dailyDay,int weeklyDay,bool getDailyRewardLimit)
    {
        getRewardBtn.interactable = !getDailyRewardLimit;
        getRewardBtnText.sprite = getRewardBtnTextSprites[getDailyRewardLimit?1:0];
        getRewardBtnText.SetNativeSize();
        weeklySlider.value = weeklyDay / 30f;

        for (int i =0;i<dailyConfig.Length;i++)
        {
            DailyPrizeConfData item = dailyConfig[i];
            if(item.day<dailyDay)
            {
                item.status = 2;
            }else if(item.day == dailyDay)
            {
                item.status = 1;
            }
            else
            {
                item.status = 0;
            }
        }

        for(int i = 0;i< dailyItems.Length;i++)
        {
            if(i< dailyConfig.Length)
            {
                dailyItems[i].SetData(dailyConfig[i]);
            }
            else
            {
                dailyItems[i].SetData(null);
            }
        }

        for (int i = 0; i < weeklyItems.Length; i++)
        {
            if (i < weeklyConfig.Length)
            {
                weeklyItems[i].SetData(weeklyConfig[i]);
            }
            else
            {
                weeklyItems[i].SetData(null);
            }
        }
    }

    public void OnClickGetRewardBtn()
    {
        GameMainManager.instance.netManager.GetDailyLoginReward((ret,res)=> {
            if(res.isOK)
            {
                GetRewardWindowData rewardData = new GetRewardWindowData();
                rewardData.reward = new RewardData
                {
                    type = res.data.prize[0].type,
                    num = res.data.prize[0].num,
                    
                };
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, rewardData);

                UserData user = GameMainManager.instance.model.userData;
                SetData(user.daily_prize_confs, user.weekly_prize_confs, res.data.dailyPrizeDay, user.week_prize_day, res.data.daily_prize_limit);

            }
           

        });
    }


}
