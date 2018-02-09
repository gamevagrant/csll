using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCompleteTutorialMessage : NetMessage {

    public DungeonCompleteTutorialMessageData data;
    public class DungeonCompleteTutorialMessageData
    {
        public int dungeon_keys;
        public int dungeon_tutorial;
    }
}
