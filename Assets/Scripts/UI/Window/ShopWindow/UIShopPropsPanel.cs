using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIShopPropsPanel : MonoBehaviour {

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI dailyEneregyText;
    public TextMeshProUGUI vipPriceText;
    public TextMeshProUGUI wantedPriceText;
    public TextMeshProUGUI contentText;
    public DynamicScrollView scrollView;

    private GoodsData goodsVip;
    //private GoodsData[] goodsList;
    public void SetData(GoodsData[] goodsList)
    {
        //this.goodsList = goodsList;
        List<GoodsData> propsList = new List<GoodsData>();
        foreach(GoodsData goods in goodsList)
        {
            if(goods.type == "vip")
            {
                timeText.text = GameMainManager.instance.model.userData.vip_days.ToString() + "天";
                energyText.text = goods.extra["energy"].ToString();
                dailyEneregyText.text = goods.extra["dailyEnergy"].ToString();
                vipPriceText.text = "购买￥" + (goods.price / 100.0f).ToString();
                string str = "1直接获得<#FFFFFFFF>{0}点</color>能量\n" +
                    "2每天获得 <#FFFFFFFF>{1}点</color>能量，持续30天\n" +
                    "3享受vip专属标识\n" +
                    "4每小时恢复 <#FFFFFFFF>{2}点</color>能量\n" +
                    "5能量恢复上限增加到 <#FFFFFFFF>{3}点</color>";
                contentText.text = string.Format(str, goods.extra["energy"].ToString(),
                    goods.extra["dailyEnergy"].ToString(),
                    goods.extra["hourEnergy"].ToString(),
                    goods.extra["recoverEnergy"].ToString());

                goodsVip = goods;
            }
            else if(goods.type == "props")
            {
                // wantedPriceText.text = (goods.price / 100.0f).ToString("C");
                propsList.Add(goods);
            }
        }
        scrollView.setDatas(propsList);
    }

	public void OnClickBuyVIPBtn()
	{
        GameMainManager.instance.iap.Purchase(goodsVip.GetPurchaseID());
    }
}
