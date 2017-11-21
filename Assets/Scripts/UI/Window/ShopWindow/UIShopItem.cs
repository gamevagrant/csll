﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        if(goodsData.type == "props")
        {
            countText.text = "+" + goodsData.name;
        }
        else
        {
            countText.text = "+" + goodsData.quantity.ToString();
        }
        priceText.text = "￥" + ((float)goodsData.price / 100).ToString();
        int index = Mathf.Min(sprites.Length - 1, Mathf.Max(0, int.Parse(goodsData.goodsId) % 10 - 1));
        image.sprite = sprites[index];
    }

    public void OnClickBuyBtn()
    {

    }
}