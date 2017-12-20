
/// <summary>
/// 商店物品
/// </summary>
public class GoodsData  {

    public string type;                                 // 类型
    public string name;                                 // display name
    public int quantity;                            // 数量
    public string goodsId;                             // 商品ID
    public int price;                            // 价格
    public int order;                             // 顺序
    public int isShow;                             // 是否显示
    public LitJson.JsonData extra;

    public GoodsData()
    {

    }

    public GoodsData(string id)
    {
        goodsId = id;
    }

    /// <summary>
    /// 应用内购买用的id
    /// </summary>
    /// <returns></returns>
    public string GetPurchaseID()
    {
        string purchaseID = "";
        string type = goodsId.Substring(0, goodsId.Length - 2);
        string subType = goodsId.Substring(goodsId.Length - 2, 2);
        if (type == "1")
        {
            purchaseID += "energy_item";
        }
        else if (type == "2")
        {
            purchaseID += "gold_item";
        }
        else if (type == "3")
        {
            purchaseID += "props_item";
        }
        int subNum = int.Parse(subType);
        purchaseID += subNum.ToString();

        return purchaseID;
    }

    public static string GetGoodsID(string purchaseID)
    {
        string id = "";
        string[] list = purchaseID.Split('_');
        if(list[0]== "energy")
        {
            id += "1";
        }
        else if (list[0] == "gold")
        {
            id += "2";
        }
        else if (list[0] == "props")
        {
            id += "3";
        }
        int index = purchaseID.LastIndexOf("item");
        string s = purchaseID.Substring(index + 4);
        int num = int.Parse(s);
        id += num.ToString("D2");
        return id;
    }
}
