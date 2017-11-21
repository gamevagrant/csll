using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData {

    public long uid;                             // 用户ID
    public string name;                            // 昵称
    public int gender;                          // 性别 1 男 2 女
    public string headImg;                         // 用户头像url
    public bool isAuthed;                        // 是否已经授权
    public bool IsSubscribed;                    // 是否关注公众号
    public bool showSubscribed;                  // 暂时未用到
    public string platformId;                      // openid
    public int platform;                      // 最后一次登录的平台 0 公众号 1 微信扫码 2 游客
    public long tutorial;                       // 新手教程  数值大于等于18表示已完成新手教程，否则未完成
    public long money;                   // 金钱数量
    public int maxEnergy;              // 最大体力值
    public int energy;              // 当前体力值
    public int recoverEnergy;            // 体力恢复值
    public long timeToRecover;                // 体力恢复剩余时间
    public long timeToRecoverInterval;      // 体力恢复时间间隔
    public int islandId;      // 岛屿ID 从1 开始
    public BuildingData[] buildings;               // 岛屿内各物体  // Building 数据结构在上面
    public int crowns;           // 星星数量
    public int shields;           // 护盾数量
    public int maxBet;          // 暂时未用到
    public RollerItemData[] rollerItems;                // 转盘items     // RollerItem 数据结构在上面
    public TargetData stealTarget;                    // 偷取对象      // Target 数据结构在上面
    public int lastRollerType;          // 暂时未用到
    public int shareCount;                // 暂时未用到
    public int shareTime;                 // 暂时未用到
    public DailyPrizeConfData[] daily_prize_confs;            // 每日登录奖励  // DailyPrizeConf 数据结构在上面
    public DailyPrizeConfData[] weekly_prize_confs;             // 连续登录奖励
    public int dailyPrizeDay;         // 登录第n天
    public bool daily_prize_limit;         // 用户是否可以领取日奖励(日登陆奖励)
    public int week_prize_day;             // 连续登录第n天
    public int wantedCount;             // 通缉令数量
    public int ShipwreckCount;            // 暂时未用到
    public int cookieCount;            // 暂时未用到
    public int potionCount;            // 暂时未用到
    public int hatchCount;             // 暂时未用到
    public int hornCount;             // 暂时未用到
    public int miniShieldCount;            // 暂时未用到
    public int monthCardExpired;           // 暂时未用到
    public bool gotNewbieGift;            // 暂时未用到
    public bool gotOccasionalGift;            // 暂时未用到
    public bool gotDailyShop;           // 暂时未用到
    public int allInOnePiece;             // 暂时未用到
    public int killTitanCannonBall;         // 暂时未用到
    public int summonStone;                 // 暂时未用到
    public int puffer;                    // 暂时未用到
    public int lolly;                    // 暂时未用到
    public int guildMedal;                 // 暂时未用到
                                           //      *DailyEventInfo dailyEventInfo                // 暂时未用到
    public MapInfoData mapInfo;                                       //     *MapInfo mapInfo                         // 暂时未用到
                                           //      *Guild guild                           // 暂时未用到
    public JigsawInfoData jigsawInfo;                      // 拼图    // JigsawInfo 数据结构在上面
    public string cashInfo;                // 暂时未用到
    public FriendData[] friendInfo;                   // 好友信息  // Friend 数据结构在上面
    public FriendData[] friendNotAgreeInfo;           // 暂时未用到
    public string friendshipCode;             // 友情码
    public string friendRewards;               // 暂时未用到
    public int friendRewardMaxLimit;         // 暂时未用到
    public string recallRewards;                 // 暂时未用到
    public int recallRewardCount;           // 暂时未用到
    public string gainRecallReward;              // 暂时未用到 
    public int[][] buildingCost;              // 岛屿各物体建造价格  
    public int[][] buildingRepairCost;           // 岛屿各建筑维修价格
    public StealIslandData[] stealIslands;                  // 偷取对象  // StealIsland 数据结构在上面
    public AttackTargetData attackTarget;                 // 攻击对象  // AttackTarget 数据结构在上面
    public int betCount;              // 暂时未用到 
    public int islandCount;          // 暂时未用到 
    public bool isTutorialMiner;           // 暂时未用到 
    public bool gainIslandReward;           // 暂时未用到 
    public bool canIslandShare;            // 暂时未用到 
    public string petName;              // 暂时未用到 
    public int petSleepRemain;           // 暂时未用到 
    public int petExpRemain;           // 暂时未用到 
    public int loginRewardRemain;         // 暂时未用到 
    public string speedGiftRemain;              // 暂时未用到 
    public bool gotSubscribedReward;         // 是否获取公众号奖励
    public bool gotMonthCardReward;          // 暂时未用到 
    public bool forbiddenPush;            // 禁用白天推送 
    public bool nightAllowPush;            // 禁用晚上推送 
    public bool broadcastOff;             // 暂时未用到
    public bool worldChatOff;              // 暂时未用到
    public bool musicOff;               // 禁用音效
    public string signature;               // 暂时未用到
    public string secret;               // 暂时未用到
    public string broadcast;             // 暂时未用到
    public int broadcastChannel;         // 暂时未用到
                                         //      []*ActivityInfo activityInfos    ;               // 暂时未用到
    public bool isExistConnonContestAward;     // 暂时未用到
    public string inviteCode;          // 暂时未用到
    public MessageResponseData[] messages;                   // 系统消息列表  // MessageResponse 数据结构在上面
    public int dailyCount;         // 暂时未用到
    public int dailyLimit;          // 暂时未用到
    public Dictionary<string, NoticeItemData> ActivityNotices;       // 系统公告列表  // NoticeItem 数据结构在上面
    public NewbieAttackTargetData newbie_attack_target;       // 新手引导中攻击者的头像和昵称  // NewbieAttackTarget 数据结构在上面
    public DailyTaskData daily_task;          // 每日任务  // DailyTask 数据结构在上面
    public AnnouncementData announcement;//系统公告
    public MailData[] user_mail;//邮件
    public bool isVip;//是否时vip
    public int vip_days;//VIP剩余时间

    /// <summary>
    /// 建造提示红点数值
    /// </summary>
    public int buildingTip
    {
        get
        {
            int num = 0;
            if(buildingCost != null)
            {
                for (int i = 0; i < buildingCost.Length; i++)
                {
                    if (buildings[i].level < buildingCost[i].Length)
                    {
                        int cost = buildingCost[i][buildings[i].level];
                        if (money > cost)
                        {
                            num++;
                        }
                    }

                }
            }
           
            return num;
        }
    }
    /// <summary>
    /// 好友提示红点
    /// </summary>
    public int friendTip
    {
        get
        {
            if(friendNotAgreeInfo!=null && friendNotAgreeInfo.Length>0)
            {
                return friendNotAgreeInfo.Length;
            }else if(friendInfo != null)
            {
                foreach(FriendData fd in friendInfo)
                {
                    if(fd.sendStatus ==0|| fd.receiveStatus == 1)
                    {
                        return 1;
                    }
                }
               
            }
            return 0;
        }
    }

    public int mailTip
    {
        get
        {
            int num = 0;
            if (user_mail != null)
            {
                foreach (MailData mail in user_mail)
                {
                    if (mail.is_get == 1)
                    {
                        num++;
                    }
                }
            }
           
            return num;
        }
    }

}
