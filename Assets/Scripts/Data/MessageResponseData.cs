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

    public string describe
    {
        get
        {
            string str = "";
            switch(action)
            {
                case 1:
                    if ((bool)extra["isShielded"])
                    {
                        str = string.Format("你成功防御了<color=#1995BCFF>{0}</color>的攻击", name);
                    }
                    else
                    {
                        if ((int)extra["building"]["status"] == 2)
                        {
                            str = string.Format("<color=#1995BCFF>{0}</color>损坏了你的{1}", name, GameMainManager.instance.configManager.islandConfig.GetBuildingName((int)extra["building_index"]));
                        }
                        else
                        {
                            str = string.Format("<color=#1995BCFF>{0}</color>摧毁了你的{1}", name, GameMainManager.instance.configManager.islandConfig.GetBuildingName((int)extra["building_index"]));
                        }
                    }
                    break;
                case 2:
                    str = string.Format("<color=#1995BCFF>{0}</color>偷走了你{1}金币", name, extra["reward"]);
                    break;
                case 5:
                    str = string.Format("<color=#1995BCFF>{0}</color>正在通缉<color=#BA7F00FF>{1}</color>,帮助好友攻击可以获得300k奖金", name, extra["name"]);
                    break;
                case 6:
                    str = string.Format("<color=#1995BCFF>{0}</color>正在通缉你，战斗号角已经吹响！", name);
                    break;
                case 11:
                    str = string.Format("您已经完成了每日任务【{0}】快去领奖吧！",extra["task"]["name"].ToString());
                    break;
                default:
                    break;
            }
            return str;
        }
    }
/*
 * action 枚举
iota=0
AttackAction
StealAction
AddFriendAction
PayAction
InviteWantAction
WantAction
PieceAction
InformAction
DonatePieceAction
NoticeAction
DailyTaskAction
Event20171001GetShovel
Event20171001NotifyRedpacket
MailAction
AchieveRedDotAction
AchieveStart
DungeonKeyGet //17
DungeonInvite
DungeonLotto
  */
}
