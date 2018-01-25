using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;
public class UIBuyWantedWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIBuyWantedWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
            }
            return _windowData;
        }
    }

    public CounterControler conter;
    public TextMeshProUGUI costText;
    const float UNIT_PRICE = 28;

    private Product wanted;

    private void Awake()
    {
        wanted = GameMainManager.instance.iap.GetProductWithID(new GoodsData("302").GetPurchaseID());
        conter.onChangeValue += OnChangeValue;
    }

    private void OnDestroy()
    {
        conter.onChangeValue -= OnChangeValue;
    }

    protected override void StartShowWindow(object[] data)
    {
        conter.num = 1;
    }

    private void OnChangeValue(int num)
    {
        if(wanted!=null)
        {
            costText.text = wanted.metadata.localizedPriceString;
        }else
        {
            costText.text = "￥" + num * UNIT_PRICE;
        }
        
       
    }

    public void OnBuyBtn()
    {
        if (wanted != null)
        {
            GameMainManager.instance.iap.Purchase(wanted.definition.id);
        }
            
    }
}
