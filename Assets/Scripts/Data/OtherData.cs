
/// <summary>
/// 其他玩家信息
/// </summary>
public class OtherData {

    public long uid;                                    // 用户ID
    public string name;                                   // 昵称
    public string headImg;                         // 头像url
    public int crowns;                          // 星星数量
    public bool isVip;                            // 暂时未用到
    public int islandId;                           // 岛屿ID
    public BuildingData[] buildings;                              // 建筑信息   // Building 数据结构在上面
    public bool isFriend;                         // 是不是好友
    public string inviteCode;                             // 邀请码
}
