using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using LitJson;

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
        Waiting.Disable();
    }

	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
	{
		Debug.Log ("IAP支付成功:"+e.purchasedProduct.metadata.localizedTitle);
		Debug.Log (string.Format ("交易id：{0} | 收据：{1}", e.purchasedProduct.transactionID, e.purchasedProduct.receipt));
#if UNITY_EDITOR
        string appStore = "AppleAppStore";
        string payload = "MIIT0AYJKoZIhvcNAQcCoIITwTCCE70CAQExCzAJBgUrDgMCGgUAMIIDcQYJKoZIhvcNAQcBoIIDYgSCA14xggNaMAoCAQgCAQEEAhYAMAoCARQCAQEEAgwAMAsCAQECAQEEAwIBADALAgEDAgEBBAMMATAwCwIBCwIBAQQDAgEAMAsCAQ4CAQEEAwIBcTALAgEPAgEBBAMCAQAwCwIBEAIBAQQDAgEAMAsCARkCAQEEAwIBAzAMAgEKAgEBBAQWAjQrMA0CAQ0CAQEEBQIDAa4WMA0CARMCAQEEBQwDMS4wMA4CAQkCAQEEBgIEUDI0OTAYAgEEAgECBBDsY8xsQTlEb5FNr4dn0kr2MBsCAQACAQEEEwwRUHJvZHVjdGlvblNhbmRib3gwGwIBAgIBAQQTDBFjb20ubnV0c3BsYXkuY3NsbDAcAgEFAgEBBBS6ihSt3pqSbPQIVhYv0HIR+v1bFDAeAgEMAgEBBBYWFDIwMTctMTItMTNUMDY6MzM6MzNaMB4CARICAQEEFhYUMjAxMy0wOC0wMVQwNzowMDowMFowSgIBBwIBAQRCDlOeKilUDnnF72sfxm8IIEVpwvXREuwYQmlI5voicMFLh4QTv0lc7co01X4CgFG1JBcflrKqtRHQdf+YgTq98CzoMFYCAQYCAQEETkGDiBM9kviRg4x7r2H0yq2bVEJB/BsbV41EVA+Y3Q61c2SPrOjWpUu4zwjMASShUS4hsIVfkR+9+5YRVF5m5nMA7VNqdsO8+UHmE3YohjCCAVECARECAQEEggFHMYIBQzALAgIGrAIBAQQCFgAwCwICBq0CAQEEAgwAMAsCAgawAgEBBAIWADALAgIGsgIBAQQCDAAwCwICBrMCAQEEAgwAMAsCAga0AgEBBAIMADALAgIGtQIBAQQCDAAwCwICBrYCAQEEAgwAMAwCAgalAgEBBAMCAQEwDAICBqsCAQEEAwIBATAMAgIGrgIBAQQDAgEAMAwCAgavAgEBBAMCAQAwDAICBrECAQEEAwIBADAXAgIGpgIBAQQODAxlbmVyZ3lfaXRlbTQwGwICBqcCAQEEEgwQMTAwMDAwMDM1OTAxODQ1OTAbAgIGqQIBAQQSDBAxMDAwMDAwMzU5MDE4NDU5MB8CAgaoAgEBBBYWFDIwMTctMTItMTNUMDY6MzM6MzNaMB8CAgaqAgEBBBYWFDIwMTctMTItMTNUMDY6MzM6MzNaoIIOZTCCBXwwggRkoAMCAQICCA7rV4fnngmNMA0GCSqGSIb3DQEBBQUAMIGWMQswCQYDVQQGEwJVUzETMBEGA1UECgwKQXBwbGUgSW5jLjEsMCoGA1UECwwjQXBwbGUgV29ybGR3aWRlIERldmVsb3BlciBSZWxhdGlvbnMxRDBCBgNVBAMMO0FwcGxlIFdvcmxkd2lkZSBEZXZlbG9wZXIgUmVsYXRpb25zIENlcnRpZmljYXRpb24gQXV0aG9yaXR5MB4XDTE1MTExMzAyMTUwOVoXDTIzMDIwNzIxNDg0N1owgYkxNzA1BgNVBAMMLk1hYyBBcHAgU3RvcmUgYW5kIGlUdW5lcyBTdG9yZSBSZWNlaXB0IFNpZ25pbmcxLDAqBgNVBAsMI0FwcGxlIFdvcmxkd2lkZSBEZXZlbG9wZXIgUmVsYXRpb25zMRMwEQYDVQQKDApBcHBsZSBJbmMuMQswCQYDVQQGEwJVUzCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKXPgf0looFb1oftI9ozHI7iI8ClxCbLPcaf7EoNVYb/pALXl8o5VG19f7JUGJ3ELFJxjmR7gs6JuknWCOW0iHHPP1tGLsbEHbgDqViiBD4heNXbt9COEo2DTFsqaDeTwvK9HsTSoQxKWFKrEuPt3R+YFZA1LcLMEsqNSIH3WHhUa+iMMTYfSgYMR1TzN5C4spKJfV+khUrhwJzguqS7gpdj9CuTwf0+b8rB9Typj1IawCUKdg7e/pn+/8Jr9VterHNRSQhWicxDkMyOgQLQoJe2XLGhaWmHkBBoJiY5uB0Qc7AKXcVz0N92O9gt2Yge4+wHz+KO0NP6JlWB7+IDSSMCAwEAAaOCAdcwggHTMD8GCCsGAQUFBwEBBDMwMTAvBggrBgEFBQcwAYYjaHR0cDovL29jc3AuYXBwbGUuY29tL29jc3AwMy13d2RyMDQwHQYDVR0OBBYEFJGknPzEdrefoIr0TfWPNl3tKwSFMAwGA1UdEwEB/wQCMAAwHwYDVR0jBBgwFoAUiCcXCam2GGCL7Ou69kdZxVJUo7cwggEeBgNVHSAEggEVMIIBETCCAQ0GCiqGSIb3Y2QFBgEwgf4wgcMGCCsGAQUFBwICMIG2DIGzUmVsaWFuY2Ugb24gdGhpcyBjZXJ0aWZpY2F0ZSBieSBhbnkgcGFydHkgYXNzdW1lcyBhY2NlcHRhbmNlIG9mIHRoZSB0aGVuIGFwcGxpY2FibGUgc3RhbmRhcmQgdGVybXMgYW5kIGNvbmRpdGlvbnMgb2YgdXNlLCBjZXJ0aWZpY2F0ZSBwb2xpY3kgYW5kIGNlcnRpZmljYXRpb24gcHJhY3RpY2Ugc3RhdGVtZW50cy4wNgYIKwYBBQUHAgEWKmh0dHA6Ly93d3cuYXBwbGUuY29tL2NlcnRpZmljYXRlYXV0aG9yaXR5LzAOBgNVHQ8BAf8EBAMCB4AwEAYKKoZIhvdjZAYLAQQCBQAwDQYJKoZIhvcNAQEFBQADggEBAA2mG9MuPeNbKwduQpZs0+iMQzCCX+Bc0Y2+vQ+9GvwlktuMhcOAWd/j4tcuBRSsDdu2uP78NS58y60Xa45/H+R3ubFnlbQTXqYZhnb4WiCV52OMD3P86O3GH66Z+GVIXKDgKDrAEDctuaAEOR9zucgF/fLefxoqKm4rAfygIFzZ630npjP49ZjgvkTbsUxn/G4KT8niBqjSl/OnjmtRolqEdWXRFgRi48Ff 9Qipz2jZkgDJwYyz+I0AZLpYYMB8r491ymm5WyrWHWhumEL1TKc3GZvMOxx6GUPzo22/SGAGDDaSK+zeGLUR2i0j0I78oGmcFxuegHs5R0UwYS/HE6gwggQiMIIDCqADAgECAggB3rzEOW2gEDANBgkqhkiG9w0BAQUFADBiMQswCQYDVQQGEwJVUzETMBEGA1UEChMKQXBwbGUgSW5jLjEmMCQGA1UECxMdQXBwbGUgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkxFjAUBgNVBAMTDUFwcGxlIFJvb3QgQ0EwHhcNMTMwMjA3MjE0ODQ3WhcNMjMwMjA3MjE0ODQ3WjCBljELMAkGA1UEBhMCVVMxEzARBgNVBAoMCkFwcGxlIEluYy4xLDAqBgNVBAsMI0FwcGxlIFdvcmxkd2lkZSBEZXZlbG9wZXIgUmVsYXRpb25zMUQwQgYDVQQDDDtBcHBsZSBXb3JsZHdpZGUgRGV2ZWxvcGVyIFJlbGF0aW9ucyBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMo4VKbLVqrIJDlI6Yzu7F+4fyaRvDRTes58Y4Bhd2RepQcjtjn+UC0VVlhwLX7EbsFKhT4v8N6EGqFXya97GP9q+hUSSRUIGayq2yoy7ZZjaFIVPYyK7L9rGJXgA6wBfZcFZ84OhZU3au0Jtq5nzVFkn8Zc0bxXbmc1gHY2pIeBbjiP2CsVTnsl2Fq/ToPBjdKT1RpxtWCcnTNOVfkSWAyGuBYNweV3RY1QSLorLeSUheHoxJ3GaKWwo/xnfnC6AllLd0KRObn1zeFM78A7SIym5SFd/Wpqu6cWNWDS5q3zRinJ6MOL6XnAamFnFbLw/eVovGJfbs+Z3e8bY/6SZasCAwEAAaOBpjCBozAdBgNVHQ4EFgQUiCcXCam2GGCL7Ou69kdZxVJUo7cwDwYDVR0TAQH/BAUwAwEB/zAfBgNVHSMEGDAWgBQr0GlHlHYJ/vRrjS5ApvdHTX8IXjAuBgNVHR8EJzAlMCOgIaAfhh1odHRwOi8vY3JsLmFwcGxlLmNvbS9yb290LmNybDAOBgNVHQ8BAf8EBAMCAYYwEAYKKoZIhvdjZAYCAQQCBQAwDQYJKoZIhvcNAQEFBQADggEBAE/P71m+LPWybC+P7hOHMugFNahui33JaQy52Re8dyzUZ+L9mm06WVzfgwG9sq4qYXKxr83DRTCPo4MNzh1HtPGTiqN0m6TDmHKHOz6vRQuSVLkyu5AYU2sKThC22R1QbCGAColOV4xrWzw9pv3e9w0jHQtKJoc/upGSTKQZEhltV/V6WId7aIrkhoxK6+JJFKql3VUAqa67SzCu4aCxvCmA5gl35b40ogHKf9ziCuY7uLvsumKV8wVjQYLNDzsdTJWk26v5yZXpT+RN5yaZgem8+bQp0gF6ZuEujPYhisX4eOGBrr/TkJ2prfOv/TgalmcwHFGlXOxxioK0bA8MFR8wggS7MIIDo6ADAgECAgECMA0GCSqGSIb3DQEBBQUAMGIxCzAJBgNVBAYTAlVTMRMwEQYDVQQKEwpBcHBsZSBJbmMuMSYwJAYDVQQLEx1BcHBsZSBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTEWMBQGA1UEAxMNQXBwbGUgUm9vdCBDQTAeFw0wNjA0MjUyMTQwMzZaFw0zNTAyMDkyMTQwMzZaMGIxCzAJBgNVBAYTAlVTMRMwEQYDVQQKEwpBcHBsZSBJbmMuMSYwJAYDVQQLEx1BcHBsZSBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTEWMBQGA1UEAxMNQXBwbGUgUm9vdCBDQTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAOSRqQkfkdseR1DrBe1eeYQt6zaiV0xV7IsZid75S2z1B6siMALoGD74UAnTf0GomPnRymacJGsR0KO75Bsqwx+VnnoMpEeLW9QWNzPLxA9NzhRp0ckZcvVdDtV/X5vyJQO6VY9NXQ3xZDUjFUsVWR2zlPf2nJ7PULrBWFBnjwi0IPfLrCwgb3C2PwEwjLdDzw+dPfMrSSgayP7OtbkO2V4c1ss9tTqt9A8OAJILsSEWLnTVPA3bYharo3GSR1NVwa8vQbP4++NwzeajTEV+H0xrUJZBicR0YgsQg0GHM4qBsTBY7FoEMoxos48d3mVz/2deZbxJ2HafMxRloXeUyS0CAwEAAaOCAXowggF2MA4GA1UdDwEB/wQEAwIBBjAPBgNVHRMBAf8EBTADAQH/MB0GA1UdDgQWBBQr0GlHlHYJ/vRrjS5ApvdHTX8IXjAfBgNVHSMEGDAWgBQr0GlHlHYJ/vRrjS5ApvdHTX8IXjCCAREGA1UdIASCAQgwggEEMIIBAAYJKoZIhvdjZAUBMIHyMCoGCCsGAQUFBwIBFh5odHRwczovL3d3dy5hcHBsZS5jb20vYXBwbGVjYS8wgcMGCCsGAQUFBwICMIG2GoGzUmVsaWFuY2Ugb24gdGhpcyBjZXJ0aWZpY2F0ZSBieSBhbnkgcGFydHkgYXNzdW1lcyBhY2NlcHRhbmNlIG9mIHRoZSB0aGVuIGFwcGxpY2FibGUgc3RhbmRhcmQgdGVybXMgYW5kIGNvbmRpdGlvbnMgb2YgdXNlLCBjZXJ0aWZpY2F0ZSBwb2xpY3kgYW5kIGNlcnRpZmljYXRpb24gcHJhY3RpY2Ugc3RhdGVtZW50cy4wDQYJKoZIhvcNAQEFBQADggEBAFw2mUwteLftjJvc83eb8nbSdzBPwR+Fg4UbmT1HN/Kpm0COLNSxkBLYvvRzm+7SZA/LeU802KI++Xj/a8gH7H05g4tTINM4xLG/mk8Ka/8r/FmnBQl8F0BWER5007eLIztHo9VvJOLr0bdw3w9F4SfK8W147ee1Fxeo3H4iNcol1dkP1mvUoiQjEfehrI9zgWDGG1sJL5Ky+ERI8GA4nhX1PSZnIIozavcNgs/e66Mv+VNqW2TAYzN39zoHLFbr2g8hDtq6cxlPtdk2f8GHVdmnmbkyQvvY1XGefqFStxu9k0IkEirHDx22TZxeY8hLgBdQqorV2uT80AkHN7B1dSExggHLMIIBxwIBATCBozCBljELMAkGA1UEBhMCVVMxEzARBgNVBAoMCkFwcGxlIEluYy4xLDAqBgNVBAsMI0FwcGxlIFdvcmxkd2lkZSBEZXZlbG9wZXIgUmVsYXRpb25zMUQwQgYDVQQDDDtBcHBsZSBXb3JsZHdpZGUgRGV2ZWxvcGVyIFJlbGF0aW9ucyBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eQIIDutXh+eeCY0wCQYFKw4DAhoFADANBgkqhkiG9w0BAQEFAASCAQA6J1augzpjcJV3U2+1yqA8UcOAkfrV7oIsALZO7SZL7/GYxCWql0SyMRFBV6qhKjX6YHI01h/adJFPZ484Wc3jEWnXItiX1YjKkUkAumSO2nmQJX00Od44gXfQ9Gstaf3dF3Njd2SO9my17t5eG/hglhWcXSWsPpNBC3dDZoJjacoIjXr7gzfGQwt3XxDQ5LHU3n2qLUXM5+LdN6JWuTfvNV/znVUYcBgo8BBhJ+Md0qiJBt7Kub0V4AhkrNMjEI69PK73vMWPiCTiXK+ILYQ9dOXMkadgW6UNMvzSt1dyhSRu3Q1f+SqScTyFOHA56qLjqs0dMQkofXigRuj+LLmj";
#else
        JsonData jd = JsonMapper.ToObject(e.purchasedProduct.receipt);
        string appStore = jd["Store"].ToString();
        string payload = jd["Payload"].ToString();
#endif
        string goodsID = GoodsData.GetGoodsID(e.purchasedProduct.definition.id);
        Debug.Log("goodsID:"+goodsID);
        string transactionID = e.purchasedProduct.transactionID;

        GameMainManager.instance.netManager.GetOrder(goodsID, 1, (ret, res) =>
        {
            string orderID = res.request_id;
            GameMainManager.instance.netManager.Purchase(appStore,transactionID,payload,orderID,(r, data) => {
                if (data.isOK)
                {
                    Debug.Log("IAP发放物品成功");
                    controller.ConfirmPendingPurchase(e.purchasedProduct);
                   
                    //Alert.Show(string.Format("成功购买【{0}】", e.purchasedProduct.metadata.localizedTitle));
                }
                else
                {
                    Alert.Show("发放物品失败：" + data.errmsg);
                }
                Waiting.Disable();
            });

        });

		


		return PurchaseProcessingResult.Pending;
	}
		

	public void Purchase(string productID)
	{
        Waiting.Enable();
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
        if(controller!=null && controller.products!=null)
        {
            return controller.products.WithStoreSpecificID(id);
        }
        return null;
    }
}
