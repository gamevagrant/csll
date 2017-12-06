

public class InviteFriendsMessage : NetMessage
{

    public InviteFriendsData data;

    public class InviteFriendsData
    {
        public int energy;
        public long money;
        public bool invite_limit_reward;
        public int invite_energy_reward;
        public int invite_reward_num_limit;
        public int invite_reward_num_limit_daily;
        public int invite_times_daily;
        public RewardData[] reward_list;
    }
}
