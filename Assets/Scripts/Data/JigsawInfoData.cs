
/// <summary>
/// 拼图数据
/// </summary>
public class JigsawInfoData {

    public int openTime;                        // 开放时间
    public long closeTime;                         // 关闭时间
    public int series;                       // 暂时未用到
    public int[] pieces;                         // 已经使用的拼图碎片 // 1 表示已经拼成 0 表示还缺什么碎片
    public int[] allInOnes;                         // 暂时未用到
    public int[] bags;                          // 背包中碎片 // 数字表示碎片的数量
    public int rewardCount;                      // 获取奖励次数
    public int rewardLimit;                       // 奖励次数限制
                                                  //      []*RewardItem rewardList                          // 暂时未用到
    public int donate_jigsaw_count_daily;         // 赠送拼图碎片单日次数
    public int donate_jigsaw_count_limit_daily;  // 赠送拼图碎片次数上限
    public bool hasUnReceivedPiece;            // 是否有未领取的拼图碎片
}
