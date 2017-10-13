using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWheelWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if(_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIWheel;
                _windowData.type = UISettings.UIWindowType.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.NormalNavigation;
            }
           
            return _windowData;
        }
    }
}
