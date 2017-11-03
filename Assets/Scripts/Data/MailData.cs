
/// <summary>
/// 邮件数据
/// </summary>
public class MailData {
    /*
     {
        "type": 3, 
        "tittle": "关注领取奖励", 
        "desc": "关注领取奖励20能量", 
        "reward": [
          {
            "type": "energy", 
            "num": 20, 
            "name": ""
          }
        ], 
        "is_get": 1, 
        "time": "今天 14:46:35"
      }
      */
    public int type;//MailTypeVIPEtraEnergy= 1    MailTypeVIPDailyEnergy= 2	    MailTypeSubscribe= 3	 MailTypeShareDungeonReward = 4
    public string tittle;
    public string desc;
    public int is_get;//// 0不可以 1可以领取 2已领取
    public string time;
    public RewardData[] reward;
}
/// <summary>
/// 邮件中的奖励物品数据
/// </summary>
public class RewardData
{
    public string type;
    public int num;
    public string name;
}
