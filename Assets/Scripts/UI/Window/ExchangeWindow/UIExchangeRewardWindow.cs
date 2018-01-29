using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExchangeRewardWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIExchangeRewardWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public GridBaseScrollView scrollView;

    protected override void StartShowWindow(object[] data)
    {
        if(data!=null && data.Length>0 && data[0]!=null)
        {
            ShowExchangeRewardData windowData = data[0] as ShowExchangeRewardData;
            scrollView.SetData(windowData.rewards);
        }
    }

    public void OnClickGetRewardBtn()
    {
        DispatchUpdateEvent();
        OnClickClose();
    }

    public override void OnClickClose()
    {
        DispatchUpdateEvent();
        base.OnClickClose();
    }

    private void DispatchUpdateEvent()
    {
        EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Energy, 0));
        EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money, 0));
        EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.vip, 0));
        EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.wanted, 0));
    }
}
