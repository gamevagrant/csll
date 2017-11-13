using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UILeftDatailWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UILeftDatailWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public Image backCoilder;
    public RectTransform panel;
    public HeadIcon head;
    public TextMeshProUGUI friendCode;
    public TextMeshProUGUI[] buttonTips;//0:转盘 1：建造 3：好友 4：消息 7：地图

    private void Awake()
    {
        panel.anchoredPosition = new Vector2(-530, 0);
    }

    protected override void StartShowWindow(object[] data)
    {
        UserData ud = GameMainManager.instance.model.userData;
        head.setData(ud.name, ud.headImg, ud.crowns, ud.isVip);
        friendCode.text = "友情码：" + ud.friendshipCode;

        for(int i = 0;i<buttonTips.Length;i++)
        {
            buttonTips[i].transform.parent.gameObject.SetActive(false);
        }

        if(ud.buildingTip>0)
        {
            buttonTips[1].transform.parent.gameObject.SetActive(true);
            buttonTips[1].text = ud.buildingTip.ToString();
        }
        if(ud.friendTip>0)
        {
            buttonTips[3].transform.parent.gameObject.SetActive(true);
            buttonTips[3].text = ud.friendTip.ToString();
        }
        if(ud.mailTip>0)
        {
            buttonTips[4].transform.parent.gameObject.SetActive(true);
            buttonTips[4].text = ud.mailTip.ToString();
        }
    }

    protected override void StartHideWindow()
    {
        base.StartHideWindow();
    }

    protected override void EnterAnimation(Action onComplete)
    {
        panel.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.OutCubic);
        backCoilder.DOFade(0.5f, 0.3f).OnComplete(()=> {
            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        panel.DOAnchorPos(new Vector2(-530, 0), 0.3f).SetEase(Ease.OutCubic);
        backCoilder.DOFade(0, 0.3f).OnComplete(() => {
            onComplete();
        });
    }


    public void OnClickNoticeBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UINoticeWindow, GameMainManager.instance.model.userData.announcement);

    }

    public void OnClickMessageBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIMessageMailWindow);
    }

    public void OnClickMapBtn()
    {

    }

    public void OnClickWheelBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow,0);
    }

    public void OnClickBuildBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow, 1);
    }

    public void OnClickBadGuyBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIBadGuyRankWindow);
    }

    public void OnClickFriendsBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFriendsWindow);

    }

    public void OnClickShopBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIShopWindow);
    }
}
