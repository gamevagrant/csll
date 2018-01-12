using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEveryDayTaskWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIEveryDayTaskWindow;
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
    [SerializeField]
    private BaseScrollView scrollView;
    [SerializeField]
    private GameObject taskPanel;
    [SerializeField]
    private GameObject extraPanel;
    [SerializeField]
    private QY.UI.Toggle facebookToggle;


    private DailyTaskData dailyTaskData;

    protected override void StartShowWindow(object[] data)
    {
        extraPanel.SetActive(false);
        taskPanel.SetActive(true);

        GameMainManager.instance.netManager.GetDailyTask((ret, res) =>
        {
            dailyTaskData = res.data.daily_task;
            UpdateData();
        });
       
       
    }

    private void UpdateData()
    {
        getRewardBtn.interactable = dailyTaskData.extra_reward.status == 1 ? true : false;
        scrollView.SetData(dailyTaskData.tasks);
    }

    public void OnClickGetRewardBtn()
    {
        extraPanel.SetActive(true);
        taskPanel.SetActive(false);

    }

    public void OnClickGetRewardAndShare()
    {
        GameMainManager.instance.netManager.GetDailyTaskReward(0, (ret, res) =>
        {
            extraPanel.SetActive(false);
            taskPanel.SetActive(true);

            if (res.isOK)
            {
                dailyTaskData = res.data.daily_task;
                UpdateData();

                GetRewardWindowData rewardData = new GetRewardWindowData();
                rewardData.reward = dailyTaskData.extra_reward.reward;
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, rewardData);

            }

        });

        if (facebookToggle.isOn)
        {
            GameMainManager.instance.open.ShareLink(GameSetting.shareFinishTaskLink);
        }
    }
}
