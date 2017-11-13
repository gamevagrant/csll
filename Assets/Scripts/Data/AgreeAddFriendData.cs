
/// <summary>
/// 好友列表，未同意的好友列表，好友的好友列表
/// </summary>
public class AgreeAddFriendData
{

    //[]* FriendOfFriend        fofs                                   // 好友的好友列表   // FriendOfFriend 数据结构在上面
    public FriendData[] friends;                                // 好友列表         // Friend 数据结构在上面
    public FriendData[] friendsNotAgree;                        // 未通过好友列表   // Friend 数据结构在上面
}
