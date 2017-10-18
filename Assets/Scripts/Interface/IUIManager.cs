

public interface IUIManager {

    void OpenWindow(UISettings.UIWindowID id,bool needTransform=true, object data = null);
    void CloseWindow(UISettings.UIWindowID id, bool needTransform = true);
    void EnableOperation();
    void DisableOperation();
}
