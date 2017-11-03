using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIModalBoxWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIModalBoxWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.NormalNavigation;
            }

            return _windowData;
        }
    }

    public TextMeshProUGUI text;
    public TextMeshProUGUI okBtnLabel;
    private ModalBoxData modalData;
    private RectTransform rectTransform;

    protected override void StartShowWindow(object[] data)
    {
        modalData = data[0] as ModalBoxData;
        text.text = modalData.content;
        okBtnLabel.text = string.IsNullOrEmpty(modalData.okName) ? "确认" : modalData.okName;
        rectTransform = transform as RectTransform;
        rectTransform.anchoredPosition = new Vector2(0, -350);
    }

    protected override void EnterAnimation(Action onComplete)
    {
 
        rectTransform.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutQuint).OnComplete(()=> {

            onComplete();
        });

    }

    protected override void ExitAnimation(Action onComplete)
    {
        rectTransform.DOAnchorPos(new Vector2(0,-350), 0.5f).SetEase(Ease.OutQuint).OnComplete(() => {

            onComplete();
        });
    }

    public void OnClickOK()
    {
        if(modalData.onClickOK != null)
        {
            modalData.onClickOK();
        }
        OnClickClose();
    }

    public class ModalBoxData
    {
        public string content;
        public string okName;
        public Action onClickOK;
    }

}
