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

    /// <summary>
    /// 第一次创建的时候会被调用
    /// </summary>
    public virtual void Init()
    {
        transform.localScale = Vector3.zero;
    }

    public IEnumerator ShowWindow(Action onComplate = null,  bool needTransform = true,params object[] data)
    {
        //transform.SetSiblingIndex(100);
        gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        transform.localScale = Vector3.one;
        StartShowWindow(data);
        if(needTransform)
        {
            GameMainManager.instance.uiManager.DisableOperation();
            EnterAnimation(() => {
                if (onComplate != null)
                {
                    onComplate();
                }
                GameMainManager.instance.uiManager.EnableOperation();
            });
        }else
        {
            EndShowWindow();
            if (onComplate != null)
            {
                onComplate();
            }
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
                gameObject.SetActive(false);
                if (onComplate != null)
                {
                    onComplate();
                }
               
            });
        }else
        {
            EndHideWindow();
            if (onComplate != null)
            {
                onComplate();
            }
           
            gameObject.SetActive(false);
        }
        
    }


    /// <summary>
    /// 每次打开窗口的时候会被首先调用
    /// </summary>
    /// <param name="data"></param>
    protected virtual void StartShowWindow(object[] data)
    {

    }
    /// <summary>
    /// 每次关闭窗口的时候会被首先调用
    /// </summary>
    protected virtual void StartHideWindow()
    {

    }
    /// <summary>
    /// 缓动动画结束，或者不使用缓动动画时 ，会调用此方法用以将UI设置成最终显示状态
    /// </summary>
    protected virtual void EndShowWindow()
    {

    }
    protected virtual void EndHideWindow()
    {

    }
    /// <summary>
    /// 打开窗口动画的时候调用，重写打开动画
    /// </summary>
    /// <param name="onComplete"></param>
    protected virtual void EnterAnimation(Action onComplete)
    {
        if(windowData.type == UISettings.UIWindowType.Fixed)
        {
            onComplete();
            return;
        }

        transform.localScale = new Vector3(0.2f,0.2f,0.2f);
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(()=> 
        {
            onComplete();
        });

    }
    /// <summary>
    /// 展示关闭窗口动画时调用，重写关闭动画
    /// </summary>
    /// <param name="onComplete"></param>
    protected  virtual void ExitAnimation(Action onComplete)
    {
        if (windowData.type == UISettings.UIWindowType.Fixed)
        {
            onComplete();
            return;
        }

        transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).SetEase(Ease.InBack).OnComplete(()=>
        {
            onComplete();
        });
    }

    public virtual void OnClickClose()
    {
        GameMainManager.instance.uiManager.CloseWindow(windowData.id);
    }
}
