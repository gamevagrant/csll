using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using System;

/// <summary>
/// UI管理类 
/// </summary>
public class UIManager : MonoBehaviour,IUIManager  {

    [SerializeField]
    private Transform FixedRoot;//固定UI根节点
    [SerializeField]
    private Transform NormalRoot;//普通窗口根节点
    [SerializeField]
    private Transform PopUpRoot;//模态窗口UI 根节点
    [SerializeField]
    private Transform coverRoot;//覆盖界面UI节点

    private Dictionary<UISettings.UIWindowID, UIWindowBase> allWindows;
    private Dictionary<UISettings.UIWindowID, UIWindowBase> showingWindows;
    private Dictionary<UISettings.UIWindowID, bool> windowsState;//窗口加载完的状态 时打开还是关闭 避免窗口还没加载完就关闭但是窗口仍是开着的情况


    protected Stack<UIWindowBase> backSequence;//导航窗口的堆栈

    private UIWindowData curNavWindow;//当前导航窗口
    private UIWindowBase curPopUpWindow;//当前打开的窗口
    private GameObject popupCollider;//模态窗口的遮挡面板
    //private GameObject windowCollider;//全局遮挡面板

    private int colliderNum = 0;//打开遮挡面板的计数
    private Queue<Action> queue = new Queue<Action>();
    private bool isOpening = false;

    private CanvasScaler canvasScaler;
    private Canvas canvas;

    public bool isEnable
    {
        get
        {
            return colliderNum == 0;
        }
    }

    public bool isWaiting
    {
        get
        {
            if(showingWindows.ContainsKey(UISettings.UIWindowID.UIWaitingWindow))
            {
                return true;
            }else
            {
                return false;
            }
            
        }
        set
        {
            if(value)
            {
                OpenWindow(UISettings.UIWindowID.UIWaitingWindow);
            }else
            {
                CloseWindow(UISettings.UIWindowID.UIWaitingWindow);
            }
            
        }
    }

    public UIWindowBase curWindow
    {
        get
        {
            for(int i = PopUpRoot.childCount - 1; i>=0;i--)
            {
                Transform tf = PopUpRoot.GetChild(i);
                if (tf != null && tf.gameObject.activeSelf)
                {
                    UIWindowBase window = tf.GetComponent<UIWindowBase>();
                    if(window!= null)
                    {
                        return window;
                    }
                   
                }
            }

            for (int i = FixedRoot.childCount - 1; i >= 0; i--)
            {
                Transform tf = FixedRoot.GetChild(i);
                if (tf != null && tf.gameObject.activeSelf)
                {
                    UIWindowBase window = tf.GetComponent<UIWindowBase>();
                    if (window != null)
                    {
                        return window;
                    }
                }
            }

            return null;
        }
    }
    /*
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
    */


