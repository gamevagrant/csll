using System;

public class Alert
{

    public const uint OK = 1;
    public const uint CANCEL = 2;
    /// <summary>
    /// 打开只有一个确认按钮的弹出确认框
    /// </summary>
    /// <param name="content">显示内容</param>
    /// <param name="okBtnName">确认键显示文字</param>
    /// <param name="onClickOKBtn">确认键点击回调</param>
    public static void ShowPopupBox(string content, Action onClickOKBtn = null, string okBtnName = "")
    {
        ModalBoxData data = new ModalBoxData();
        data.content = content;
        data.okName = okBtnName;
        data.onClick = (flags) =>
        {
            if (flags == OK && onClickOKBtn != null)
            {
                onClickOKBtn();
            }
        };
       GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIPopupModalBox, data);
    }
    /// <summary>
    /// 打开一个有确认和取消按钮的模态窗口 
    /// </summary>
    /// <param name="content">内容</param>
    /// <param name="flags">控件中放置的按钮。有效值为 Alert.OK、Alert.CANCEL默认值为 Alert.OK。使用按位 OR 运算符可显示多个按钮。例如，传递 (Alert.OK | Alert.CANCEL) 显示“确认”和“取消”按钮。无论按怎样的顺序指定按钮，它们始终按照以下顺序从左到右显示：“确定”，“取消”。
    /// <param name="onClickBtn">点击按钮的回调，Alert.OK 或者 Alert.CANCEL的值</param>
    /// <param name="okBtnName"></param>
    /// <param name="cancelBtnName"></param>
    public static void Show(string content, uint flags = Alert.OK, Action<uint> onClickBtn = null, string okBtnName = "", string cancelBtnName = "")
    {
        ModalBoxData data = new ModalBoxData();
        data.content = content;
        data.flags = flags;
        data.okName = okBtnName;
        data.cancelName = cancelBtnName;
        data.onClick = onClickBtn;
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIModalBox, data);
    }
}

public class ModalBoxData
{
    public string content;
    public uint flags = Alert.OK;
    public string okName = "确认";
    public string cancelName = "取消";
    public Action<uint> onClick = null;
}
