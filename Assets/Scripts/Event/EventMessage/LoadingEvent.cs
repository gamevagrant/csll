using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingEvent : BaseEvent {

    public float progress;
    /// <summary>
    /// 加载名称 用于区分不同的加载项
    /// </summary>
    public string name;
    /// <summary>
    /// 加载内容
    /// </summary>
    public string msg;

	public LoadingEvent(string name,float progress,string msg = "") :base(EventEnum.LOADING_PROGRESS)
    {
        this.name = name;
        this.progress = progress;
        this.msg = msg;

    }
}
