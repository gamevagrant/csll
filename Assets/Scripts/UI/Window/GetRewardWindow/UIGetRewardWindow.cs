using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIGetRewardWindow : UIWindowBase {

    public TextMeshProUGUI text;
    public Image image;

    public Sprite[] sprites;//0:钱 1：能量
    private GetRewardWindowData getRewardWindowData;

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIGetRewardWindow;
                _windowData.type = UISettings.UIWindowType.Cover;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.Transparent;
            }
            return _windowData;
        }
    }

    protected override void StartShowWindow(object[] data)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_box_down);
        getRewardWindowData = data[0] as GetRewardWindowData;
        RewardData reward = getRewardWindowData.reward;
        if(reward.type == "money")
        {
            image.sprite = sprites[0];
            text.text = "金币 x" + reward.num.ToString();
        }
        else if (reward.type == "energy")
        {
            image.sprite = sprites[1];
            text.text = "能量 x" + reward.num.ToString();
        }else if(reward.type == "vip")
        {
            image.sprite = sprites[2];
            text.text = "vip" + reward.num.ToString();
        }
        else if(reward.type == "props")
        {
            image.sprite = sprites[3];
            text.text = "通缉令 x" + reward.num.ToString();
        }
        image.SetNativeSize();
    }

    protected override void EnterAnimation(Action onComplete)
    {
        onComplete();
    }

    protected override void ExitAnimation(Action onComplete)
    {
        onComplete();
    }

    public void OnClickGetRewardBtn()
    {
        OnClickClose();
        if(getRewardWindowData.OnGetReward!=null)
        {
            getRewardWindowData.OnGetReward();
        }
    }
}

public class GetRewardWindowData
{
    public RewardData reward;
    public System.Action OnGetReward;
}
