using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMapMessage : NetMessage {

    public MapinfoMessage data;
}

public class MapinfoMessage
{
    public MapInfoData mapInfo;
}
