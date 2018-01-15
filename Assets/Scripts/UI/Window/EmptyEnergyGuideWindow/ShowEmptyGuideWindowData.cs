using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEmptyGuideWindowData  {


    public enum PanelType
    {
        GuideReward,
        EmptyEnergy,
    }

    public PanelType type;

    public ShowEmptyGuideWindowData(PanelType type)
    {
        this.type = type;
    }
}
