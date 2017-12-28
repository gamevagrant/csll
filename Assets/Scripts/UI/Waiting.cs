using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiting {

    public static void Enable()
    {
        GameMainManager.instance.uiManager.isWaiting = true;
    }

    public static void Disable()
    {
        GameMainManager.instance.uiManager.isWaiting = false;
    }
}
