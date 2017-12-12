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
		builder.AddProduct("gold_400k", ProductType.Consumable);
		if (builder.Configure<IAppleConfiguration> ().canMakePayments) {
			UnityPurchasing.Initialize (this, builder);
		} else {
			
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
	}

	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
	{
		Debug.Log ("IAP支付成功:"+e.purchasedProduct.metadata.localizedTitle);
		Debug.Log (string.Format ("交易id：{0} | 收据：{1}", e.purchasedProduct.transactionID, e.purchasedProduct.receipt));

		GameMainManager.instance.netManager.ShopList ((ret,data) => {
			Debug.Log ("IAP发放物品成功");
			controller.ConfirmPendingPurchase (e.purchasedProduct);
		});


		return PurchaseProcessingResult.Pending;
	}
		

	public void Purchase(string productID)
	{
		Debug.Log ("IAP点击购买:"+productID);
		if (controller != null) {
			
			controller.InitiatePurchase (productID);
		} else 
		{

			Debug.Log ("IAP未初始化:");
		}

	}
}
