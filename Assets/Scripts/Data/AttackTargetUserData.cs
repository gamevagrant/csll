
/// <summary>
/// 可以攻击的对象
/// </summary>
public class AttackTargetUserData {

    public long uid;                           // 用户ID
    public string name;                         // 昵称
    public string headImg;                        // 头像url
    public int crowns;                    // 星星数量
    public int gender;                     // 性别 1 男 2 女
    public bool isVip;                 // 暂时未用到
    public string signature;                      // 暂时未用到
    public int islandId;                     // 岛屿ID
    public BuildingData[] buildings;                       // 建筑信息    // Building 数据结构在上面
}
