using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStarPosEvent : BaseEvent {

    public System.Action<Vector3> starPos;//显示星星位置的坐标



    public GetStarPosEvent(System.Action<Vector3> starPos) :base(EventEnum.GET_STAR)
    {
        this.starPos = starPos;
    }
}
