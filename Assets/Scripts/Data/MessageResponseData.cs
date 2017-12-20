/// <summary>
/// 消息列表
/// </summary>
public class MessageResponseData {

    public long uid;                     // 用户ID
    public long toid;                    // 暂时未用到
    public int action;              // 1 表示摧毁建筑 2 表示偷取
    public long result;              // 暂时未用到
    public string time;                // 时间
    public string name;                // 昵称
    public string headImg;              // 头像url
    public int crowns;            // 星星数量
    public LitJson.JsonData extra;                   // 其他数据
    public bool read;                 // 是否已读
    public bool isWanted;            // 暂时未用到
    public bool isVip;
    public int head_frame;
}
