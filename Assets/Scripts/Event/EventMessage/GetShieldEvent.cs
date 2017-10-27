using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetShieldEvent : BaseEvent {

    public System.Action<Vector3> emptyShieldPos;//空的盾牌格子坐标
    public float delay;//delay 延迟后刷新盾牌数据

    public GetShieldEvent(System.Action<Vector3> emptyShieldPos,float delay) :base(EventEnum.GET_SHIELD)
    {
        this.emptyShieldPos = emptyShieldPos;
        this.delay = delay;
    }
}
