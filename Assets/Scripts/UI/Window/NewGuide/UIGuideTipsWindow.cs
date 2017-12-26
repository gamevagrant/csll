using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIGuideTipsWindow : UIWindowBase {

    public TextMeshProUGUI text;
    public QY.UI.Button closeBtn;

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIGuideTipsWindow;
                _windowData.type = UISettings.UIWindowType.Cover;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.TouchClose;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }

    }

    protected override void StartShowWindow(object[] data)
    {
        closeBtn.isIgnoreLock = true;
        text.text = data[0].ToString();
    }

    protected override void EnterAnimation(Action onComplete)
    {
        onComplete();
    }

    protected override void ExitAnimation(Action onComplete)
    {
        onComplete();
    }
}
