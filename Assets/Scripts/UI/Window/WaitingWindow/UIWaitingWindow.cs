using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIWaitingWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIWaitingWindow;
                _windowData.type = UISettings.UIWindowType.Cover;
            }
            return _windowData;
        }
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
