using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBuyingReward {

    /*
     * "timeliness": 86400, 
      "gift_bag": [
        {
          "type": "gold", 
          "num": 400000
        }, 
        {
          "type": "energy", 
          "num": 30
        }
      ], 
      "countdown": 0, 
      "price": 100, 
      "original_price": 3000, 
      "itemId": 303, 
      "buy_status": 0
     * */
    /// <summary>
    /// 未使用
    /// </summary>
    public int timeliness;
    /// <summary>
    /// 物品列表
    /// </summary>
    public RewardData[] gift_bag;
    /// <summary>
    /// 倒计时
    /// </summary>
    private long _countdown;
    public long countdown
    {
        get
        {
            return _countdown;
        }
        set
        {
            _countdown = value;
            timeTag = Time.time;
        }
    }
    /// <summary>
    /// 购买Id
    /// </summary>
    public int itemId;
    /// <summary>
    ///  0，不展示；1，展示
    /// </summary>
    public int buy_status;
    public int original_price;//真实价格的3.33倍
    /// <summary>
    /// app版本中不使用此参数 价格从应用商店读取
    /// </summary>
    public int price;
    /// <summary>
    /// 数据更新时的 Time.time标记
    /// </summary>
    public float timeTag;
}
