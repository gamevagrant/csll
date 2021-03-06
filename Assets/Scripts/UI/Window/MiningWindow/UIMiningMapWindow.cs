﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIMiningMapWindow : UIWindowBase
{

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIMiningMapWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public override bool canOpen
    {
        get
        {
            if (GameMainManager.instance.model.userData.islandId < 3)
            {
                string name = GameMainManager.instance.configManager.islandConfig.GetIslandName(3);
                Alert.Show(string.Format("到达3号岛屿（{0}）后开启地图功能", name));
                return false;
            }
           
            return true;
        }
    }

    public RectTransform content;
    public RectTransform panel;
    public UIMiningMapItem[] items;
    public ScrollRect scrollRect;

    public override void Init()
    {
        panel.sizeDelta = (panel.parent as RectTransform).rect.size;
        panel.anchoredPosition = new Vector2(-Screen.width,0 );
        panel.GetComponent<CanvasGroup>().alpha = 0;
        items = content.transform.GetComponentsInChildren<UIMiningMapItem>();
    }
   
    protected override void StartShowWindow(object[] data)
    {
        GameMainManager.instance.netManager.GetMap((ret,res)=> {});
        int islandID = GameMainManager.instance.model.userData.islandId;
        foreach(UIMiningMapItem item in items)
        {
            if(item.SetData(islandID))
            {
                Vector2 pos = (item.transform as RectTransform).anchoredPosition;
                pos = new Vector2(pos.x * content.localScale.y, pos.y * content.localScale.y);
                content.anchoredPosition = -pos;
                scrollRect.movementType = ScrollRect.MovementType.Elastic;
                
            }

        }
    }

    protected override void EnterAnimation(Action onComplete)
    {
        panel.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);
        panel.GetComponent<CanvasGroup>().DOFade(1, 0.5f).OnComplete(() =>
        {
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        panel.DOAnchorPos(new Vector2(-Screen.width,0 ), 0.5f).SetEase(Ease.OutCubic);
        panel.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
        {
            onComplete();
        });
    }

    public void OnClickMiningBtn()
    {
        GameMainManager.instance.uiManager.ChangeState(new MiningState());
    }
}
