/// <summary>
/// 注册帐号数据
/// </summary>
public class RegisterMessage : NetMessage {

    public RegisterData data;
}

public class RegisterData
{
    public string openid;
    public string token;
}
