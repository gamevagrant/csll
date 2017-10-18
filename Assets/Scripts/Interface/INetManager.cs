
using System;

public interface INetManager {

    bool Login(long userid, Action<bool, LoginMessage> callBack);
    bool Roll(long uid, Action<bool, RollMessage> callBack);
    bool Build(long uid, int islandID, int buildIndex, Action<bool, BuildMessage> callBack);
}
