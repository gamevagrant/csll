using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings  {

    public enum UIWindowID
    {
        UIWheelWindow,
        UIBuildingWindow,
        UIAttackWindow,
        UIStealWindow,
        UILeftDatailWindow,
        UITopBarWindow,
        UISideBarWindow,
        UIPopupMessageWindow,
        UIPopupModalBox,
        UIModalBox,
        UINoticeWindow,
        UIMessageMailWindow,
        UIGetRewardWindow,
        UIBadGuyRankWindow,
        UIBuyWantedWindow,
        UICheckPlayerIslandWindow,
        UIFriendsWindow,
        UIShopWindow,
        UIRankWindow,
        UIMiningMapWindow,
        UIMiningWindow,
        UICloudCover,
        UIWaitingWindow,
        UIInviteWindow,
        UIFacebookTipsWindow,
        UISettingWindow,
        UIGuideTipsWindow,
        UIGuideWindow,
        UIGuideRewardWindow,
        UIEveryDayTaskWindow,
        UIEveryDayRewardWindow,
        UIEveryDayEnergyWindow,
        UIFreeRewardWindow,
        UIEmptyEnergyGuideWindow,
        UINewUserGuiderWindow,
        UIExchangeCodeWindow,
        UIExchangeRewardWindow,
        UIGetBindingRewardWindow,
        UIFirstBuyingRewardWindow,
        UIDungeonWindow,
        UIDungeonLottoWindow,
        UIDungeonGetCardWindow,
        UIDungeonGetRewardWindow,
        UIDungeonGetMailRewardWindow,
        UIDungeonLottoHelpWindow,
        UIDungeonGetKeyWindow,
    }

    public enum UIWindowType
    {
        Normal,    // 可推出界面(UIMainMenu,UIRank等)
        Fixed,     // 固定窗口(UITopBar等)
        PopUp,     // 模式窗口(UIMessageBox, yourPopWindow , yourTipsWindow ......)
        Cover,      //覆盖效果
    }

    public enum UIWindowColliderMode
    {
        Normal,    // 背景板阻挡点击事件
        TouchClose,    // 点击背景板关闭面板
    }

    public enum UIWindowColliderType
    {
        Transparent,//透明的
        SemiTransparent,//半透
    }

    public enum UIWindowShowMode
    {
        DoNothing = 0,
        HideOtherWindow,
        DestoryOtherWindow,
    }

    public enum UIWindowNavigationMode
    {
        IgnoreNavigation = 0,
        NormalNavigation,
    }

    // Main folder
    //public static string UIPrefabPath = "UIPrefab/";
    private static Dictionary<UIWindowID, string> windowPrefabPath = new Dictionary<UIWindowID, string>()
    {
        {UIWindowID.UIWheelWindow,"UIWheelWindow"},
        {UIWindowID.UIBuildingWindow,"UIBuildingWindow"},
        {UIWindowID.UIAttackWindow,"UIAttackWindow"},
        {UIWindowID.UIStealWindow,"UIStealWindow"},
        {UIWindowID.UILeftDatailWindow,"UILeftDatailWindow"},
        {UIWindowID.UITopBarWindow,"UITopBarWindow"},
        {UIWindowID.UISideBarWindow,"UISideBarWindow"},
        {UIWindowID.UIPopupMessageWindow,"UIPopupMessageWindow" },
        {UIWindowID.UIPopupModalBox,"UIPopupModalBoxWindow" },
        {UIWindowID.UIModalBox,"UIModalBoxWindow" },
        {UIWindowID.UINoticeWindow,"UINoticeWindow" },
        {UIWindowID.UIMessageMailWindow,"UIMessageMailWindow" },
        {UIWindowID.UIGetRewardWindow,"UIGetRewardWindow" },
        {UIWindowID.UIBadGuyRankWindow,"UIBadGuyRankWindow" },
        {UIWindowID.UIBuyWantedWindow,"UIBuyWantedWindow" },
        {UIWindowID.UICheckPlayerIslandWindow,"UICheckPlayerIslandWindow" },
        {UIWindowID.UIFriendsWindow,"UIFriendsWindow" },
        {UIWindowID.UIShopWindow,"UIShopWindow" },
        {UIWindowID.UIRankWindow,"UIRankWindow" },
        {UIWindowID.UIMiningMapWindow,"UIMiningMapWindow" },
        {UIWindowID.UIMiningWindow,"UIMiningWindow" },
        {UIWindowID.UICloudCover,"UICloudCover"},
        {UIWindowID.UIWaitingWindow,"UIWaitingWindow"},
        {UIWindowID.UIInviteWindow,"UIInviteWindow"},
        {UIWindowID.UIFacebookTipsWindow,"UIFacebookTipsWindow"},
        {UIWindowID.UISettingWindow,"UISettingWindow"},
        {UIWindowID.UIGuideTipsWindow,"UIGuideTipsWindow"},
        {UIWindowID.UIGuideWindow,"UIGuideWindow"},
        {UIWindowID.UIGuideRewardWindow,"UIGuideRewardWindow"},
        {UIWindowID.UIEveryDayTaskWindow,"UIEveryDayTaskWindow"},
        {UIWindowID.UIEveryDayRewardWindow,"UIEveryDayRewardWindow"},
        {UIWindowID.UIEveryDayEnergyWindow,"UIEveryDayEnergyWindow"},
        {UIWindowID.UIFreeRewardWindow,"UIFreeRewardWindow"},
        {UIWindowID.UIEmptyEnergyGuideWindow,"UIEmptyEnergyGuideWindow"},
        {UIWindowID.UINewUserGuiderWindow,"UINewUserGuiderWindow"},
        {UIWindowID.UIExchangeCodeWindow,"UIExchangeCodeWindow"},
        {UIWindowID.UIExchangeRewardWindow,"UIExchangeRewardWindow"},
        {UIWindowID.UIGetBindingRewardWindow,"UIGetBindingRewardWindow"},
        {UIWindowID.UIFirstBuyingRewardWindow,"UIFirstBuyingRewardWindow"},
        {UIWindowID.UIDungeonWindow,"UIDungeonWindow"},
        {UIWindowID.UIDungeonLottoWindow,"UIDungeonLottoWindow"},
        {UIWindowID.UIDungeonGetCardWindow,"UIDungeonGetCardWindow"},
        {UIWindowID.UIDungeonGetRewardWindow,"UIDungeonGetRewardWindow"},
        {UIWindowID.UIDungeonGetMailRewardWindow,"UIDungeonGetMailRewardWindow"},
        {UIWindowID.UIDungeonLottoHelpWindow,"UIDungeonLottoHelpWindow"},
        {UIWindowID.UIDungeonGetKeyWindow,"UIDungeonGetKeyWindow"},
    };



    public static string getWindowName(UIWindowID id)
    {
        string name;
        windowPrefabPath.TryGetValue(id, out name);
        return name;
    }

}
