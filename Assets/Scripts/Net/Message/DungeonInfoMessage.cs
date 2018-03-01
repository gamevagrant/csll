using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInfoMessage : NetMessage {


    public DungeonInfoMessageData data;

    public class DungeonInfoMessageData
    {
        public DungeonInfoData dungeon_info;

        public int dungeon_keys;//获取副本信息和领奖时返回的数据
        public RewardData[] dungeon_reward;//副本攻击和领取奖励时返回的值
        public int card_fish;//使用食卡鱼后返回的值
        public int master_card;//使用王卡时返回的值
    }
}
