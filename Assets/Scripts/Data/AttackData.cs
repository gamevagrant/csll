using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击后返回的数据
/// </summary>
public class AttackData{
    public long money;                       // 当前金钱
    public bool isShielded;                // 是否有护盾
    public bool isMiniShielded;             // 是否有小护盾
    public bool isWanted;              // 暂时未使用
    public bool isBear;                // 暂时未使用
    public bool isSeal;                  // 暂时未使用
    public int reward;                  // 攻击奖励
    public bool isNewCannonContestScoreAward;  // 暂时未使用
    public int betCount;                  // 暂时未使用
    public AttackTargetData attackTarget;                   // 攻击对象  // AttackTarget 数据结构在上面
    public long tutorial;                  // 新手教程
}
