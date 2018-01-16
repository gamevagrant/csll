using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowShopWindowData {

	public enum PanelType
    {
        Energy,
        Gold,
        Props,
    }

    public PanelType type;

    public ShowShopWindowData(PanelType type)
    {
        this.type = type;
    }
}
