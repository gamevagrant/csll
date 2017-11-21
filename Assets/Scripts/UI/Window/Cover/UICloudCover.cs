using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UICloudCover :UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UICloudCover;
                _windowData.type = UISettings.UIWindowType.Cover;
            }
            return _windowData;
        }
    }

    public RectTransform left;
    public RectTransform right;

    protected override void StartShowWindow(object[] data)
    {
        left.anchoredPosition = new Vector2(-560, 0);
        right.anchoredPosition = new Vector2(560, 0);
    }

    protected override void EnterAnimation(Action onComplete)
    {
        left.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);
        right.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic).OnComplete(()=> {
            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        left.DOAnchorPos(new Vector2(-560, 0), 0.5f).SetEase(Ease.InCubic);
        right.DOAnchorPos(new Vector2(560, 0), 0.5f).SetEase(Ease.InCubic).OnComplete(() => {
            onComplete();
        });
    }

}
