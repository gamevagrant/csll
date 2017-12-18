using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFacebookTipsWindow :UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIFacebookTipsWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public void OnClickFacebookBtn()
    {
        AccountManager.instance.BindAccount((issuccess)=> 
        {
            if(issuccess)
            {
                OnClickClose();
            }
        });
    }
}
