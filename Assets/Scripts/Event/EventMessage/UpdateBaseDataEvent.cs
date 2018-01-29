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
        vip,//vip
        island,//岛屿升级
    }

    public UpdateType updateType;
    public float delay;
    public UpdateBaseDataEvent(UpdateType type,float delay) : base(EventEnum.UPDATE_BASE_DATA)
    {
        this.updateType = type;
        this.delay = delay;
    }
}
