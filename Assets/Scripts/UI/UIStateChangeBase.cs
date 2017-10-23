using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateChangeBase {

    /// <summary>
    /// 列表中的窗口最后都会显示在界面上，其他打开的窗口都会被关闭
    /// </summary>
    internal Dictionary<UISettings.UIWindowID,object> needShowWindows;

    /// <summary>
    /// 列表中的窗开在开始时都会进行关闭，即使他存在于needShowWindows列表中
    /// </summary>
    internal UISettings.UIWindowID[] needHideWindows;

    private float delay;

    public UIStateChangeBase(Dictionary<UISettings.UIWindowID, object> needShowWindows, UISettings.UIWindowID[] needHideWindows = null, float delayOpen = 0)
    {
        this.needShowWindows = needShowWindows;
        this.needHideWindows = needHideWindows;
        this.delay = delayOpen;
    }


    public virtual void ChangeState(Dictionary<UISettings.UIWindowID,UIWindowBase> showingWindows)
    {
        Dictionary<UISettings.UIWindowID,string> needHide = new Dictionary<UISettings.UIWindowID, string>();
        foreach(UISettings.UIWindowID showingID in showingWindows.Keys)
        {
            if(!needShowWindows.ContainsKey(showingID))
            {
                needHide.Add(showingID, "");
            }
        }

        if(needHideWindows!= null)
        {
            foreach (UISettings.UIWindowID needHideID in needHideWindows)
            {
                if (!needHide.ContainsKey(needHideID))
                {
                    needHide.Add(needHideID, "");
                }
            }
        }


        foreach(UISettings.UIWindowID id in needHide.Keys)
        {
            GameMainManager.instance.uiManager.CloseWindow(id);
        }


        AssetBundleLoadManager.Instance.StartCoroutine(openWindow(delay));
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
