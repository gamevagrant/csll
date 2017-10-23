

public interface IUIManager {

    void OpenWindow(UISettings.UIWindowID id,bool needTransform=true,params object[] data);
    void OpenWindow(UISettings.UIWindowID id, params object[] data);
    void CloseWindow(UISettings.UIWindowID id, bool needTransform = true);
    void ChangeState(UIStateChangeBase state);
    void EnableOperation();
    void DisableOperation();
}
