using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCompleteMessage : NetMessage {

    public TutorialCompleteData data;
}

public class TutorialCompleteData
{
    public long uid;
    public long money;
    public int energy;
    public int timeToRecover;
    public int tutorial;
}
