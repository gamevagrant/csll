
/// <summary>
/// 建造成功
/// </summary>
public class BuildComplateEvent : BaseEvent {

    public int buildIndex;
    public int level;

    public BuildComplateEvent():base(EventEnum.BUILD_COMPLATE)
    {

    }
}
