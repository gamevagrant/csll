
/// <summary>
/// 建造成功
/// </summary>
public class BuildComplateEvent : BaseEvent {

    public int islandID;
    public int buildIndex;
    public int level;
    public int status;
    public bool isRepair;
    public bool isUpgrade;                  //是否升级
    public int upgradeEnergyReward;        // 升级后能量奖励
    public long upgradeMoneyReward;        // 升级后金币奖励

    public BuildComplateEvent():base(EventEnum.BUILD_COMPLATE)
    {

    }
}
