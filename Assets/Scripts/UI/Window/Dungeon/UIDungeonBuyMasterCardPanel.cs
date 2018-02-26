using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;

public class UIDungeonBuyMasterCardPanel : MonoBehaviour {

    public TextMeshProUGUI priceText;

    Product product;

	public void SetData(GoodsData goodsData)
    {
        product = GameMainManager.instance.iap.GetProductWithID(goodsData.GetPurchaseID());
        priceText.text = product.metadata.localizedPriceString;
    }

    public void BuyMasterCard()
    {
        if(product!=null)
        {
            GameMainManager.instance.iap.Purchase(product.definition.id);
            UIDungeonPopupPanels.instance.ClosePanel(transform as RectTransform);
        }
        
    }
}
