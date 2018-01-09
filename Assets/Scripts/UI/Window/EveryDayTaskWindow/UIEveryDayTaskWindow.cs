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


    private DailyTaskData dailyTaskData;

    protected override void StartShowWindow(object[] data)
    {
        dailyTaskData = GameMainManager.instance.model.userData.daily_task;
        UpdateData();
    }

    private void UpdateData()
    {
        getRewardBtn.interactable = dailyTaskData.extra_reward.status == 1 ? true : false;
        scrollView.SetData(dailyTaskData.tasks);
    }

    public void OnClickGetRewardBtn()
    {
        GameMainManager.instance.netManager.GetDailyTaskReward(0, (ret,res) =>
        {
            dailyTaskData = res.data.daily_task;
            UpdateData();
        });
    }
}
