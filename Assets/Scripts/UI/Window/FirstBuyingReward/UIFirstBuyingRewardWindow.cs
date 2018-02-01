﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using System;

public class UIFirstBuyingRewardWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIFirstBuyingRewardWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public UIFirstBuyingRewardItem[] rewardItems;
    public TextMeshProUGUI salePrice;
    public TextMeshProUGUI originalPrice;
    public TextMeshProUGUI price;
    public TextMeshProUGUI countDown;

    private FirstBuyingReward data;
    private Product product;
    private long lastTime;
    private long countDownTime
    {
        get
        {
            long t = (long)Mathf.Max(0, data.countdown - (Time.unscaledTime - data.timeTag));
            return t;
        }
    }

    private void Update()
    {
        UpdateCountDown();
    }

    protected override void StartShowWindow(object[] data)
    {
        this.data = GameMainManager.instance.model.userData.one_yuan_buying;
        GoodsData goods = new GoodsData(this.data.itemId.ToString());
        Product product = GameMainManager.instance.iap.GetProductWithID(goods.GetPurchaseID());
        if(product!=null)
        {
            price.text = product.metadata.localizedPriceString;
            originalPrice.text = product.metadata.localizedPriceString.Substring(0, 1) + (product.metadata.localizedPrice * 3m).ToString("F2");

            string[] priceSplite = product.metadata.localizedPriceString.Split('.');
            salePrice.text = string.Format("{0}.<size=22>{1}</size>", priceSplite[0], priceSplite[1]);
            product = GameMainManager.instance.iap.GetProductWithID(new GoodsData(this.data.itemId.ToString()).GetPurchaseID());
        }
        

        for (int i =0;i< rewardItems.Length;i++)
        {
            if(this.data.gift_bag!=null && i < this.data.gift_bag.Length)
            {
                rewardItems[i].SetData(this.data.gift_bag[i]);
            }else
            {
                rewardItems[i].SetData(null);
            }
        }
    }

    private void UpdateCountDown()
    {
        long t = countDownTime;
        if(t!=lastTime)
        {
            TimeSpan ts = new TimeSpan(t * 10000000L);
            string str = "";
            if (ts.Days > 0)
            {
                str = string.Format("{0}天 {1}:{2}:{3}", ts.Days.ToString(), ts.Hours.ToString("D2"), ts.Minutes.ToString("D2"), ts.Seconds.ToString("D2"));
            }
            else
            {
                str = string.Format("{0}:{1}:{2}", ts.Hours.ToString("D2"), ts.Minutes.ToString("D2"), ts.Seconds.ToString("D2"));
            }
            countDown.text = str;
            lastTime = t;
            if(t == 0)
            {
                OnClickClose();
            }
        }
        

    }

    public void OnClickBuyBtn()
    {
        
        if (product != null)
        {
            GameMainManager.instance.iap.Purchase(product.definition.id);
        }
    }
}
