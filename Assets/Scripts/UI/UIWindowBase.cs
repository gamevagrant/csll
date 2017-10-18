using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

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
	
	public void showWindow(Action onComplate = null,bool needTransform = true)
    {
        gameObject.transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        startShowWindow();
        if(needTransform)
        {
            enterAnimation(() => {
                onComplate();
            });
        }else
        {
            onComplate();
        }
        
    }

    public void hideWindow(Action onComplate = null, bool needTransform = true)
    {
        startHideWindow();
        if(needTransform)
        {
            exitAnimation(() => {
                onComplate();
                gameObject.SetActive(false);
            });
        }else
        {
            onComplate();
        }
        
    }

    protected virtual void startShowWindow()
    {

    }

    protected virtual void startHideWindow()
    {

    }

    protected virtual void enterAnimation(Action onComplete)
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

    protected  virtual void exitAnimation(Action onComplete)
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
