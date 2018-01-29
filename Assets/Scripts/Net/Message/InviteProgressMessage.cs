

public class InviteProgressMessage : NetMessage
{
    public InviteProgress data;

    public class InviteProgress
    {
        public int limit;//进度总数
        public int times;//当前进度
        public bool is_first;//是否时第一次邀请，第一次邀请奖励50能量
    }
}


