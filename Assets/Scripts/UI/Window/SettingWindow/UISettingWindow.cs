using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.UI;
using UnityEngine.SceneManagement;

public class UISettingWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UISettingWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.NormalNavigation;
            }

            return _windowData;
        }
    }

    public Button logoutBtn;
    public Button facebookBtn;
    public HeadIcon head;

    protected override void StartShowWindow(object[] data)
    {
        UserData user = GameMainManager.instance.model.userData;
        logoutBtn.gameObject.SetActive(AccountManager.instance.isLoginAccount);
        facebookBtn.gameObject.SetActive(!AccountManager.instance.isLoginAccount);
        head.setData(user.name, user.headImg, 0, user.isVip);
    }

    public void OnSoundToggleChange(bool isSelected)
    {
        AudioManager.instance.soundVolume = isSelected?1:0;
    }

    public void OnMusicToggleChange(bool isSelected)
    {
        AudioManager.instance.musicVolume = isSelected ? 1 : 0;
    }

    public void OnClickLogoutBtn()
    {
        AccountManager.instance.Logout();
        SceneManager.LoadScene("Login");
    }

    public void OnClickFacebookBtn()
    {
        AccountManager.instance.LoinPlatform((issuccess)=> {
            if (issuccess)
            {
                OnClickClose();
            }
        });
    }

    public void OnClickLikeBtn()
    {
        Application.OpenURL(GameSetting.homePage);
    }

    public void OnClickExchangeBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIExchangeCodeWindow);
        OnClickClose();
    }
}
