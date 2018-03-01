using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIDungeonGetKeyWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIDungeonGetKeyWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public RectTransform keyIcon;
    public GameObject title;
    public Image backGround;
    private Vector3 iconPos;

    protected override void StartShowWindow(object[] data)
    {
        GetDungeonKeyEvent evt = new GetDungeonKeyEvent((pos)=> {

            iconPos = pos;
        });
        EventDispatcher.instance.DispatchEvent(evt);

        backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b,0);
        title.SetActive(true);
        keyIcon.localScale = Vector3.one;
        keyIcon.anchoredPosition = Vector2.zero;

        StartCoroutine(startClose());
    }

    private IEnumerator startClose()
    {
        yield return new WaitForSeconds(2);
        OnClickClose();
    }

    protected override void EnterAnimation(Action onComplete)
    {
        backGround.DOFade(0.5f, 0.5f).OnComplete(()=> {

            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        title.SetActive(false);
        Sequence sq = DOTween.Sequence();
        sq.Append(backGround.DOFade(0, 1));
        sq.Insert(0, keyIcon.DOMove(iconPos, 1).SetEase(Ease.OutCubic));
        sq.Insert(0, keyIcon.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f));
        sq.AppendCallback(() =>
        {
            onComplete();
        });
    }

}
