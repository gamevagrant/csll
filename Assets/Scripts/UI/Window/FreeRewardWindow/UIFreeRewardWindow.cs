using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFreeRewardWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIFreeRewardWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }
    [SerializeField]
    private Slider progress;
    [SerializeField]
    private BaseScrollView scrollView;
    [SerializeField]
    private TextMeshProUGUI progressText;

    private ShareData.InviteReward[] rewards;
    protected override void StartShowWindow(object[] data)
    {
        GameMainManager.instance.netManager.GetRecallableFriends((ret, res) =>
        {
            progress.value = (float)res.data.invite_process / res.data.invite_reward_num_limit;
            progressText.text = res.data.invite_process.ToString() + "/" + res.data.invite_reward_num_limit.ToString();
            rewards = res.data.invite_friend_rewards;
            scrollView.SetData(rewards);

        });
    }

    public void OnClickInviteBtn()
    {
        OnClickClose();
       
        if (AccountManager.instance.isLoginAccount)
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIInviteWindow);
        }
        else
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFacebookTipsWindow);
        }
    }
}
