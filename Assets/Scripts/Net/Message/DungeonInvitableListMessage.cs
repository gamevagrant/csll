using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInvitableListMessage : NetMessage {

    public DungeonInvitableListMessageData data;
    public class DungeonInvitableListMessageData
    {
        public FriendData[] friend_list;
    }
}
