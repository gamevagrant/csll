/// <summary>
/// 好友信息
/// </summary>
public class FriendData {

    public long uid;                 // 用户ID
    public int crowns;              // 星星数量
    public long crownsUpdateTime;     // 星星数量更新时间
    public string friendName;            // 好友昵称
    public int gender;          // 性别 1 男 2 女
    public string headImg;            // 头像url
    public bool isEmpty;           // 暂时未用到
    public bool isVip;             // 暂时未用到
    public string name;               // 好友昵称
    public int rank;             // 暂时未用到
    public int recallCount;          // 暂时未用到
    public string signature;             // 暂时未用到
    public string updateTime;           // 上次活跃时间
                                        //     *BasicGuild guild                   // 暂时未用到
    public int status;           // 状态值
    /// <summary>
    /// 0=我未给对方赠送体力 1=我已给对方赠送体力
    /// </summary>
    public int sendStatus;       // 0=我未给对方赠送体力 1=我已给对方赠送体力
    /// <summary>
    /// 0=对方没有送 1=对方送了，我没领 2=对方送了我领了
    /// </summary>
    public int receiveStatus;       // 0=对方没有送 1=对方送了，我没领 2=对方送了我领了
    public BuildingData[] buildings;              // 好友建筑信息    // Building 数据结构在上面
    public int islandId;              // 好友岛屿ID
}
