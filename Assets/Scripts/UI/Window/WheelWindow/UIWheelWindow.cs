using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIWheelWindow : UIWindowBase {

    public UIWheelPanel wheelPanel;
    public UIBuildPanel buildPanel;

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
        UserData ud = GameMainManager.instance.model.userData;
        updateUserData(ud);
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOGIN_COMPLATE, onUpdateUserData);
    }


    private void onUpdateUserData(BaseEvent e)
    {
        updateUserData(e.datas[0] as UserData);
    }

    private void updateUserData(UserData ud)
    {
        if(ud!=null)
        {
            wheelPanel.setData(ud.rollerItems);
            buildPanel.setData(ud.islandId, ud.buildings);
        }

    }

    protected override void StartShowWindow(object[] data)
    {

    }

    protected override void EnterAnimation(Action onComplete)
    {
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
