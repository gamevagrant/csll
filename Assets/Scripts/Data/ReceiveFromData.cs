
/// <summary>
/// 获取好友赠送能量的返回数据
/// </summary>
public class ReceiveFromData {

    public int maxEnergy;                          // 最大体力值
    public int energy;                          // 当前剩余体力值
    public int recoverEnergy;                    // 体力恢复值
    public long timeToRecover;                      // 体力恢复剩余值
    public FriendData[] friends;                             // 好友列表   // Friend 数据结构在上面
    public int dailyCount;                          // 数量
    public int dailyLimit;                       // 每日次数限制
}
