//简易用户数据 用以保存在本地
public class SimpleUserData {

    public string uuid;//游客ID
    public string name;
    public int level;

    public string ToJson()
    {
        return LitJson.JsonMapper.ToJson(this);
    }

    public static SimpleUserData Create(string json)
    {
        return LitJson.JsonMapper.ToObject<SimpleUserData>(json);
    }
}
