using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.UI;
using System;

public class UIGuideWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIGuideWindow;
                _windowData.type = UISettings.UIWindowType.Cover;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public RectTransform pointer;
    private Transform point;

    private void Update()
    {
        if(point!= null)
        {
            pointer.transform.position = point.position;
        }
       
    }

    protected override void StartShowWindow(object[] data)
    {
        Interactable interact = data[0] as Interactable;
        point = interact.transform;
        pointer.transform.position = point.position;
        pointer.gameObject.SetActive(true);
    }

    protected override void StartHideWindow()
    {
        point = null;
        pointer.gameObject.SetActive(false);
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
