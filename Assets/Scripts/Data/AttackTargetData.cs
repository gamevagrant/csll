/// <summary>
/// 攻击对象
/// </summary>
public class AttackTargetData {
    public long uid;                // 用户ID
    public string name;               // 昵称
    public string headImg;              // 头像url
    public int crowns;           // 星星数量
    public string signature;         // 暂时未用到
    public int islandId;        // 岛屿ID
    public BuildingData[] buildings;            // 建筑信息   // Building数据结构在上面
}
