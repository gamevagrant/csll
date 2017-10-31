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
        UIModalBoxWindow,
    }

    public enum UIWindowType
    {
        Normal,    // 可推出界面(UIMainMenu,UIRank等)
        Fixed,     // 固定窗口(UITopBar等)
        PopUp,     // 模式窗口(UIMessageBox, yourPopWindow , yourTipsWindow ......)
    }

    public enum UIWindowColliderMode
    {
        Normal,    // 背景板阻挡点击事件
        TouchClose,    // 点击背景板关闭面板
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
        { UIWindowID.UIWheelWindow,"UIWheelWindow"},
         { UIWindowID.UIBuildingWindow,"UIBuildingWindow"},
          { UIWindowID.UIAttackWindow,"UIAttackWindow"},
           { UIWindowID.UIStealWindow,"UIStealWindow"},
            { UIWindowID.UILeftDatailWindow,"UILeftDatailWindow"},
             { UIWindowID.UITopBarWindow,"UITopBarWindow"},
              { UIWindowID.UISideBarWindow,"UISideBarWindow"},
                { UIWindowID.UIPopupMessageWindow,"UIPopupMessageWindow" },
        {UIWindowID.UIModalBoxWindow,"UIModalBoxWindow" },
    };

    public static string getWindowName(UIWindowID id)
    {
        string name;
        windowPrefabPath.TryGetValue(id, out name);
        return name;
    }

}
