using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeCodeMessage :NetMessage {

    public ExchangeCodeData data;

    public class ExchangeCodeData
    {
        public RewardData[] reward;
        public UserStateData user_state;
    }


    public class UserStateData
    {
        public long money;
        public int energy;
        public int vip;
        public int wanted;
        public int[] pieces;
        public int card_fish;
        public int master_card;
        public int dungeon_keys;
    }
}
