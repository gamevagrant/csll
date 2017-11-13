using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainState : UIStateChangeBase
{
    public MainState()
    {
        needShowWindows = new Dictionary<UISettings.UIWindowID, object>();
        needShowWindows.Add(UISettings.UIWindowID.UIWheelWindow, null);
        needShowWindows.Add(UISettings.UIWindowID.UISideBarWindow, null);
        needShowWindows.Add(UISettings.UIWindowID.UITopBarWindow, null);
    }
    public override void ChangeState(Dictionary<UISettings.UIWindowID, UIWindowBase> showingWindows)
    {
        if (showingWindows != null)
        {
            List<UISettings.UIWindowID> needHide = new List<UISettings.UIWindowID>();
            foreach (UISettings.UIWindowID id in showingWindows.Keys)
            {
                needHide.Add(id);
            }
            foreach (UISettings.UIWindowID id in needHide)
            {
                GameMainManager.instance.uiManager.CloseWindow(id);
            }
        }
        GameMainManager.instance.mono.StartCoroutine(openWindow(1));
    }

    IEnumerator openWindow(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (UISettings.UIWindowID id in needShowWindows.Keys)
        {
            GameMainManager.instance.uiManager.OpenWindow(id, needShowWindows[id]);
        }
    }
}
