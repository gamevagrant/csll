

public class AddGuesMessage: NetMessage
{
    public string result;
    public string bntoken;
    public AddGuestData UserInfo;

}

public class AddGuestData
{
    public int Id;
    public string OpenId;
    public string HeadImageUrl;
}
