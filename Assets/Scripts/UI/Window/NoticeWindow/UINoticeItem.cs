using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UINoticeItem : MonoBehaviour {

    public TextMeshProUGUI titleLabel;
    public TextMeshProUGUI contentLabel;

    private string[] colors = new string[] { "#FFFFFFFF", "#3C3DFFFF" , "#00940DFF", "#BA7F00FF", "#DB4400FF" };
    public void SetData(AnnouncementContentData data)
    {
        titleLabel.text = data.sub_title;
        string str = "";
        for(int i = 0;i<data.content.Length;i++)
        {
            str += string.Format("<{0}>{1}</color>",data.content[i].color<colors.Length?colors[data.content[i].color]:colors[1], data.content[i].text);
        }
        contentLabel.text = str;
    }
}
