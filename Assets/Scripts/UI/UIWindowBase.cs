using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
/// <summary>
/// 所有fixed类型的窗口都会被强制拉升固定在界面的四角，要做过度动画使用内部控件或者在外面再套一层
/// </summary>
public abstract class UIWindowBase : MonoBehaviour {

    protected UIWindowData _windowData;
    public abstract UIWindowData windowData
    {
        get;
        
    }

    public bool isOpen
    {
        get
        {
            return gameObject.activeSelf;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	public void ShowWindow(Action onComplate = null,  bool needTransform = true,params object[] data)
    {
        
        gameObject.SetActive(true);
        StartShowWindow(data);
        if(needTransform)
        {
            GameMainManager.instance.uiManager.DisableOperation();
            EnterAnimation(() => {
                onComplate();
                GameMainManager.instance.uiManager.EnableOperation();
            });
        }else
        {
            onComplate();
        }
        
    }

    public void HideWindow(Action onComplate = null, bool needTransform = true)
    {
        StartHideWindow();
        if(needTransform)
        {
            GameMainManager.instance.uiManager.DisableOperation();
            ExitAnimation(() => {
                
                GameMainManager.instance.uiManager.EnableOperation();
                onComplate();
                gameObject.SetActive(false);
            });
        }else
        {
            onComplate();
            gameObject.SetActive(false);
        }
        
    }

    protected virtual void StartShowWindow(object[] data)
    {

    }

    protected virtual void StartHideWindow()
    {

    }

    protected virtual void EnterAnimation(Action onComplete)
    {
        if(windowData.type == UISettings.UIWindowType.Fixed)
        {
            onComplete();
            return;
        }

        transform.localScale = new Vector3(0.5f,0.5f,1);
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(()=> 
        {
            onComplete();
        });

    }

    protected  virtual void ExitAnimation(Action onComplete)
    {
        if (windowData.type == UISettings.UIWindowType.Fixed)
        {
            onComplete();
            return;
        }

        transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f).SetEase(Ease.InBack).OnComplete(()=>
        {
            onComplete();
        });
    }

    public void OnClickClose()
    {
        GameMainManager.instance.uiManager.CloseWindow(windowData.id);
    }
}
