﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using DG.Tweening;
using QY.UI;

public class UIShopWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIShopWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public Toggle energyToggle;
    public Toggle goldToggle;
    public Toggle propsToggle;

    public RectTransform TopBar;
    public RectTransform energyPanel;
    public RectTransform goldPanel;
    public RectTransform propsPanel;

    public GridBaseScrollView energyScrollView;
    public GridBaseScrollView goldScrollView;
    public UIShopPropsPanel propsShopPanel;


    private RectTransform currPanel;
    private GoodsListData goodsList;

    private void Awake()
    {
		if(GameMainManager.instance.iap==null)
		{
			GameMainManager.instance.iap = new IAPManager ();
		}

        energyToggle.onValueChanged.AddListener(OnEnergyToggleValueChange);
        goldToggle.onValueChanged.AddListener(OnGoldToggleValueChange);
        propsToggle.onValueChanged.AddListener(OnPropsToggleValueChange);
    }

    private void OnDestroy()
    {
        energyToggle.onValueChanged.RemoveAllListeners();
        goldToggle.onValueChanged.RemoveAllListeners();
        propsToggle.onValueChanged.RemoveAllListeners();
    }

    protected override void StartShowWindow(object[] data)
    {
        GameMainManager.instance.netManager.ShopList((ret, res) =>
        {
            goodsList = res.goods;
            energyScrollView.SetData (goodsList.energy);
            goldScrollView.SetData (goodsList.money);
            propsShopPanel.SetData(goodsList.prop);
        });

        energyPanel.gameObject.SetActive(false);
        goldPanel.gameObject.SetActive(false);
        propsPanel.gameObject.SetActive(false);
        if (data!=null && data.Length>0 && data[0]!=null && data[0] is ShowShopWindowData)
        {
            ShowShopWindowData showData = data[0] as ShowShopWindowData;
            switch(showData.type)
            {
                case ShowShopWindowData.PanelType.Energy:
                    energyToggle.isOn = true;
                    currPanel = energyPanel;
                    energyPanel.gameObject.SetActive(true);
                    break;
                case ShowShopWindowData.PanelType.Gold:
                    goldToggle.isOn = true;
                    currPanel = goldPanel;
                    goldPanel.gameObject.SetActive(true);
                    break;
                case ShowShopWindowData.PanelType.Props:
                    propsToggle.isOn = true;
                    currPanel = propsPanel;
                    propsPanel.gameObject.SetActive(true);
                    break;
            }
        }else
        {
            energyToggle.isOn = true;
            currPanel = energyPanel;
            energyPanel.gameObject.SetActive(true);
        }

        TopBar.anchoredPosition = new Vector2(0, 160);
        energyPanel.anchoredPosition = new Vector2(0, 950);
        goldPanel.anchoredPosition = new Vector2(0, 950);
        propsPanel.anchoredPosition = new Vector2(0, 950);
       
    }
    protected override void EnterAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        ShowPanel(currPanel);
        sq.Insert(0.2f, TopBar.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic));
        sq.InsertCallback(1, () =>
        {
            onComplete();
        });

    }
    protected override void ExitAnimation(Action onComplete)
    {
       
        Sequence sq = DOTween.Sequence();
        HidePanel(currPanel);
        sq.Insert(0.1f, TopBar.DOAnchorPos(new Vector2(0,160), 0.5f).SetEase(Ease.InCubic));
        sq.InsertCallback(1, () =>
        {
            onComplete();
        });
    }

    private void OnEnergyToggleValueChange(bool value)
    {
        
        if(value)
        {
            currPanel = energyPanel;
            ShowPanel(currPanel);
        }else
        {
            HidePanel(energyPanel);
        }
    }
    private void OnGoldToggleValueChange(bool value)
    {
       
        if (value)
        {
            currPanel = goldPanel;
            ShowPanel(currPanel);
        }
        else
        {
            HidePanel(goldPanel);
        }
    }
    private void OnPropsToggleValueChange(bool value)
    {
        
        if (value)
        {
            currPanel = propsPanel;
            ShowPanel(currPanel);
        }
        else
        {
            HidePanel(propsPanel);
        }
    }

    private void ShowPanel(RectTransform panel)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_in);
        //Sequence sq = DOTween.Sequence();
        panel.gameObject.SetActive(true);
        panel.DOAnchorPos(new Vector2(0, 0), 1f).SetEase(Ease.OutBack);
    }

    private void HidePanel(RectTransform panel)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_out);
        panel.DOAnchorPos(new Vector2(0, 950), 1f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}
