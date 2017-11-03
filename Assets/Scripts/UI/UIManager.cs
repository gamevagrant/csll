using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

/// <summary>
/// UI管理类 
/// </summary>
public class UIManager : MonoBehaviour,IUIManager {

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
    private GameObject popupCollider;//模态窗口的遮挡面板
    private GameObject windowCollider;//全局遮挡面板
    private UIWindowBase curPopUpWindow;//当前打开的窗口

    private int colliderNum = 0;//打开遮挡面板的计数

    private bool openCoilder
    {
        get
        {
            return windowCollider.activeSelf;
        }
        set
        {
            colliderNum += value ? 1 : -1;
            windowCollider.SetActive(colliderNum>0);
        }
    }


    private void Awake()
    {
        //------------------------添加全屏遮挡背板start-----------------------
        windowCollider = GameUtils.createGameObject(FixedRoot.gameObject, "WindwCollider");
        RectTransform rt = windowCollider.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        windowCollider.AddComponent<Image>().color = new Color(0,0,0,0);
        windowCollider.SetActive(false);
        //------------------------添加全屏遮挡背板end-----------------------
        //-------------------------添加模态窗口的背板start--------------------
        popupCollider = GameUtils.createGameObject(PopUpRoot.gameObject, "popupCollider");
        rt = popupCollider.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        popupCollider.AddComponent<Image>().color = new Color(0,0,0,0.4f);

        EventTrigger eventTrigger = popupCollider.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;//设置监听事件类型
        entry.callback.AddListener((evt)=> {
            if(curPopUpWindow != null && curPopUpWindow.windowData.colliderMode == UISettings.UIWindowColliderMode.TouchClose)
            {
                CloseWindow(curPopUpWindow.windowData.id);
            }
        });
        eventTrigger.triggers.Add(entry);
        popupCollider.SetActive(false);
        //---------------添加模态窗口的背板end----------------------------

        Init();


    }


    private void Start()
    {
        //Init();
    }
    private void Init()
    {
        allWindows = new Dictionary<UISettings.UIWindowID, UIWindowBase>();
        showingWindows = new Dictionary<UISettings.UIWindowID, UIWindowBase>();
        backSequence = new Stack<UIWindowBase>();

        UIWindowBase[] windows = transform.GetComponentsInChildren<UIWindowBase>(true);
        foreach(UIWindowBase window in windows)
        {
            allWindows.Add(window.windowData.id, window);
            window.HideWindow(null, false);
        }

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        Canvas canvas = GetComponent<Canvas>();

        SpriteAtlasManager.atlasRequested += (tag, act) => {
            Debug.Log("开始加载[" + tag + "]图集");
            string path = FilePathTools.getSpriteAtlasPath(tag);
            AssetBundleLoadManager.Instance.LoadAsset<SpriteAtlas>(path, (sa) => {

                act(sa);
               

                canvasScaler.enabled = false;
                canvas.enabled = false;

                canvasScaler.enabled = true;
                canvas.enabled = true;
                Debug.Log("图集加载完毕：" + sa);
            });
        };
    }
    public void OpenWindow(UISettings.UIWindowID id, params object[] data)
    {
        OpenWindow(id,true,data);
    }
    public void OpenWindow(UISettings.UIWindowID id,bool needTransform = true,params object[] data)
    {
        UIWindowBase window;
        allWindows.TryGetValue(id, out window);
        if(window!=null)
        {
            UIWindowData windowdata = window.windowData;
            if(windowdata.type == UISettings.UIWindowType.Fixed)
            {
                
            }
            else if(windowdata.type == UISettings.UIWindowType.PopUp)
            {
                curPopUpWindow = window;
                popupCollider.transform.SetSiblingIndex(0);
                popupCollider.SetActive(true);
                //popupCollider.GetComponent<Image>().color = new Color(0.8f,0.8f,0.8f,0.5f);
            }
            else if(windowdata.type == UISettings.UIWindowType.Normal)
            {
                showNavigationWindow(window);
               
            }

            
            if (!showingWindows.ContainsKey(id))
            {
                showingWindows.Add(id,window);
            }
            window.ShowWindow(()=> 
            {

            },needTransform, data);
        }
        else
        {
            loadWindow(id, needTransform,data);
        }
    }

