

public class InviteProgressMessage : NetMessage
{
    public InviteProgress data;

    public class InviteProgress
    {
        public int limit;//进度总数
        public int times;//当前进度
    }
}


