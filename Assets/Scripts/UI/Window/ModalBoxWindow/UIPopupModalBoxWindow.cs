using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPopupModalBoxWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIPopupModalBoxWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
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
        okBtnLabel.text = string.IsNullOrEmpty(modalData.okName) ? "确认" : modalData.okName;
        cancelBtnLabel.text = string.IsNullOrEmpty(modalData.cancelName) ? "取消" : modalData.cancelName;
    }

    protected override void StartHideWindow()
    {
        modalData = null;
    }

    public void OnClickOKBtn()
    {
        if (modalData.onClick != null)
        {
            modalData.onClick(true);
        }
        OnClickClose();
    }

    public void OnClickCancelBtn()
    {
        if (modalData.onClick != null)
        {
            modalData.onClick(false);
        }
        OnClickClose();
    }
}
