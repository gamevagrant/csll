﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildData {

    public long money;                   // 金币数量
    public int maxEnergy;                     // 最大体力值
    public int energy;                       // 当前剩余体力值
    public int recoverEnergy;                // 体力恢复值
    public int timeToRecover;                 // 体力恢复剩余时间
    public int islandId;              // 岛屿ID
    public BuildingData[] buildings;                     // 建筑信息  // Building 数据结构在上面
    public int crowns;                     // 星星数量
    public string mapInfo;                    // 暂时未用到
    public int[][] buildingCost;                  // 岛屿上各物体建造费用
    public int[][] buildingRepairCost;         // 岛屿上各物体维修费用 
    public bool gainIslandReward;             // 暂时未用到
    public bool canIslandShare;               // 暂时未用到
    public bool playUpgradeAnimation;        // 暂时未用到
    public int upgradeEnergyAfterReward;       // 暂时未用到
    public int upgradeMoneyAfterReward;      // 暂时未用到
    public RollerItemData[] rollerItems;                   // 转盘items   // RollerItem 数据结构在上面
    public long tutorial;                    // 新手教程
}
