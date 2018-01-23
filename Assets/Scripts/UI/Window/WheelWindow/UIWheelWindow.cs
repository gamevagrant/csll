using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;


public class UIWheelWindow : UIWindowBase {

    private enum OpenState
    {
        Wheel,
        Building,
        Closed,
    }

    public UIWheelPanel wheelPanel;
    public UIBuildPanel buildPanel;
    public RectTransform leftBtn;

    private UserData user;


    private OpenState currState;
    private OpenState openState;

    private Vector2 leftBtnPos;

    public override UIWindowData windowData
    {
        get
        {
            if(_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIWheelWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.NormalNavigation;
                _windowData.siblingNum = 0;
            }
           
            return _windowData;
        }
    }

    private void Awake()
    {
        currState = OpenState.Closed;

        leftBtnPos = leftBtn.anchoredPosition;
    }



    private void updateUserData(UserData ud)
    {
        if(ud!=null)
        {
            wheelPanel.SetData(ud.rollerItems);
            wheelPanel.SetStealerData(ud.stealTarget);
            buildPanel.SetData(ud.islandId, ud.buildings, OnUpgrading);
        }

    }

    protected override void StartShowWindow(object[] data)
    {
        user = GameMainManager.instance.model.userData;
        
        if (user != null)
        {
            updateUserData(user);
        }
       
        if(data!= null && data.Length>0 && data[0]!=null)
        {
            openState = (int)data[0]==0?OpenState.Wheel:OpenState.Building;
           
        }else
        {
            openState = OpenState.Wheel;
        }
        
    }

    protected override void EnterAnimation(Action onComplete)
    {
       if(openState== OpenState.Wheel)
        {
            ShowWheelStateAnimation();
        }else if(openState == OpenState.Building)
        {
            ShowBuildStateAnimation();
        }
        ShowLeftBtn(true);
        onComplete();
    }
    protected override void ExitAnimation(Action onComplete)
    {
        ShowHideStateAnimation(onComplete);
        ShowLeftBtn(false);

    }

    protected override void EndShowWindow()
    {
        base.EndShowWindow();
        if (openState == OpenState.Wheel)
        {
            ShowWheelState();
        }
        else if (openState == OpenState.Building)
        {
            ShowBuildState();
        }
        ShowLeftBtn(true);
    }
    protected override void EndHideWindow()
    {
        base.EndHideWindow();
    }

    private void OnUpgrading(bool isUpgrading)
    {
        ShowLeftBtn(!isUpgrading);
    }

    private void ShowLeftBtn(bool isShow)
    {
        if(isShow)
        {
            leftBtn.DOAnchorPos(leftBtnPos, 0.5f).SetEase(Ease.OutCubic);
        }
        else
        {
            leftBtn.DOAnchorPos(leftBtnPos - new Vector2(150, 0), 0.5f).SetEase(Ease.OutCubic);
        }
    }

   

    public void ShowWheelState()
    {
        if (currState != OpenState.Wheel)
        {
            wheelPanel.ShowWheelState();
            buildPanel.ShowWheelState();
            currState = OpenState.Wheel;
            QY.Guide.GuideManager.instance.state = "wheel";
            if (!GameMainManager.instance.model.userData.isTutorialing)
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UISideBarWindow, false);
        }
    }
    private void ShowBuildState()
    {
        if (currState != OpenState.Building)
        {
            wheelPanel.ShowBuildState();
            buildPanel.ShowBuildState();
            currState = OpenState.Building;
            QY.Guide.GuideManager.instance.state = "building";
            if (!GameMainManager.instance.model.userData.isTutorialing)
                GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UISideBarWindow, false);
        }
    }
    private void ShowWheelStateAnimation()
    {
        if (currState != OpenState.Wheel)
        {
            wheelPanel.EnterToWheelPanelState();
            buildPanel.EnterToWheelPanelState();
            currState = OpenState.Wheel;
            QY.Guide.GuideManager.instance.state = "wheel";
            if (!GameMainManager.instance.model.userData.isTutorialing)
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UISideBarWindow, true);
        }
    }

    private void ShowBuildStateAnimation()
    {
        if (currState != OpenState.Building)
        {
            wheelPanel.EnterToBuildPanelState();
            buildPanel.EnterToBuildPanelState();
            currState = OpenState.Building;
            QY.Guide.GuideManager.instance.state = "building";
            if (!GameMainManager.instance.model.userData.isTutorialing)
                GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UISideBarWindow, true);
        }
    }

    private void ShowHideStateAnimation(Action onComplate)
    {
        wheelPanel.EnterToHideState(()=> {

        });
        buildPanel.EnterToHideState(()=> {
            onComplate();
        });
        currState = OpenState.Closed;
    }

    public void onClickShowBuildBtn()
    {
        ShowBuildStateAnimation();
        
    }

    public void onClickShowWheelBtn()
    {
        ShowWheelStateAnimation();       
    }

    public void OnClickLeftDatailBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UILeftDatailWindow);
    }
}
