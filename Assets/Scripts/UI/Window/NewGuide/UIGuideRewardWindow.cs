using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuideRewardWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIGuideRewardWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public QY.UI.Button getRewardBtn;

    protected override void StartShowWindow(object[] data)
    {
        getRewardBtn.isIgnoreLock = true;
    }

    public void OnClickGetRewardBtn()
    {
        GameMainManager.instance.netManager.TutorialComplete((ret, res) =>
        {
            
        });

        OnClickClose();
    }
}
