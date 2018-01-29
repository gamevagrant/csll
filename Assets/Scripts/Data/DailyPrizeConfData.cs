
/// <summary>
/// 每日登录奖励
/// </summary>
public class DailyPrizeConfData {
    public int day;       // 第n天
    public string type;        // 奖励类型
    public int num;    // 数值
    /// <summary>
    /// // 状态值 0 不可以领，1可以领取，2已领取
    /// </summary>
    public int status;    
}
