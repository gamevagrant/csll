
public class RecallFriendsMessage : NetMessage
{
    public RecallFriendsData data;

    public class RecallFriendsData
    {
        public int energy;
        public long money;
        public bool recall_limit_reward;
        public int recall_reward_num_limit_daily;
        public int recall_times_daily;
        public RewardData[] reward_list;
    }

}
