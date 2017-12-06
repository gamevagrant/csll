
public class ShareData {

    public int energy;// 当前体力值
    public int ttl;
    public int num;
    public int is_reward;// 是否获得奖励
    public int invite_energy_reward;// 好友邀请奖励
    public int invite_reward_num_limit;// 好友邀请上限
    public InviteReward[] invite_friend_rewards; // 邀请好友从1位到100位的数组  // Invite 数据结构在上面
    public RecallableFriendData[] recall_friend_rewards;
    public int invite_process;

    public class InviteReward
    {
        public int num;                                    // 邀请到第几个好友
        public int status;                                 // 0:不可以领取 1:可以领取 2：已领取 
        public int inviteid;                               
        public string username;
        public string avatar;
        public RewardData[] rewardList;                     //奖励物品

    }

    public class RecallableFriendData
    {
        public long uid;
        public string name;
        public string head_img;
        public string last_active_time;
        public RewardData reward;
        public int status;
        public bool isVip;
        public int head_frame;//头像框
    }
}
