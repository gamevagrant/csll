using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UISideBarWindow :UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UISideBarWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public RectTransform leftPanel;
    public RectTransform rightPanel;

    protected override void StartShowWindow(object[] data)
    {
        leftPanel.anchoredPosition = new Vector2(0, 800);
        rightPanel.anchoredPosition = new Vector2(0, 800);
    }

    protected override void StartHideWindow()
    {
        base.StartHideWindow();
    }

    protected override void EnterAnimation(Action onComplete)
    {
        leftPanel.DOAnchorPos(new Vector2(0, -60), 1f).SetEase(Ease.OutCubic);
        rightPanel.DOAnchorPos(new Vector2(0, -60), 1f).SetEase(Ease.OutCubic);
    }

    protected override void ExitAnimation(Action onComplete)
    {
        leftPanel.DOAnchorPos(new Vector2(0, 800), 1f).SetEase(Ease.OutCubic);
        rightPanel.DOAnchorPos(new Vector2(0, 800), 1f).SetEase(Ease.OutCubic);
    }
}
