using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class UIModalBoxWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIModalBox;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
            }
            return _windowData;
        }
    }

    public TextMeshProUGUI text;
    public TextMeshProUGUI okBtnLabel;
    public TextMeshProUGUI cancelBtnLabel;

    private ModalBoxData modalData;
    protected override void StartShowWindow(object[] data)
    {
        modalData = data[0] as ModalBoxData;
        text.text = modalData.content;
        text.alignment = modalData.alignment;
        okBtnLabel.text = string.IsNullOrEmpty(modalData.okName) ? "确认" : modalData.okName;
        cancelBtnLabel.text = string.IsNullOrEmpty(modalData.cancelName) ? "取消" : modalData.cancelName;

        if ((modalData.flags & Alert.OK) == Alert.OK)
        {
            okBtnLabel.transform.parent.gameObject.SetActive(true);
        }else
        {
            okBtnLabel.transform.parent.gameObject.SetActive(false);
        }

        if ((modalData.flags & Alert.CANCEL) == Alert.CANCEL)
        {
            cancelBtnLabel.transform.parent.gameObject.SetActive(true);
        }else
        {
            cancelBtnLabel.transform.parent.gameObject.SetActive(false);
        }
    }

    protected override void StartHideWindow()
    {
        modalData = null;
    }

    public void OnClickOKBtn()
    {
        if (modalData.onClick != null)
        {
            modalData.onClick(Alert.OK);
        }
        OnClickClose();
    }

    public void OnClickCancelBtn()
    {
        if (modalData.onClick != null)
        {
            modalData.onClick(Alert.CANCEL);
        }
        OnClickClose();
    }
}


