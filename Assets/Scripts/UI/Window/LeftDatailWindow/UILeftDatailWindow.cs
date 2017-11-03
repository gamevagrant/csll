using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILeftDatailWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UILeftDatailWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public Image backCoilder;
    public RectTransform panel;

    protected override void StartShowWindow(object[] data)
    {
        panel.anchoredPosition = new Vector2(-530,0);
    }

    protected override void StartHideWindow()
    {
        base.StartHideWindow();
    }

    protected override void EnterAnimation(Action onComplete)
    {
        panel.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);
        backCoilder.DOFade(0.5f, 0.5f).OnComplete(()=> {
            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        panel.DOAnchorPos(new Vector2(-530, 0), 0.5f).SetEase(Ease.OutCubic);
        backCoilder.DOFade(0, 0.5f).OnComplete(() => {
            onComplete();
        });
    }


    public void OnClickNoticeBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UINoticeWindow, GameMainManager.instance.model.userData.announcement);

    }

    public void OnClickMessageBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIMessageMailWindow);
    }

    public void OnClickMapBtn()
    {

    }

    public void OnClickWheelBtn()
    {

    }

    public void OnClickBuildBtn()
    {

    }

    public void OnClickBadGuyBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIBadGuyRankWindow);
    }
}
