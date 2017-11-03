using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuyWantedWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIBuyWantedWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    protected override void StartShowWindow(object[] data)
    {
        base.StartShowWindow(data);
    }

    protected override void EnterAnimation(Action onComplete)
    {
        base.EnterAnimation(onComplete);
    }

    protected override void ExitAnimation(Action onComplete)
    {
        base.ExitAnimation(onComplete);
    }
}
