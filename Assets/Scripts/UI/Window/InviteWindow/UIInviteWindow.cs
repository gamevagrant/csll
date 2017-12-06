using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInviteWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIInviteWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public UIInvitePanel invitePanel;
    public UIRecallPanel recallPanel;

    protected override void StartShowWindow(object[] data)
    {
        invitePanel.Refresh();
        recallPanel.Refresh();

    }
}
