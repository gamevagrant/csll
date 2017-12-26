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

    public AttackTargetUserData ToAttackTargetUserData()
    {
        AttackTargetUserData data = new AttackTargetUserData();
        data.uid = uid;
        data.name = name;
        data.headImg = headImg;
        data.crowns = crowns;
        data.signature = signature;
        data.islandId = islandId;
        data.buildings = buildings;
        return data;
    }
}
