

public class BuyMinerMessage : NetMessage {

    public BuyMinerData data;
}

public class BuyMinerData
{
    public int energy;
    public long money;
    public MapInfoData mapInfo;
}
