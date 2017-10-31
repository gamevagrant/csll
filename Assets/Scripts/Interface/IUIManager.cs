

public interface IUIManager {

    void OpenWindow(UISettings.UIWindowID id,bool needTransform=true,params object[] data);
    void OpenWindow(UISettings.UIWindowID id, params object[] data);
    void CloseWindow(UISettings.UIWindowID id, bool needTransform = true);
    void ChangeState(UIStateChangeBase state);
    /// <summary>
    /// 打开只有一个确认按钮的模态窗口 
    /// </summary>
    /// <param name="content">显示内容</param>
    /// <param name="okBtnName">确认键显示文字</param>
    /// <param name="onClickOKBtn">确认键点击回调</param>
    void OpenModalBoxWindow(string content, string okBtnName = "", System.Action onClickOKBtn = null);
    void EnableOperation();
    void DisableOperation();
}
