using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetShieldPosEvent : BaseEvent {

    public System.Action<Vector3> emptyShieldPos;//空的盾牌格子坐标

    public GetShieldPosEvent(System.Action<Vector3> emptyShieldPos) :base(EventEnum.GET_SHIELD)
    {
        this.emptyShieldPos = emptyShieldPos;
    }
}
