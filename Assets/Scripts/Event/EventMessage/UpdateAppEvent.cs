using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAppEvent : BaseEvent {

    public string url;
    public UpdateAppEvent(string url):base(EventEnum.NEED_UPDATE_APP)
    {
        this.url = url;
    }
}
