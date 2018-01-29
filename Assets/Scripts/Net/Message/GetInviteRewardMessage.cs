using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInviteRewardMessage : NetMessage {

    public GetInviteRewardData data;

    public class GetInviteRewardData
    {
        public int energy;
        public int invite_process;
        public int invite_reward_num_limit;
        public long money;

        public ShareData.InviteReward[] invite_friend_rewards;
       
    }
}
