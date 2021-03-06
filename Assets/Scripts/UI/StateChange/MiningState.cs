﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningState : UIStateChangeBase {

    
    public MiningState()
    {

    }

    public override void ChangeState(Dictionary<UISettings.UIWindowID, UIWindowBase> showingWindows,bool needTransform = true)
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UICloudCover, true, () => {

            List<UISettings.UIWindowID> needHide = new List<UISettings.UIWindowID>();
            foreach (UISettings.UIWindowID id in showingWindows.Keys)
            {
                if(id != UISettings.UIWindowID.UICloudCover)
                {
                    needHide.Add(id);
                }
              
            }
            foreach (UISettings.UIWindowID id in needHide)
            {
                GameMainManager.instance.uiManager.CloseWindow(id,false);
            }

            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIMiningWindow, false);
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UITopBarWindow, false);

            GameMainManager.instance.mono.StartCoroutine(DelayOpenCloud(0.5f, needTransform));
        });
    }

    private IEnumerator DelayOpenCloud(float delay, bool needTransform)
    {
        yield return new WaitForSeconds(delay);
        GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UICloudCover, needTransform);
    }
}
