/// <summary>
/// 公告
/// </summary>
public class AnnouncementData {

    public string click_img_to;//点击置顶图片公告跳转的地址
    public string img_url;//置顶图片公告地址
    public AnnouncementContentData[] sections;//子公告列表

}

/// <summary>
/// 子公告
/// </summary>
public class AnnouncementContentData
{
    public string sub_title;
    public AnnouncementSubContent[] content;
}

public class AnnouncementSubContent
{
    public int color;
    public string text;
}
