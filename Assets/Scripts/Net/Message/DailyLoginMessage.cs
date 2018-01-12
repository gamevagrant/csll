using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyLoginMessage : NetMessage {

    public GetDailyLoginRewardData data;

    public class GetDailyLoginRewardData
    {
        public int money;                             // 当前金币数量
        public int energy;                       // 当前剩余体力值
        public int wanted_count;                   // 暂时未用到
        public DailyPrizeConfData[] prize;                            // 奖励                 
        public DailyPrizeConfData[] weekly_prize_confs;                  // 连续登录奖励
        public int dailyPrizeDay;               // 连续登录第n天
        public bool daily_prize_limit;                // 用户是否可以领取日奖励(日登陆奖励)
    }
}
