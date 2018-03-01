using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonReapMessage : NetMessage {

    public DungeonReapMessageData data;

    public class DungeonReapMessageData
    {
        public DungeonInfoData dungeon_info;
        public int dungeon_keys;
        public RewardData[] dungeon_reward;

    }
}
