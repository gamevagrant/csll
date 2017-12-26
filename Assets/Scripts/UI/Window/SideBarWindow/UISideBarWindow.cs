using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.UI;
using UnityEngine.EventSystems;
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

    public RectTransform rank;

    private void Awake()
    {
        rank.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIRankWindow);
        });
        leftPanel.anchoredPosition = new Vector2(0, 800);
        rightPanel.anchoredPosition = new Vector2(0, 800);
    }

    private void OnDestroy()
    {
        rank.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
    }


    protected override void StartShowWindow(object[] data)
    {
       
    }

    protected override void StartHideWindow()
    {
        base.StartHideWindow();
    }

    protected override void EnterAnimation(Action onComplete)
    {
        leftPanel.DOAnchorPos(new Vector2(0, -60), 1f).SetEase(Ease.OutCubic);
        rightPanel.DOAnchorPos(new Vector2(0, -60), 1f).SetEase(Ease.OutCubic).OnComplete(()=> {
            onComplete();
        });
    }
    protected override void EndShowWindow()
    {
        leftPanel.anchoredPosition = new Vector2(0, -60);
        rightPanel.anchoredPosition = new Vector2(0, -60);
    }
    protected override void ExitAnimation(Action onComplete)
    {
        leftPanel.DOAnchorPos(new Vector2(0, 800), 1f).SetEase(Ease.OutCubic);
        rightPanel.DOAnchorPos(new Vector2(0, 800), 1f).SetEase(Ease.OutCubic).OnComplete(() => {
            onComplete();
        }); 
    }

    protected override void EndHideWindow()
    {
        leftPanel.anchoredPosition = new Vector2(0, 800);
        rightPanel.anchoredPosition = new Vector2(0, 800);
    }
}
