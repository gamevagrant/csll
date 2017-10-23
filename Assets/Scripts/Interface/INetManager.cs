
using System;

public interface INetManager {

    bool Login(long userid, Action<bool, LoginMessage> callBack);
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
    bool Steal(int idx,Action<bool, StealMessage> callBack);
}
