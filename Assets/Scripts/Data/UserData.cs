using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.Open;

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
    public int tutorial;                       // 新手教程  数值大于等于18表示已完成新手教程，否则未完成
    public long money;                   // 金钱数量
    public int maxEnergy;              // 最大体力值
    public int energy;              // 当前体力值
    public int recoverEnergy;            // 体力恢复值
    public long timeToRecover;                // 体力恢复剩余时间
    public float timeTag;                     //获取到体力剩余时间时的时间节点
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
    public AttackTargetUserData attackTargetUser;//可以攻击的对象 攻击重突退出 下次登录会返回此值
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
    public int last_action;//上次结束游戏时正在干的事情0 默认 无操作，1 攻击, 2 偷取
    public List<InvitableFriendsData> invitableList;//可邀请好友列表
    public List<ShareData.RecallableFriendData> recallableList;//可召回好友列表
    public int daily_energy;//每日能量
    public FirstBuyingReward one_yuan_buying;//一元购
    public int max_island;//最大岛屿数

    public DungeonInfoData dungeon_info;
    public int dungeon_keys;
    public int dungeon_tutorial;
    public int dungeon_reward;
    public int card_fish;
    public int master_card;
    /// <summary>
    /// 建造提示红点数值
    /// </summary>
    public int buildingTip
    {
        get
        {
            int num = 0;
            if (buildingCost != null)
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
            if (friendNotAgreeInfo != null && friendNotAgreeInfo.Length > 0)
            {
                return friendNotAgreeInfo.Length;
            } else if (friendInfo != null)
            {
                foreach (FriendData fd in friendInfo)
                {
                    if (fd.sendStatus == 0 || fd.receiveStatus == 1)
                    {
                        return 1;
                    }
                }

            }
            return 0;
        }
    }


    /// <summary>
    /// 地图红点
    /// </summary>
    public int mapTip
    {
        get
        {
            if (islandId < 3)
            {
                return 0;
            }
            if (mapInfo != null)
            {
                for (int i = 0; i < mapInfo.mines.Length; i++)
                {
                    MinesData mines = mapInfo.mines[i];
                    if (mines.miner < mines.costs.Length)
                    {
                        if (money > mines.costs[mines.miner])
                        {
                            return 1;
                        }
                        break;
                    }
                }
                if (mapInfo.moneyBox >= mapInfo.limit)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
    /// <summary>
    /// 消息邮件红点
    /// </summary>
    public int mailTip
    {
        get
        {

            if (user_mail != null)
            {
                int num = 0;
                foreach (MailData mail in user_mail)
                {
                    if (mail.is_get == 1)
                    {
                        num++;
                    }
                }
                return num;
            }

            if (messages != null)
            {
                foreach (MessageResponseData m in messages)
                {
                    if (!m.read)
                    {
                        return 1;
                    }
                }
            }
            return 0;

        }
    }

    public int invitTip
    {
        get
        {
            if (invitableList != null)
            {
                return invitableList.Count;
            }
            return 0;
        }
    }

    public int recallTip
    {
        get
        {
            if (recallableList != null)
            {
                return recallableList.Count;
            }
            return 0;
        }
    }

    public int dailyTaskTip
    {
        get
        {
            if (islandId < 2)
            {
                return 0;
            }
            if (daily_task != null)
            {
                if (daily_task.extra_reward.status != 2)
                {
                    return 1;
                }
                foreach (TaskItemData data in daily_task.tasks)
                {
                    if (data.status != 2)
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }
    }

    public int dailyRewardTip
    {
        get
        {
            if (!daily_prize_limit)
            {
                return 1;
            }
            if (weekly_prize_confs != null)
            {
                foreach (DailyPrizeConfData item in weekly_prize_confs)
                {
                    if (item.status == 1)
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }
    }

    public int dailyEnergyTip
    {
        get
        {
            return daily_energy;
        }
    }

    public bool isTutorialing
    {
        get
        {
            return tutorial > 0 && tutorial < GameSetting.TUTORIAL_MAX;
        }
    }

    /// <summary>
    /// 0:可开启 未开启 1：已开启 2：可领取奖励 3:不可开启
    /// </summary>
    public int dungeonState
    {
        get
        {
            if(dungeon_info==null && dungeon_keys>0)
            {
                return 0;
            }else if(dungeon_info!=null && dungeon_info.is_reward==0)
            {
                return 1;
            }else if(dungeon_info != null && dungeon_info.is_reward == 1)
            {
                return 2;
            }else
            {
                return 3;
            }
        }
    }
}
