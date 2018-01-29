/// <summary>
/// 任务数据
/// </summary>
public class TaskItemData  {

    public int type;              // 类型
    /// <summary>
    /// // 0默认, 1可领取， 2已领取
    /// </summary>
    public int status;              
    public long totalProgress;       // 总进度
    public long progress;          // 完成进度
    public RewardData reward;                // 奖励   // DailyReward 数据结构在上面     
    public string name;            // display name
    public string desc;              // 描述
}