    public void CloseWindow(UISettings.UIWindowID id,bool needTransform = true)
    {
        UIWindowBase window;
        allWindows.TryGetValue(id, out window);
        if (window != null)
        {

           
            if (showingWindows.ContainsKey(id))
            {
                showingWindows.Remove(id);
            }
           
            window.HideWindow(() =>
            {
                UIWindowData windowdata = window.windowData;
                if (windowdata.type == UISettings.UIWindowType.Fixed)
                {

                }
                else if (windowdata.type == UISettings.UIWindowType.PopUp)
                {
                    popupCollider.SetActive(false);
                    curPopUpWindow = null;

                }
                else if (windowdata.type == UISettings.UIWindowType.Normal)
                {
                    hideNavigationWindow(window);
                }
            },needTransform);

            
        }
       
    }

    /// <summary>
    /// 打开只有一个确认按钮的模态窗口 
    /// </summary>
    /// <param name="content">显示内容</param>
    /// <param name="okBtnName">确认键显示文字</param>
    /// <param name="onClickOKBtn">确认键点击回调</param>
    public void OpenModalBoxWindow(string content,string okBtnName = "",System.Action onClickOKBtn = null)
    {
        UIModalBoxWindow.ModalBoxData data = new UIModalBoxWindow.ModalBoxData();
        data.content = content;
        data.okName = okBtnName;
        data.onClickOK = onClickOKBtn;
        OpenWindow(UISettings.UIWindowID.UIModalBoxWindow, data);
    }

    public void ChangeState(UIStateChangeBase state)
    {
        state.ChangeState(showingWindows);
    }

    /// <summary>
    /// 允许界面操作
    /// </summary>
    public void EnableOperation()
    {
        openCoilder = false;
    }
    /// <summary>
    /// 禁止界面操作
    /// </summary>
    public void DisableOperation()
    {
        //windowCollider.GetComponent<Image>().color = new Color(0,0,0,0);
        openCoilder = true;
        windowCollider.transform.SetSiblingIndex(windowCollider.transform.parent.childCount);
    }



    private void loadWindow(UISettings.UIWindowID id, bool needTransform = true, params object[] data)
    {
        string path = FilePathTools.getUIPath(UISettings.getWindowName(id));
        AssetBundleLoadManager.Instance.LoadAsset<GameObject>(path,(go)=> {

            UIWindowBase window;
            allWindows.TryGetValue(id, out window);
            if (window == null)
            {
                GameObject windowGO = GameObject.Instantiate(go);
                windowGO.SetActive(false);
                window = windowGO.GetComponent<UIWindowBase>();
                UIWindowData windowData = window.windowData;
                if (windowData.id != id)
                {
                    Debug.LogError("加载的window id和目标id不符");
                }
                allWindows.Add(windowData.id, window);
            }

            
            StartCoroutine(DelayOpen(id,needTransform,data,0.5f));
        });
    }

    IEnumerator DelayOpen(UISettings.UIWindowID id,bool needTransform,object[] data,float delay)
    {
        
        UIWindowBase window = allWindows[id];


        switch (window.windowData.type)
        {
            case UISettings.UIWindowType.Fixed:
                //window.gameObject.transform.parent = FixedRoot;
                window.gameObject.transform.SetParent(FixedRoot);
                RectTransform rt = window.transform as RectTransform;
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.one;
                break;
            case UISettings.UIWindowType.Normal:
                window.transform.SetParent(NormalRoot);
                break;
            case UISettings.UIWindowType.PopUp:
                window.transform.SetParent(PopUpRoot);
                break;
        }
        window.transform.localPosition = Vector3.zero;
        window.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(delay);
        OpenWindow(id, needTransform, data);
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
                    preWindow.HideWindow();
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
                backWindow.ShowWindow();

            }
        }
    }
}
