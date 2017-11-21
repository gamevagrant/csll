using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WantedMessage : NetMessage {

    public WantedData data;
}

public class WantedData
{
    public BadGuyData otherData;
    public int wantedCount;
}
