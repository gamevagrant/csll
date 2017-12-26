/// <summary>
/// 转懂转盘得到的数据
/// </summary>
public class RollData {

    public long money;                          // 金钱数量
    public int maxEnergy;                    // 最大体力
    public int energy;                        // 当前剩余体力
    public int recoverEnergy;                   // 体力恢复值
    public long timeToRecover;                 // 体力恢复时间剩余
    public int shields;                // 护盾数量
    public StealIslandData[] stealIslands;                    // 偷取岛屿数组   // StealIsland 数据结构在上面
    public AttackTargetUserData attackTarget;                    // 攻击对象
    public FriendData[] fof;                     // 暂时未用到
    public RollerItemData rollerItem;                   // 转盘item
    public int betCount;                      // 暂时未用到
    public long timeToRecoverInterval;           // 体力恢复时间间隔
    public int tutorial;                      // 新手教程
}
