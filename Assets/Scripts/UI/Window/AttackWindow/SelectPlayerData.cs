

public class SelectPlayerData {

    public long uid; // 用户ID
    public int gender; // 性别 1 男 2 女
    public string headImg; // 头像url
    public bool isEmpty;// 暂时未使用
    public bool isVip; // 暂时未使用
    public string name;// 昵称
    public int crowns; // 星星数量
    public int attactTimes = 0; // 攻击次数
    public long stealMoney = 0; // 偷取金钱数量
    public int islandId; // 岛屿ID
    public bool isRandomUser;//是否时随机用户
    public bool isSelected;//是否是当前选中的
    public bool isWanted = false;//是否呗通缉
    public BuildingData[] buildings;

    public SelectPlayerData()
    {

    }
    public SelectPlayerData(BadGuyData data)
    {
        uid = data.uid;
        gender = data.gender;
        headImg = data.headImg;
        isEmpty = data.isEmpty;
        isVip = data.isVip;
        name = data.name;
        crowns = data.crowns;
        attactTimes = data.attactTimes;
        stealMoney = data.stealMoney;
        islandId = data.islandId;
        buildings = data.buildings;
        isWanted = data.isWanted;
    }

    public SelectPlayerData(FriendData data)
    {
        uid = data.uid;
        gender = data.gender;
        headImg = data.headImg;
        isEmpty = data.isEmpty;
        isVip = data.isVip;
        name = data.name;
        crowns = data.crowns;
        islandId = data.islandId;
        buildings = data.buildings;
    }

    public SelectPlayerData(AttackTargetUserData data)
    {
        uid = data.uid;
        gender = data.gender;
        headImg = data.headImg;
        isVip = data.isVip;
        name = data.name;
        crowns = data.crowns;
        islandId = data.islandId;
        buildings = data.buildings;
        isEmpty = true;
        foreach(BuildingData build in data.buildings)
        {
            if(build.level>0)
            {
                isEmpty = false;
            }
        }
    }
}
