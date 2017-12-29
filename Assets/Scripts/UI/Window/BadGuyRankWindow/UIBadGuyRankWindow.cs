using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIBadGuyRankWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIBadGuyRankWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public RectTransform topBar;
    public RectTransform panel;
    public DynamicScrollView scrollView;
    public TextMeshProUGUI wantedNumText;

    private BadGuyData[] badGuys;

    private void Awake()
    {
        topBar.anchoredPosition = new Vector2(0, 110);
        panel.anchoredPosition = new Vector2(0, 900);

        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_BASE_DATA, OnUpdateUserDataHandle);
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_BASE_DATA, OnUpdateUserDataHandle);
    }

    protected override void StartShowWindow(object[] data)
    {
        wantedNumText.text = GameMainManager.instance.model.userData.wantedCount.ToString();
        GameMainManager.instance.netManager.Enemy((ret, res) =>
        {
            badGuys = res.data.enemies;
            scrollView.setDatas(badGuys);
        });
    }

    protected override void EnterAnimation(Action onComplete)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_in);
        Sequence sq = DOTween.Sequence();
        sq.Append(panel.DOAnchorPos(new Vector2(0, -92), 1f).SetEase(Ease.OutCubic));
        sq.Insert(0.3f, topBar.DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.OutCubic));
        sq.AppendInterval(0.5f).OnComplete(() => {
            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_out);
        Sequence sq = DOTween.Sequence();
        sq.Append(panel.DOAnchorPos(new Vector2(0, 900), 1).SetEase(Ease.OutCubic));
        sq.Insert(0.1f, topBar.DOAnchorPos(new Vector2(0, 110), 0.5f).SetEase(Ease.InCubic)).OnComplete(() => {
            onComplete();
        });
    }

    private void OnUpdateUserDataHandle(BaseEvent e)
    {
        UpdateBaseDataEvent evt = e as UpdateBaseDataEvent;
        if(evt.updateType == UpdateBaseDataEvent.UpdateType.wanted)
        {
            wantedNumText.text = GameMainManager.instance.model.userData.wantedCount.ToString();
        }
       
    }

    public void OnClickBuyWantedBtn()
    {
        OnClickClose();
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIBuyWantedWindow);
    }
}