    private void Awake()
    {
        //------------------------添加全屏遮挡背板start-----------------------
      /*  windowCollider = GameUtils.createGameObject(FixedRoot.gameObject, "WindwCollider");
        RectTransform rt = windowCollider.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        windowCollider.AddComponent<Image>().color = new Color(0,0,0,0);
        windowCollider.SetActive(false);
        */
        //------------------------添加全屏遮挡背板end-----------------------
        //-------------------------添加模态窗口的背板start--------------------
        popupCollider = GameUtils.createGameObject(PopUpRoot.gameObject, "popupCollider");
        RectTransform rt = popupCollider.AddComponent<RectTransform>();
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
    //GameObject selectedGO;
    private void Update()
    {
        if(queue.Count > 0 && !isOpening)
        {
            Action act = queue.Dequeue();
            if (act != null)
            {
                act();
            }
        }

    }

    private void OnDestroy()
    {
        SpriteAtlasManager.atlasRequested -= OnLoadAtlas;
    }

    private void Init()
    {
        allWindows = new Dictionary<UISettings.UIWindowID, UIWindowBase>();
        showingWindows = new Dictionary<UISettings.UIWindowID, UIWindowBase>();
        windowsState = new Dictionary<UISettings.UIWindowID, bool>();
        backSequence = new Stack<UIWindowBase>();

        UIWindowBase[] windows = transform.GetComponentsInChildren<UIWindowBase>(true);
        foreach(UIWindowBase window in windows)
        {
            allWindows.Add(window.windowData.id, window);
            window.HideWindow(null, false);
        }

        canvasScaler = GetComponent<CanvasScaler>();
        canvas = GetComponent<Canvas>();

        SpriteAtlasManager.atlasRequested += OnLoadAtlas;
    }

    private void OnLoadAtlas(string tag,Action<SpriteAtlas> act)
    {
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
    }

    public void OpenWindow(UISettings.UIWindowID id, params object[] data)
    {
        OpenWindow(id,true,data);
    }
    public void OpenWindow(UISettings.UIWindowID id, bool needTransform, params object[] data)
    {
        OpenWindow(id, needTransform, null, data);
    }
    public void OpenWindow(UISettings.UIWindowID id,bool needTransform,System.Action onComplate,params object[] data)
    {
        SetWindowState(id, true);
        queue.Enqueue(() =>
        {
            StartOpenWindow(id, needTransform, onComplate, data);
        });
    }

    public void CloseWindow(UISettings.UIWindowID id)
    {
        CloseWindow(id,true);
    }

    public void CloseWindow(UISettings.UIWindowID id,bool needTransform = true, System.Action onComplate = null)
    {
        SetWindowState(id, false);
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
                    curPopUpWindow = null;
                    popupCollider.SetActive(false);
                    for (int i = PopUpRoot.childCount-1;i>=0;i--)
                    {
                        Transform tf = PopUpRoot.GetChild(i);
                        if (tf.gameObject.activeSelf)
                        {
                            UIWindowBase wd = tf.GetComponent<UIWindowBase>();
                            if(wd != null)
                            {
                                popupCollider.SetActive(true);
                                curPopUpWindow = wd;
                                popupCollider.transform.SetSiblingIndex(Mathf.Max(0, i-1));
                                
                                break;
                            }
                        }
                    }

                }
                else if (windowdata.type == UISettings.UIWindowType.Normal)
                {
                    hideNavigationWindow(window);
                }
                if(onComplate != null)
                {
                    onComplate();
                }
            },needTransform);

            
        }
       
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
        //openCoilder = false;
        QY.UI.Interactable.isLock = false;
    }
    /// <summary>
    /// 禁止界面操作
    /// </summary>
    public void DisableOperation()
    {

        //openCoilder = true;
        //windowCollider.transform.SetSiblingIndex(windowCollider.transform.parent.childCount);

        QY.UI.Interactable.isLock = true;
    }

    private void StartOpenWindow(UISettings.UIWindowID id, bool needTransform = true, System.Action onComplate = null, params object[] data)
    {
        isOpening = true;
        UIWindowBase window;
        allWindows.TryGetValue(id, out window);
        if (window != null)
        {
            UIWindowData windowdata = window.windowData;

            if (windowdata.type == UISettings.UIWindowType.Fixed)
            {
                window.transform.SetSiblingIndex(FixedRoot.childCount);
            }
            else if (windowdata.type == UISettings.UIWindowType.PopUp)
            {
                curPopUpWindow = window;
                window.transform.SetSiblingIndex(PopUpRoot.childCount);
                popupCollider.transform.SetSiblingIndex(PopUpRoot.childCount - 2);
                popupCollider.SetActive(true);
                if(window.windowData.colliderType == UISettings.UIWindowColliderType.Transparent)
                {
                    popupCollider.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 0f);
                }
                else
                {
                    popupCollider.GetComponent<Image>().color = new Color(0, 0, 0, 0.3f);
                }
                //popupCollider.GetComponent<Image>().color = new Color(0.8f,0.8f,0.8f,0.5f);
            }
            else if (windowdata.type == UISettings.UIWindowType.Normal)
            {
                window.transform.SetSiblingIndex(NormalRoot.childCount);
                showNavigationWindow(window);

            }else if(windowdata.type == UISettings.UIWindowType.Cover)
            {
                window.transform.SetSiblingIndex(coverRoot.childCount);
            }


            if (!showingWindows.ContainsKey(id))
            {
                showingWindows.Add(id, window);
            }
            isOpening = false;
            window.ShowWindow(() =>
            {
                if (onComplate != null)
                {
                    onComplate();
                }
            }, needTransform, data);
        }
        else
        {
            string path = FilePathTools.getUIPath(UISettings.getWindowName(id));
            AssetBundleLoadManager.Instance.LoadAsset<GameObject>(path, (go) =>
            {
                StartCoroutine(ConfigureWindow(go,id, needTransform, onComplate, data));
            });
 
        }
    }

    private IEnumerator ConfigureWindow(GameObject go,UISettings.UIWindowID id, bool needTransform = true,System.Action onComplate = null, params object[] data)
    {
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

        RectTransform rt = window.transform as RectTransform;
        switch (window.windowData.type)
        {
            case UISettings.UIWindowType.Fixed:
                window.gameObject.transform.SetParent(FixedRoot);
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
            case UISettings.UIWindowType.Cover:
                window.transform.SetParent(coverRoot);
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.one;
                break;
        }
        window.transform.localPosition = Vector3.zero;
        window.transform.localScale = Vector3.one;
        //(window.transform as RectTransform).anchoredPosition = Vector2.zero;
        window.Init();
        yield return new WaitForSeconds(0.1f);
        if(windowsState[id])
        {
            StartOpenWindow(id, needTransform, onComplate, data);
        }else
        {
            isOpening = false;
        }
       
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

    private void SetWindowState(UISettings.UIWindowID id ,bool state)
    {
        if (!windowsState.ContainsKey(id))
        {
            windowsState.Add(id, state);
        }
        else
        {
            windowsState[id] = state;
        }
    }
}
