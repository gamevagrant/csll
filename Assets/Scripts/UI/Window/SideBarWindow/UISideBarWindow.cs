﻿using System;
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



    public void OnClickRankBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIRankWindow);
    }

    public void OnClickDailyTaskBtn()
    {
        if(GameMainManager.instance.model.userData.islandId <2)
        {
            Alert.Show(string.Format("2号岛屿（{0}）解锁该功能，快去升级岛屿吧！",GameMainManager.instance.configManager.islandConfig.GetIslandName(2)));
        }else
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIEveryDayTaskWindow);
        }
        
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
       
        if (AccountManager.instance.isLoginAccount)
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFreeRewardWindow);
        }
        else
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFacebookTipsWindow);
        }
    }


}

