using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIExchangeCodeWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIExchangeCodeWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public TMP_InputField inputField;

    protected override void StartShowWindow(object[] data)
    {
        base.StartShowWindow(data);
    }

    public void OnClickExchangeBtn()
    {
        string str = inputField.text.Trim();
        if (string.IsNullOrEmpty(str))
        {
            Alert.Show("亲，兑换码不能为空嗒！");
            return;
        }
        
        GameMainManager.instance.netManager.UseExchangeCode(str, (ret, res) =>
        {
            if(ret&&res.isOK)
            {
                OnClickClose();
                ShowExchangeRewardData windowData = new ShowExchangeRewardData()
                {
                    rewards = res.data.reward
                };
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIExchangeRewardWindow, windowData);
               
            }

        });
    }

}
