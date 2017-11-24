using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;


public class UIWheelWindow : UIWindowBase {

    private enum State
    {
        Wheel,
        Building,
        Closed,
    }

    public UIWheelPanel wheelPanel;
    public UIBuildPanel buildPanel;
    public RectTransform leftBtn;

    private UserData user;


    private State currState;
    private State openState;

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
       // EventDispatcher.instance.AddEventListener(EventEnum.LOGIN_COMPLATE, onUpdateUserData);
        currState = State.Closed;

        leftBtnPos = leftBtn.anchoredPosition;
    }


    private void Update()
    {
        /*
        if(user != null)
        {
            wheelPanel.SetEnergyData(user.maxEnergy, user.energy, user.recoverEnergy, energyTimeToRecover - (long)(Time.time - energyTimeToRecoverTag));
        }
        */
        if(qy.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown(CrossPlatformInput.LEFT))
        {
            if(GameMainManager.instance.uiManager.isEnable && GameMainManager.instance.uiManager.curWindow.windowData == windowData)
            {
                onClickShowBuildBtn();
            }
        }else if(qy.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown(CrossPlatformInput.RIGHT))
        {
            if (GameMainManager.instance.uiManager.isEnable && GameMainManager.instance.uiManager.curWindow.windowData == windowData)
            {
                onClickShowWheelBtn();
            }
        }
    }
    /*
    private void OnDestroy()
    {
        //EventDispatcher.instance.RemoveEventListener(EventEnum.LOGIN_COMPLATE, onUpdateUserData);
    }


    private void onUpdateUserData(BaseEvent e)
    {
        user = e.datas[0] as UserData;
        updateUserData(user);
    }*/

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
        openState = State.Wheel;
        if(data!= null && data.Length>0 && data[0]!=null)
        {
            openState = (int)data[0]==0?State.Wheel:State.Building;
           
        }
        
    }

    protected override void EnterAnimation(Action onComplete)
    {
        if(currState!=State.Closed)
        {   
            if(currState != openState)
            {
                if (openState == State.Wheel)
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
                if (openState == State.Building)
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
                if (openState == State.Building)
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
                currState = State.Closed;
                onComplete();
            }
        });
        buildPanel.ClosePanel(() => {
            count++;
            if (count > 1)
            {
                currState = State.Closed;
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
        wheelPanel.enterToBuildPanelState();
        buildPanel.enterToBuildPanelState();
        currState = State.Building;
    }

    public void onClickShowWheelBtn()
    {
        wheelPanel.enterToWheelPanelState();
        buildPanel.enterToWheelPanelState();
        currState = State.Wheel;
    }

    public void OnClickLeftDatailBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UILeftDatailWindow);
    }
}
