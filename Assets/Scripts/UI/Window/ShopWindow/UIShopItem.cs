﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Purchasing;
public class UIShopItem : BaseItemView
{
    public TextMeshProUGUI countText;
    public Image image;
    public TextMeshProUGUI priceText;
    public Sprite[] sprites;

    private GoodsData goodsData;
    public override void SetData(object data)
    {
        goodsData = data as GoodsData;
        if(goodsData.type != "money" && goodsData.type != "energy")
        {
            //countText.fontSize = 25;
            countText.text = "+" + goodsData.name;
        }
        else
        {
            //countText.fontSize = 25;
            countText.text = "+" + GameUtils.GetCurrencyString(goodsData.quantity);
        }
        //priceText.text = "￥" + ((float)goodsData.price / 100).ToString();
        Product product = GameMainManager.instance.iap.GetProductWithID(goodsData.GetPurchaseID());
        if(product!=null)
        {
            priceText.text = product.metadata.localizedPriceString;
            int index = Mathf.Min(sprites.Length - 1, Mathf.Max(0, int.Parse(goodsData.goodsId) % 10 - 1));
            image.sprite = sprites[index];

        }else
        {
            gameObject.SetActive(false);
        }

        
    }

    public void OnClickBuyBtn()
    {
        GameMainManager.instance.iap.Purchase(goodsData.GetPurchaseID());
    }


}
