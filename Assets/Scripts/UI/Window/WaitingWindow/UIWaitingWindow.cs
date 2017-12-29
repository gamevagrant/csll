using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIWaitingWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIWaitingWindow;
                _windowData.type = UISettings.UIWindowType.Cover;
            }
            return _windowData;
        }
    }

    private CanvasGroup canvasGroup;
    private float timeTag;
    private float time;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        time = Time.time - timeTag;
        if (time>2)
        {
            canvasGroup.alpha = (time-1.5f)*1.5f;
        }
    }

    protected override void StartShowWindow(object[] data)
    {
        timeTag = Time.time;
        canvasGroup.alpha = 0;
    }

    protected override void EnterAnimation(Action onComplete)
    {
        onComplete();
    }

    protected override void ExitAnimation(Action onComplete)
    {
        onComplete();
    }
}
