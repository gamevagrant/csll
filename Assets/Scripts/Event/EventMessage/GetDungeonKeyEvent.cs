using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDungeonKeyEvent : BaseEvent
{

    public System.Action<Vector3> iconPos;

    public GetDungeonKeyEvent(System.Action<Vector3> iconPos) :base(EventEnum.GET_DUNGEON_KEY)
    {
        this.iconPos = iconPos;
    }
}
