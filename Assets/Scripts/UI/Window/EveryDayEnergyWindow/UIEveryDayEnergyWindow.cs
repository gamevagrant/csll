using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEveryDayEnergyWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIEveryDayEnergyWindow;
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
    private QY.UI.Button getRewardBtn;

    protected override void StartShowWindow(object[] data)
    {
        getRewardBtn.interactable = GameMainManager.instance.model.userData.daily_energy > 0;
    }

    public void OnClickGetRewardBtn()
    {
        if(GameMainManager.instance.model.userData.energy>=GameMainManager.instance.model.userData.maxEnergy)
        {
            Alert.Show("能量已满，用掉一些再来吧。");
            return;
        }
        GameMainManager.instance.netManager.GetDailyEnergyReward((ret, res) =>
        {
            GetRewardWindowData rewardData = new GetRewardWindowData();
            rewardData.reward = new RewardData();
            rewardData.reward.type = "energy";
            rewardData.reward.num = res.data.energy - GameMainManager.instance.model.userData.energy;

            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, rewardData);
            OnClickClose();
        });
    }
}
