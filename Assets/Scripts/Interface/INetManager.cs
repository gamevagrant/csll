
using System;

public interface INetManager
{

    bool Login(string openid, Action<bool, LoginMessage> callBack);
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
}

