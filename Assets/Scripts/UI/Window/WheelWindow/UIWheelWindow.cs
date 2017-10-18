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
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_USERDATA, onUpdateUserData);

    }

    private void Start()
    {
        UserData ud = GameMainManager.instance.model.userData;
        updateUserData(ud);
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_USERDATA, onUpdateUserData);
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

    protected override void startShowWindow()
    {
        RectTransform rtf = transform as RectTransform;
        rtf.anchorMin = Vector2.zero;
        rtf.anchorMax = Vector2.one;
        rtf.offsetMin = Vector2.zero;
        rtf.offsetMax = Vector2.zero;
    }

    public void onClickShowBuildBtn()
    {
        wheelPanel.hidePanel();
        buildPanel.showPanel();
    }

    public void onClickShowWheelBtn()
    {
        wheelPanel.showPanel();
        buildPanel.hidePanel();
    }


}
