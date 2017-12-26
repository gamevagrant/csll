
using System;

public interface INetManager
{

    bool Login(string openid, Action<bool, LoginMessage> callBack);
    bool TutorialComplete(Action<bool, TutorialCompleteMessage> callBack);
    /// <summary>
    /// 转转盘
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool Roll(Action<bool, RollMessage> callBack);
    /// <summary>
    /// 建造
    /// </summary>
    /// <param name="islandID">岛屿的id</param>
    /// <param name="buildIndex">建筑的索引</param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool Build(int islandID, int buildIndex, Action<bool, BuildMessage> callBack);
    /// <summary>
    /// 攻击
    /// </summary>
    /// <param name="puid">攻击玩家的ID</param>
    /// <param name="buildIndex">攻击建筑的索引</param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool Attack(long puid, int buildIndex, Action<bool, AttackMessage> callBack);
    /// <summary>
    /// 偷取
    /// </summary>
    /// <param name="idx">偷取对象索引</param>
    /// <returns></returns>
    bool Steal(int idx, Action<bool, StealMessage> callBack);

    /// <summary>
    /// 获取恶人列表
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool Enemy(Action<bool, BadGuyMessage> callBack);
    /// <summary>
    /// 获取仇人列表
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool Vengeance(Action<bool, VengeanceMessage> callBack);
    /// <summary>
    /// 获取玩家信息
    /// </summary>
    /// <param name="fid"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool Show(long fid, Action<bool, ShowMessage> callBack);

    /// <summary>
    /// 获取好友列表
    /// </summary>
    /// <param name="needFoF"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool Friend(int needFoF, Action<bool, FriendMessage> callBack);
    /// <summary>
    /// 同意好友申请
    /// </summary>
    /// <param name="friendUid"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool AgreeAddFriend(long friendUid, Action<bool, AgreeAddFriendMessage> callBack);
    /// <summary>
    /// 同意所有添加好友请求
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool AgreeAddAllFriend(Action<bool, FriendMessage> callBack);
    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="FriendshipCode"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool AddFriend(string FriendshipCode, Action<bool, FriendMessage> callBack);

    /// <summary>
    /// 删除好友/忽略添加好友请求
    /// </summary>
    /// <param name="friendUid"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool RemoveFriend(long friendUid, Action<bool, FriendMessage> callBack);
    /// <summary>
    /// 忽略所有添加好友请求
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool RemoveAllFriend(Action<bool, FriendMessage> callBack);
    /// <summary>
    /// 赠送好友能量
    /// </summary>
    /// <param name="friendUid"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool SendEnergy(long friendUid, Action<bool, SendEnergyMessage> callBack);
    /// <summary>
    /// 领取好友赠送的能量/一键领取好友赠送的能量
    /// </summary>
    /// <param name="friendUid">为0时一键领取/赠送好友能量</param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool ReceiveEnergy(long friendUid, Action<bool, ReceiveEnergyMessage> callBack);
    /// <summary>
    /// 获取商店数据
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool ShopList(Action<bool, ShopMessage> callBack);
    /// <summary>
    /// 获取世界排行榜
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool AllRank(Action<bool, AllRankMessage> callBack);
    /// <summary>
    /// 获取好友排行榜
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool FriendRank(Action<bool, SendEnergyMessage> callBack);
    /// <summary>
    /// 获取地图数据
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool GetMap(Action<bool, GetMapMessage> callBack);
    /// <summary>
    /// 购买矿工
    /// </summary>
    /// <param name="islanID"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool BuyMiner(int islanID, Action<bool, BuyMinerMessage> callBack);
    /// <summary>
    /// 收获采矿金币
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool ReapMine(Action<bool, BuyMinerMessage> callBack);
    /// <summary>
    /// 通缉玩家
    /// </summary>
    /// <param name="wid"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool Wanted(long wid, Action<bool, WantedMessage> callBack);
    /// <summary>
    /// 获取消息和邮件
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool Message(Action<bool, MessageMailMessage> callBack);
    /// <summary>
    /// 获取邮件列表里的奖励
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool GetReward(int index,Action<bool, GetRewardMessage> callBack);

    //------------------------平台区分---------------------------------------

    /// <summary>
    /// facebook登录
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="expirationTime"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool LoginFB(string accessToken, Action<bool, LoginMessage> callBack);
    /// <summary>
    /// 游客登录  
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="username"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool LoginGuest(string uuid, string username, Action<bool, LoginMessage> callBack);
    /// <summary>
    /// 绑定平台帐号
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="accessToken"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool BindAccount(string uuid, string accessToken, Action<bool, LoginMessage> callBack);
    /// <summary>
    ///判断平台ID(facebookID)是否绑定了帐号 返回json: errcode=0,已绑定；errcode=-1，未绑定
    /// </summary>
    /// <param name="accountID"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool GetIsBind(string accountID, Action<bool, NetMessage> callBack);
    /// <summary>
    /// 获取好友邀请进度
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    bool GetInviteProgress(Action<bool, InviteProgressMessage> callBack);
    /// <summary>
    /// 设置好友邀请进度 每次邀请时把获取到的可邀请好友数量传给服务器
    /// </summary>
    /// <returns></returns>
    bool SetInviteProgress(int limit, Action<bool, NetMessage> callBack);
    /// <summary>
    /// 获取可召回好友列表
    /// </summary>
    /// <param name="isShared">（0：没有进行分享,1：分享过）</param>
    /// <returns></returns>
    bool GetRecallableFriends(Action<bool, RecallableFriendsMessage> callBack);

    /// <summary>
    /// 邀请好友
    /// </summary>
    /// <param name="reqid">邀请后获得的requestID</param>
    /// <param name="to">邀请的好友列表</param>
    /// <returns></returns>
    /// {"errcode":0,"errmsg":"","data":{"energy":119,"money":16957,"invite_limit_reward":true,"invite_energy_reward":20,"invite_reward_num_limit":200,"invite_reward_num_limit_daily":1,"invite_times_daily":1,"reward_list":[{"type":"energy","num":20,"name":""}]}}
    bool InviteFriends(string reqid, string[] to, Action<bool, InviteFriendsMessage> callBack);

    /// <summary>
    /// 召回好友
    /// </summary>
    /// <param name="limit">GetRecallableFriends 获取到的可召回好友的数量</param>
    /// <param name="to"></param>
    /// <returns></returns>
    /// {"errcode":0,"errmsg":"send success","data":{"energy":646,"money":315947502,"recall_limit_reward":false,"recall_reward_num_limit_daily":2,"recall_times_daily":1,"reward_list":[{"type":"energy","num":5,"name":""}]}}
    bool RecallFriends(int limit, string[] to, Action<bool, RecallFriendsMessage> callBack);

    //------------支付-------------------
    bool Purchase(string store, string transactionID, string payload, string orderID, Action<bool, NetMessage> callBack);
    bool GetOrder(string itemId, int itemNum, Action<bool, OrderMessage> callBack);

}

