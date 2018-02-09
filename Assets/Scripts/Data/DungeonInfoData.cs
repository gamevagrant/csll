using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInfoData {

    /// <summary>
    /// 副本创建时间 用于唯一标记副本
    /// </summary>
    public int create_time;
    /// <summary>
    /// //（1开启 2完成没有领奖励 3完成且领取了奖励）
    /// </summary>
    public int state;
    /// <summary>
    /// 副本所有者的uid
    /// </summary>
    public int owner;
    /// <summary>
    /// boss血量
    /// </summary>
    public int boss_hp;

    private float timeTag;
    private int _time_remaining;// 倒计时
    /// <summary>
    /// 请求数据时倒计时的剩余时间
    /// </summary>
    public int time_remaining
    {
        get
        {
            return _time_remaining;
        }
        set
        {
            _time_remaining = value;
            timeTag = Time.unscaledTime;
        }
    }

    /// <summary>
    /// 倒计时
    /// </summary>
    public int countDown
    {
        get
        {
            return (int)Mathf.Max(0, time_remaining - (Time.unscaledTime - timeTag));
        }
    }
    /// <summary>
    /// //0 没有未领取的奖项 1有未领取的奖品
    /// </summary>
    public int is_reward;
    /// <summary>
    /// //卡槽中当前牌值的总和
    /// </summary>
    public int cards;
    /// <summary>
    /// //是否攻击过蚌精
    /// </summary>
    public int is_attack;
    /// <summary>
    /// 0没有使用， 1使用过
    /// </summary>
    public int is_used_master_card_by_buy;
    public GoodsData buy_master_card;
    /// <summary>
    /// //自身奖品
    /// </summary>
    public RewardData[] self_rewards;
    /// <summary>
    /// //总奖
    /// </summary>
    private RewardData[][] _rewards;
    public RewardData[][] rewards
    {
        get
        {
            return _rewards;
        }
        set
        {
            _rewards = value;
            if(_rewards!=null)
            {
                //服务器用5代表了500万，这里处理一下
                foreach (RewardData[] rs in _rewards)
                {
                    foreach (RewardData rd in rs)
                    {
                        if (rd.type == "money" || rd.type == "gold")
                        {
                            rd.num = rd.num * 1000000;
                        }
                    }
                }
            }
           
        }
    }
    /// <summary>
    /// 选中的牌
    /// </summary>
    public DungeonCardData[] selected_cards;
    /// <summary>
    /// 大牌池里的牌
    /// </summary>
    public DungeonCardData[] card_big;
    /// <summary>
    /// 小牌池里的牌
    /// </summary>
    public DungeonCardData[] card_small;

    private float[] rewardLimit = { 0.3f, 0.5f, 0.8f, 0.95f, 1 };
    /// <summary>
    /// 目前伤害可以得到的奖励索引
    /// </summary>
    public int rewardIndex
    {
        get
        {
            float f = cards / (float)boss_hp;
            for(int i =0;i< rewardLimit.Length;i++)
            {
                if(f<rewardLimit[i])
                {
                    return i;
                }
            }
            if(cards == boss_hp)
            {
                return rewardLimit.Length;
            }
            return 0;
        }
    }

    
}

public class DungeonCardData
{
    public int uid;
    public string head_img;
    public string user_name;
    public int num;
    public bool selected;

}
