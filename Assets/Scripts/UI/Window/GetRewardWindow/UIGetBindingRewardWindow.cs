using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGetBindingRewardWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIGetBindingRewardWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.NormalNavigation;
            }

            return _windowData;
        }
    }

    protected override void StartShowWindow(object[] data)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_box_down);
    }


    public void OnCLickGetRewardBtn()
    {
        EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money,0));
        EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Energy, 0));
        OnClickClose();
    }
}
