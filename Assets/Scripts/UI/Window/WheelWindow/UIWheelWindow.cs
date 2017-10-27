using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class UIWheelWindow : UIWindowBase {

    public UIWheelPanel wheelPanel;
    public UIBuildPanel buildPanel;

    private UserData user;

    private long energyTimeToRecover;//增加体力剩余时间
    private float energyTimeToRecoverTag;// 获取体力剩余时间时的标记

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

    }

    private void Start()
    {

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
            buildPanel.setData(ud.islandId, ud.buildings);
        }

    }

    protected override void StartShowWindow(object[] data)
    {
        user = GameMainManager.instance.model.userData;
        if (user != null)
        {
            updateUserData(user);
        }

    }

    protected override void EnterAnimation(Action onComplete)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.wheel_come);
        int count = 0;
        wheelPanel.OpenPanel(()=> {
            count++;
            if(count>1)
            {
                onComplete();
            }
            });
        buildPanel.OpenPanel(() => {
            count++;
            if (count > 1)
            {
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
                onComplete();
            }
        });
        buildPanel.ClosePanel(() => {
            count++;
            if (count > 1)
            {
                onComplete();
            }
        });
    }

    public void onClickShowBuildBtn()
    {
        wheelPanel.enterToBuildPanelState();
        buildPanel.enterToBuildPanelState();
    }

    public void onClickShowWheelBtn()
    {
        wheelPanel.enterToWheelPanelState();
        buildPanel.enterToWheelPanelState();
    }


}
