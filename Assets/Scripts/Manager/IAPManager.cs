using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : IStoreListener {

	private IStoreController controller;
	private IExtensionProvider extensions;

	public IAPManager()
	{
        ConfigurationBuilder builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
        ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
        foreach(ProductCatalogItem item in catalog.allProducts)
        {
            if(item.allStoreIDs.Count>0)
            {
                IDs ids = new IDs();
                foreach(StoreID storeID in item.allStoreIDs)
                {
                    ids.Add(storeID.id, storeID.store);
                }
                builder.AddProduct(item.id, item.type,ids);
            }else
            {
                builder.AddProduct(item.id, item.type);
            }
            
        }

        if (builder.Configure<IAppleConfiguration> ().canMakePayments) {
			UnityPurchasing.Initialize (this, builder);
		} else {
            Alert.Show("当前设备不允许支付，请启用内购功能");
            Debug.Log ("当前设备不允许支付");
		}

	}

	public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
	{
		this.controller = controller;
		this.extensions = extensions;
		Debug.Log ("IAP初始化成功");
	}

	public void OnInitializeFailed (InitializationFailureReason error)
	{
		Debug.Log ("IAP初始化失败"+error.ToString());
	}

	public void OnPurchaseFailed (Product i, PurchaseFailureReason p)
	{
		Debug.Log ("IAP支付失败:"+p);
        Alert.Show("支付失败：" + p);
        GameMainManager.instance.uiManager.isWaiting = false;
    }

	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
	{
		Debug.Log ("IAP支付成功:"+e.purchasedProduct.metadata.localizedTitle);
		Debug.Log (string.Format ("交易id：{0} | 收据：{1}", e.purchasedProduct.transactionID, e.purchasedProduct.receipt));

		GameMainManager.instance.netManager.GetInviteProgress((ret,data) => {
			Debug.Log ("IAP发放物品成功");
			controller.ConfirmPendingPurchase (e.purchasedProduct);
            GameMainManager.instance.uiManager.isWaiting = false;
            Alert.Show(string.Format("购买【{0}】成功",e.purchasedProduct.metadata.localizedTitle));
        });


		return PurchaseProcessingResult.Pending;
	}
		

	public void Purchase(string productID)
	{
        GameMainManager.instance.uiManager.isWaiting = true;
		Debug.Log ("IAP点击购买:"+productID);
		if (controller != null) {
			
			controller.InitiatePurchase (productID);
		} else 
		{
            Alert.Show("IAP未成功初始化");
            Debug.Log ("IAP未初始化:");
		}

	}

    public Product GetProductWithID(string id)
    {
       return controller.products.WithStoreSpecificID(id);
    }
}
