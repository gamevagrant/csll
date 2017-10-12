/// <summary>
/// 系统公告
/// </summary>
public class NoticeItemData {

    public string text;              // 公告文本
    public long send_time;              // 最新一次的发送时间
    public long start_time;           // 开始发送的时间
    public long end_time;            // 结束时间
    public int interval;           // 每次发送的时间间隔
    public int count;               // 前端重复显示的次数
    public string type;                 // 在哪里展示
    public int speed;              // 速度
    public int priority;              // 优先级
}
