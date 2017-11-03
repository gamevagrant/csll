using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIMessageMailWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIMessageMailWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public Toggle messageToggle;
    public Toggle mailToggle;
    public RectTransform messagePanel;
    public RectTransform mailPanel;
    public DynamicScrollView messageScrollView;
    public DynamicScrollView mailScrollView;

    public RectTransform topBar;

    private MessageResponseData[] messages;
    private MailData[] mails;
    private RectTransform currentPanel;

    private void Awake()
    {
        topBar.anchoredPosition = new Vector2(0, 150);
        messagePanel.anchoredPosition = new Vector2(0, 1000);
        mailPanel.anchoredPosition = new Vector2(0, 1000);

        messageToggle.onValueChanged.AddListener(OnChangeMessageToggle);
        mailToggle.onValueChanged.AddListener(OnChangeMailToggle);
    }

    protected override void StartShowWindow(object[] data)
    {
        messages = GameMainManager.instance.model.userData.messages;
        mails = GameMainManager.instance.model.userData.user_mail;
        messageScrollView.setDatas(messages);
        
        messageToggle.isOn = true;
        mailToggle.enabled = false;
        mailToggle.enabled = true;
    }

    protected override void StartHideWindow()
    {
        base.StartHideWindow();
    }

    protected override void EnterAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        ShowPanel(messagePanel);
        sq.Insert(0.2f, topBar.DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.OutCubic)).OnComplete(() => {
            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        HidePanel(currentPanel);
        sq.Insert(0.1f, topBar.DOAnchorPos(new Vector2(0, 150), 0.5f).SetEase(Ease.InCubic));
        sq.AppendInterval(0.5f).OnComplete(() =>
        {
            onComplete();
        });
    }

    // Use this for initialization
    public void OnChangeMessageToggle(bool value)
    {
        if (value)
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
            messageScrollView.setDatas(messages);
            ShowPanel(messagePanel);
        }
        else if (!messageToggle.isOn)
        {
            HidePanel(currentPanel);
        }
    }
    public void OnChangeMailToggle(bool value)
    {
        if (value)
        {
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
            mailScrollView.setDatas(mails);
            ShowPanel(mailPanel);
        }
        else if (!mailToggle.isOn)
        {
            HidePanel(currentPanel);
        }

    }

    public void OnClickClosePanelBtn()
    {
        OnClickClose();
    }


    private void ReSetToggle()
    {
        messageToggle.isOn = false;
        messageToggle.enabled = false;
        messageToggle.enabled = true;

        mailToggle.isOn = false;
        mailToggle.enabled = false;
        mailToggle.enabled = true;
    }
    private void ShowPanel(RectTransform panel)
    {
        currentPanel = panel;
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_in);
        panel.gameObject.SetActive(true);
        panel.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBack);
    }
    private void HidePanel(RectTransform panel)
    {
        currentPanel = null;
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_out);
        panel.DOAnchorPos(new Vector2(0, 1000), 1f).SetEase(Ease.OutCubic).OnComplete(() => {
            panel.gameObject.SetActive(false);
        });
    }

   
}
