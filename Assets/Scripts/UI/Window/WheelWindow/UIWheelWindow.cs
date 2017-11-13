using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class UIWheelWindow : UIWindowBase {

    private enum State
    {
        Wheel,
        Building,
        Closed,
    }

    public UIWheelPanel wheelPanel;
    public UIBuildPanel buildPanel;

    private UserData user;

    private long energyTimeToRecover;//增加体力剩余时间
    private float energyTimeToRecoverTag;// 获取体力剩余时间时的标记
    private State currState;
    private State openState;

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
        EventDispatcher.instance.AddEventListener(EventEnum.LOGIN_COMPLATE, onUpdateUserData);
        currState = State.Closed;
    }


    private void Update()
    {
        if(user != null)
        {
            wheelPanel.SetEnergyData(user.maxEnergy, user.energy, user.recoverEnergy, energyTimeToRecover - (long)(Time.time - energyTimeToRecoverTag));
        }
       
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOGIN_COMPLATE, onUpdateUserData);
    }


    private void onUpdateUserData(BaseEvent e)
    {
        user = e.datas[0] as UserData;
        updateUserData(user);
    }

    private void updateUserData(UserData ud)
    {
        if(ud!=null)
        {
            energyTimeToRecover = ud.timeToRecover;
            energyTimeToRecoverTag = Time.time;
           
            wheelPanel.setData(ud.rollerItems);
            wheelPanel.SetStealerData(ud.stealTarget);
            buildPanel.setData(ud.islandId, ud.buildings,ud.mapInfo);
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
        if(data!= null && data[0]!=null)
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
