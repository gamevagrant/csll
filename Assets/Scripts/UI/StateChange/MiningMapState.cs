using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningMapState : UIStateChangeBase
{
    public MiningMapState()
    {

    }
    public override void ChangeState(Dictionary<UISettings.UIWindowID, UIWindowBase> showingWindows)
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UICloudCover, true, () => {

            List<UISettings.UIWindowID> needHide = new List<UISettings.UIWindowID>();
            foreach (UISettings.UIWindowID id in showingWindows.Keys)
            {
                if (id != UISettings.UIWindowID.UICloudCover)
                {
                    needHide.Add(id);
                }

            }
            foreach (UISettings.UIWindowID id in needHide)
            {
                GameMainManager.instance.uiManager.CloseWindow(id, false);
            }

           
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UISideBarWindow, false);
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UITopBarWindow, false);
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow, false);
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIMiningMapWindow,false);

            GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UICloudCover);
        });
    }
}
