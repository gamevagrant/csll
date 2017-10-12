using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class UIWindowBase : MonoBehaviour {

    public virtual UIWindowData windowData
    {
        get
        {
            return new UIWindowData();
        }
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
	
	public void showWindow(Action onComplate = null)
    {
        gameObject.SetActive(true);
        startShowWindow();
        enterAnimation(()=> {
            onComplate();
        });
    }

    public void hideWindow(Action onComplate = null)
    {
        startHideWindow();
        exitAnimation(()=> {
            onComplate();
            gameObject.SetActive(false);
        });
    }

    protected virtual void startShowWindow()
    {

    }

    protected virtual void startHideWindow()
    {

    }

    private void enterAnimation(Action onComplete)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic).OnComplete(()=> 
        {
            onComplete();
        });

    }

    private void exitAnimation(Action onComplete)
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InElastic).OnComplete(()=>
        {
            onComplete();
        });
    }
}
