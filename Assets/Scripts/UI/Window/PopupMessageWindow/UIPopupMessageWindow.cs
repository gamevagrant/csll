using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIPopupMessageWindow : UIWindowBase {

    public RawImage headImage;
    public TextMeshProUGUI contentLabel;

    private RectTransform rectTransform;
    private Queue<PopupMessageData> queue;

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIPopupMessageWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.Transparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;

            }
            return _windowData;
        }
    }

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        queue = new Queue<PopupMessageData>();
    }

    protected override void StartShowWindow(object[] data)
    {
        PopupMessageData msg = data[0] as PopupMessageData;
        queue.Enqueue(msg);
        if (queue.Count == 1)
        {
            rectTransform.anchoredPosition = new Vector2(-700, 60);
            ActionNextMsg();
        }
        
    }

    protected override void StartHideWindow()
    {
        base.StartHideWindow();
    }

    protected override void EnterAnimation(Action onComplete)
    {
        onComplete();
    }

    protected override void ExitAnimation(Action onComplete)
    {
        onComplete();
    }

    public void OnClickOKBtn()
    {
        ConfirmMsg();
    }
    private void ConfirmMsg()
    {
        Hide(()=> {
            if(queue.Count>0)
            {
                queue.Dequeue();
                ActionNextMsg();
            }
        });
       
    }

    private void ActionNextMsg()
    {
        if(queue.Count>0)
        {
            PopupMessageData msg = queue.Peek();

            AssetLoadManager.Instance.LoadAsset<Texture2D>(msg.headImg, (tex) =>
            {
                headImage.texture = tex;
            });

            contentLabel.text = msg.content;
            Show();
        }
        else
        {
            OnClickClose();
        }

    }

    private void Show()
    {
        rectTransform.DOAnchorPos(new Vector2(0, 60), 0.5f).SetEase(Ease.OutBack);
    }

    private void Hide(Action onComplate)
    {
        rectTransform.DOAnchorPos(new Vector2(-700, 60), 0.5f).SetEase(Ease.OutBack).OnComplete(()=> {

            onComplate();
        });
        
        
    }
}

public class PopupMessageData
{
    public string headImg;
    public string content;
    public Action confirmHandle;
}
