using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;

public class UIEmptyEnergyGuideWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIEmptyEnergyGuideWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public QY.UI.Button[] btns;//每日登录，每日能量，好友赠送，完成拼图，每日任务
    public QY.UI.Button getRewardBtn;

    public GameObject guideRewardPanel;
    public GameObject buyVipPanel;
    public TextMeshProUGUI vipPriceText;

    private Product productVip;
    protected override void StartShowWindow(object[] data)
    {
        ShowEmptyGuideWindowData openData;
        if (data!=null && data.Length>0&&data[0]!=null)
        {
            openData = data[0] as ShowEmptyGuideWindowData;
           
        }else
        {
            openData = new ShowEmptyGuideWindowData(ShowEmptyGuideWindowData.PanelType.GuideReward);
        }
        if (openData.type == ShowEmptyGuideWindowData.PanelType.GuideReward)
        {
            getRewardBtn.isIgnoreLock = true;
            guideRewardPanel.SetActive(true);
            buyVipPanel.SetActive(false);
            foreach(QY.UI.Button btn in btns)
            {
                btn.interactable = false;
            }
        }
        else
        {
            guideRewardPanel.SetActive(false);
            buyVipPanel.SetActive(true);

            productVip = GameMainManager.instance.iap.GetProductWithID(new GoodsData("304").GetPurchaseID());
            vipPriceText.text = productVip.metadata.localizedPriceString;

            btns[0].interactable = GameMainManager.instance.model.userData.dailyRewardTip > 0 ? true : false;
            btns[1].interactable = GameMainManager.instance.model.userData.dailyEnergyTip > 0 ? true : false;
            btns[2].interactable = GameMainManager.instance.model.userData.friendTip > 0 ? true : false;
            btns[3].interactable = false;
            btns[4].interactable = GameMainManager.instance.model.userData.dailyTaskTip > 0 ? true : false;
        }
       
    }

    public void OnClickDailyLoginBtn()
    {
       
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIEveryDayRewardWindow);
        OnClickClose();
    }

    public void OnClickDailyEnergyBtn()
    {
        
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIEveryDayEnergyWindow);
        OnClickClose();
    }

    public void OnClickFriendBtn()
    {
        
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFriendsWindow);
        OnClickClose();
    }

    public void OnClickPieceBtn()
    {
        OnClickClose();
    }
    public void OnClickDailyTaskBtn()
    {
        
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIEveryDayTaskWindow);
        OnClickClose();
    }

    public void OnClickBuyVipBtn()
    {
        if(productVip!=null)
        {
           
            GameMainManager.instance.iap.Purchase(productVip.definition.id);
            OnClickClose();
        }
      
    }

    public void OnClickGetRewardBtn()
    {
        GameMainManager.instance.netManager.TutorialComplete((ret, res) =>
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UINewUserGuiderWindow);
        });

        OnClickClose();
    }
}
