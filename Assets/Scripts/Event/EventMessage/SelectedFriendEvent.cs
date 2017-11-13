using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedFriendEvent : BaseEvent {

    public Vector3 pos;
    public FriendData friend;

    public SelectedFriendEvent(FriendData friend,Vector3 pos):base(EventEnum.SELECTED_FRIEND)
    {
        this.pos = pos;
        this.friend = friend;
    }
}
