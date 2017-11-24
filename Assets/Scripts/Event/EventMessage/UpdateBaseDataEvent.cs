using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBaseDataEvent :BaseEvent {

    public enum UpdateType
    {
        Money,//金币
        Energy,//能量
        star,//星星
        sheild,//盾牌
        wanted,//通缉令
    }

    public UpdateType type;
    public float delay;
    public UpdateBaseDataEvent(UpdateType type,float delay) : base(EventEnum.UPDATE_BASE_DATA)
    {
        this.type = type;
        this.delay = delay;
    }
}
