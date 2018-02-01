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
                _windowData.siblingNum = 0.1f;
            }
            return _windowData;
        }
    }

    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public RectTransform mapIcon;

    public RectTransform[] icons;//排行，免费奖励，活动，每日任务，每日登录，每日能量
    public GameObject raycastPanel;


    private void Awake()
    {

        leftPanel.anchoredPosition = new Vector2(0, 800);
        rightPanel.anchoredPosition = new Vector2(0, 800);
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
            mapIcon.gameObject.SetActive(true);
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
        mapIcon.gameObject.SetActive(false);
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



    public void OnClickRankBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIRankWindow);
    }

    public void OnClickDailyTaskBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIEveryDayTaskWindow);

    }

    public void OnClickDailyRewardBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIEveryDayRewardWindow);
    }

    public void OnClickDailyEnergyBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIEveryDayEnergyWindow);
    }

    public void OnClickFreeRewardBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFreeRewardWindow);
       
    }

    public void OnClickMapBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIMiningMapWindow);
    }

    public void OnClickFirstBuyRewardBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFirstBuyingRewardWindow);
    }
}

