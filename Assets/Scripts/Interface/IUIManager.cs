

public interface IUIManager {

    UIWindowBase curWindow{ get; }
    bool isWaiting { get; set; }

    void OpenWindow(UISettings.UIWindowID id, bool needTransform, System.Action onComplate, params object[] data);
    void OpenWindow(UISettings.UIWindowID id,bool needTransform,params object[] data);
    void OpenWindow(UISettings.UIWindowID id, params object[] data);

    void CloseWindow(UISettings.UIWindowID id, bool needTransform = true, System.Action onComplate = null);
    void CloseWindow(UISettings.UIWindowID id);
    void ChangeState(UIStateChangeBase state);
    /// <summary>
    /// 打开只有一个确认按钮的模态窗口 
    /// </summary>
    /// <param name="content">显示内容</param>
    /// <param name="okBtnName">确认键显示文字</param>
    /// <param name="onClickOKBtn">确认键点击回调</param>
    //void OpenPopupModalBox(string content, string okBtnName = "", System.Action onClickOKBtn = null);

    /// <summary>
    /// 打开一个有确认和取消按钮的模态窗口 
    /// </summary>
    /// <param name="content">内容</param>
    /// <param name="flags">控件中放置的按钮。有效值为 ModalBoxData.OK、ModalBoxData.CANCEL默认值为 ModalBoxData.OK。使用按位 OR 运算符可显示多个按钮。例如，传递 (ModalBoxData.OK | ModalBoxData.CANCEL) 显示“确认”和“取消”按钮。无论按怎样的顺序指定按钮，它们始终按照以下顺序从左到右显示：“确定”，“取消”。
    /// <param name="onClickBtn">点击按钮的回调，返回ModalBoxData.OK 或者 ModalBoxData.CANCEL的值</param>
    /// <param name="okBtnName"></param>
    /// <param name="cancelBtnName"></param>
    //void OpenModalBox(string content, uint flags, System.Action<uint> onClickBtn = null, string okBtnName = "", string cancelBtnName = "");
    void EnableOperation();
    void DisableOperation();
}
