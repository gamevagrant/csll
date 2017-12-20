using System;
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

    public DynamicScrollView energyScrollView;
    public DynamicScrollView goldScrollView;
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
            energyScrollView.setDatas(goodsList.energy);
            goldScrollView.setDatas(goodsList.money);
            propsShopPanel.SetData(goodsList.prop);
        });

        energyToggle.isOn = true;
        //energyToggle.enabled = false;
        //energyToggle.enabled = true;

        TopBar.anchoredPosition = new Vector2(0, 160);
        //toggleGroup.NotifyToggleOn(energyToggle);
        energyPanel.gameObject.SetActive(true);
        energyPanel.anchoredPosition = new Vector2(0, 950);

        goldPanel.gameObject.SetActive(false);
        goldPanel.anchoredPosition = new Vector2(0, 950);

        propsPanel.gameObject.SetActive(false);
        propsPanel.anchoredPosition = new Vector2(0, 950);
        currPanel = energyPanel;
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
        //Sequence sq = DOTween.Sequence();
        panel.gameObject.SetActive(true);
        panel.DOAnchorPos(new Vector2(0, 0), 1f).SetEase(Ease.OutBack);
    }

    private void HidePanel(RectTransform panel)
    {
        panel.DOAnchorPos(new Vector2(0, 950), 1f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}
