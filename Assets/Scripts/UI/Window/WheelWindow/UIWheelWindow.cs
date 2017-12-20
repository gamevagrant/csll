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
            }
           
            return _windowData;
        }
    }

    private void Awake()
    {
        currState = OpenState.Closed;

        leftBtnPos = leftBtn.anchoredPosition;
    }

     /*
    private void Update()
    {
       
        if(QY.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown(CrossPlatformInput.LEFT))
        {
            if(GameMainManager.instance.uiManager.isEnable && GameMainManager.instance.uiManager.curWindow.windowData == windowData)
            {
                onClickShowBuildBtn();
            }
        }else if(QY.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown(CrossPlatformInput.RIGHT))
        {
            if (GameMainManager.instance.uiManager.isEnable && GameMainManager.instance.uiManager.curWindow.windowData == windowData)
            {
                onClickShowWheelBtn();
            }
        }
       
        
    }
 */

    private void updateUserData(UserData ud)
    {
        if(ud!=null)
        {
            wheelPanel.setData(ud.rollerItems);
            wheelPanel.SetStealerData(ud.stealTarget);
            buildPanel.setData(ud.islandId, ud.buildings, OnUpgrading);
        }

    }

    protected override void StartShowWindow(object[] data)
    {
        user = GameMainManager.instance.model.userData;
        
        if (user != null)
        {
            updateUserData(user);
        }
        openState = OpenState.Wheel;
        if(data!= null && data.Length>0 && data[0]!=null)
        {
            openState = (int)data[0]==0?OpenState.Wheel:OpenState.Building;
           
        }
        
    }

    protected override void EnterAnimation(Action onComplete)
    {
        if(currState!=OpenState.Closed)
        {   
            if(currState != openState)
            {
                if (openState == OpenState.Wheel)
                {
                    onClickShowWheelBtn();
                }
                else
                {
                    onClickShowBuildBtn();
                }
                currState = openState;
            }
            onComplete();
            return;
        }
        currState = openState;
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_come);
        int count = 0;
        wheelPanel.OpenPanel(()=> {
            count++;
            if(count>1)
            {
                if (openState == OpenState.Building)
                {
                    onClickShowBuildBtn();
                }
                onComplete();
               
            }
            });
        buildPanel.OpenPanel(() => {
            count++;
            if (count > 1)
            {
                if (openState == OpenState.Building)
                {
                    onClickShowBuildBtn();
                }
                onComplete();
            }
        });
        ShowLeftBtn(true);
    }
    protected override void ExitAnimation(Action onComplete)
    {
        int count = 0;
        wheelPanel.ClosePanel(() => {
            count++;
            if (count > 1)
            {
                currState = OpenState.Closed;
                onComplete();
            }
        });
        buildPanel.ClosePanel(() => {
            count++;
            if (count > 1)
            {
                currState = OpenState.Closed;
                onComplete();
            }
        });
        ShowLeftBtn(false);
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

    public void onClickShowBuildBtn()
    {
        if(currState!= OpenState.Building)
        {
            wheelPanel.enterToBuildPanelState();
            buildPanel.enterToBuildPanelState();
            currState = OpenState.Building;
        }
        
    }

    public void onClickShowWheelBtn()
    {
        if(currState!= OpenState.Wheel)
        {
            wheelPanel.enterToWheelPanelState();
            buildPanel.enterToWheelPanelState();
            currState = OpenState.Wheel;
        }
       
    }

    public void OnClickLeftDatailBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UILeftDatailWindow);
    }
}
