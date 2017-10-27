
/// <summary>
/// 建造成功
/// </summary>
public class BuildComplateEvent : BaseEvent {

    public int islandID;
    public int buildIndex;
    public int level;
    public bool isUpgrade;

    public BuildComplateEvent():base(EventEnum.BUILD_COMPLATE)
    {

    }
}
