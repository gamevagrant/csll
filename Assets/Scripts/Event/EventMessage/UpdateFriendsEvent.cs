using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFriendsEvent : BaseEvent {

    public enum UpdateType
    {
        SendEnergy,
        ReceiveEnergy,
        IgnoreFriend,
        RemoveFriend,
        AgreeFriend,
    }

    public UpdateType updateType;

	public UpdateFriendsEvent(UpdateType type) :base(EventEnum.UPDATE_FRIENDS)
    {
        this.updateType = type;
    }
}
