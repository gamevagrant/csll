using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIManager : MonoBehaviour {

    [SerializeField]
    private Transform FixedRoot;//固定UI根节点
    [SerializeField]
    private Transform NormalRoot;//普通窗口根节点
    [SerializeField]
    private Transform PopUpRoot;//模态窗口UI 根节点

    private Dictionary<UISettings.UIWindowID, UIWindowBase> allWindows;
    private Dictionary<UISettings.UIWindowID, UIWindowBase> showingWindows;

    protected Stack<UIWindowBase> backSequence;//导航窗口的堆栈

    private UIWindowData curNavWindow;//当前导航窗口
    private GameObject windowCollider;//模态窗口的遮挡面板
    private UIWindowBase curWindow;//当前打开的窗口

    private void Awake()
    {
        allWindows = new Dictionary<UISettings.UIWindowID, UIWindowBase>();
        showingWindows = new Dictionary<UISettings.UIWindowID, UIWindowBase>();
        backSequence = new Stack<UIWindowBase>();

        //添加模态窗口的背板
        windowCollider = GameUtils.createGameObject(PopUpRoot.gameObject, "PopUpWindwCollider");
        RectTransform rt = windowCollider.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        windowCollider.AddComponent<Image>().color = new Color(0.8f,0.8f,0.8f,0.5f);

        EventTrigger eventTrigger = windowCollider.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;//设置监听事件类型
        entry.callback.AddListener((evt)=> {
            if(curWindow.windowData.type == UISettings.UIWindowType.PopUp && curWindow.windowData.colliderMode == UISettings.UIWindowColliderMode.TouchClose)
            {
                closeWindow(curWindow.windowData.id);
            }
        });
        eventTrigger.triggers.Add(entry);
        windowCollider.SetActive(false);

    }
    private void Start()
    {
        
    }

    public void openWindow(UISettings.UIWindowID id)
    {
        UIWindowBase window;
        allWindows.TryGetValue(id, out window);
        if(window!=null)
        {
            UIWindowData windowdata = window.windowData;
            if(windowdata.type == UISettings.UIWindowType.Fixed)
            {

            }else if(windowdata.type == UISettings.UIWindowType.PopUp)
            {
                windowCollider.SetActive(true);
                
            }
            else if(windowdata.type == UISettings.UIWindowType.Normal)
            {
                showNavigationWindow(window);
               
            }

            curWindow = window;
            if (!showingWindows.ContainsKey(id))
            {
                showingWindows.Add(id,window);
            }

            window.showWindow(()=> 
            {

            });
        }
        else
        {
            loadWindow(id);
        }
    }

    public void closeWindow(UISettings.UIWindowID id)
    {
        UIWindowBase window;
        allWindows.TryGetValue(id, out window);
        if (window != null)
        {
 
            curWindow = null;
            if (showingWindows.ContainsKey(id))
            {
                showingWindows.Remove(id);
            }
           
            window.hideWindow(() =>
            {

            });

            UIWindowData windowdata = window.windowData;
            if (windowdata.type == UISettings.UIWindowType.Fixed)
            {

            }
            else if (windowdata.type == UISettings.UIWindowType.PopUp)
            {
                windowCollider.SetActive(false);

            }
            else if (windowdata.type == UISettings.UIWindowType.Normal)
            {
                hideNavigationWindow(window);
            }
        }
       
    }

    private void loadWindow(UISettings.UIWindowID id)
    {
        string path = FilePathTools.getUIPath(UISettings.getWindowName(id));
        AssetBundleLoadManager.Instance.LoadAsset<GameObject>(path,(go)=> {
            GameObject windowGO = GameObject.Instantiate(go);
            UIWindowBase window = windowGO.GetComponent<UIWindowBase>();
            UIWindowData windowData = window.windowData;
            switch(windowData.type)
            {
                case UISettings.UIWindowType.Fixed:
                    windowGO.transform.parent = FixedRoot;
                    break;
                case UISettings.UIWindowType.Normal:
                    windowGO.transform.parent = NormalRoot;
                    break;
                case UISettings.UIWindowType.PopUp:
                    windowGO.transform.parent = PopUpRoot;
                    break;
            }
            windowGO.transform.localPosition = Vector3.zero;
            if(windowData.id != id)
            {
                Debug.LogError("加载的window id和目标id不符");
            }
            allWindows.Add(windowData.id, window);
            openWindow(id);
        });
    }

    private void showNavigationWindow(UIWindowBase window)
    {
        if (window.windowData.navMode == UISettings.UIWindowNavigationMode.NormalNavigation)
        {
            if (backSequence.Count > 0)
            {
                UIWindowBase preWindow = backSequence.Peek();
                if(preWindow.isOpen)
                {
                    preWindow.hideWindow();
                }
            }
            backSequence.Push(window);
        }
    }

    private void hideNavigationWindow(UIWindowBase window)
    {
        if (window.windowData.navMode == UISettings.UIWindowNavigationMode.NormalNavigation)
        {
            if (backSequence.Pop() != window)
            {
                Debug.LogError("关掉的窗口和退出的窗口不一致");
            }

            if (backSequence.Count > 0)
            {
                UIWindowBase backWindow = backSequence.Peek();
                backWindow.showWindow();

            }
        }
    }
}
