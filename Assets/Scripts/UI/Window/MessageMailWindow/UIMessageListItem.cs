using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMessageListItem : BaseItemView {

    public Image headFrame;
    public RawImage headImage;
    public TextMeshProUGUI startCountText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI contentText;
    public Button button;
    public Sprite[] headFrameSprites;

    
    override public void SetData(object obj)
    {
        MessageResponseData data = (MessageResponseData)obj;
        headFrame.sprite = data.isVip ? headFrameSprites[1] : headFrameSprites[0];
        timeText.text = data.time;
        startCountText.text = data.crowns.ToString();
        
        if(data.action == 1)//被攻击
        {
            string str = "";
            if ((bool)data.extra["isShielded"])
            {
                str = string.Format("你成功防御了<font=\"FZLanTYJW_Da SDF\" material=\"FZLanTYJW_Da SDF Shadow & RedOutline\"><color=white><size=130%>{0}</size></color></font>的攻击", data.name);
                button.gameObject.SetActive(false);
            }
            else
            {
                if ((int)data.extra["building"]["status"] == 2)
                {
                    str = string.Format("<font=\"FZLanTYJW_Da SDF\" material=\"FZLanTYJW_Da SDF Shadow & RedOutline\"><color=white><size=130%>{0}</size></color></font>损坏了你的{1}", data.name, GameEnumeConfig.GetBuildingName((int)data.extra["building_index"]));
                }
                else
                {
                    str = string.Format("<font=\"FZLanTYJW_Da SDF\" material=\"FZLanTYJW_Da SDF Shadow & RedOutline\"><color=white><size=130%>{0}</size></color></font>摧毁了你的{1}", data.name, GameEnumeConfig.GetBuildingName((int)data.extra["building_index"]));
                }
                button.gameObject.SetActive(true);
            }
            contentText.text = str;
        }
        else if(data.action == 2)//被偷窃
        {
            string str = "";
            str = string.Format("<font=\"FZLanTYJW_Da SDF\" material=\"FZLanTYJW_Da SDF Shadow & RedOutline\"><color=white><size=130%>{0}</size></color></font>偷走了{1}金币", data.name, data.extra["reward"]);
            button.gameObject.SetActive(true);
            contentText.text = str;
        }
        else if(data.action == 6)//被通缉
        {
            string str = "";
            str = string.Format("你被<font=\"FZLanTYJW_Da SDF\" material=\"FZLanTYJW_Da SDF Shadow & RedOutline\"><color=white><size=130%>{0}</size></color></font>通缉了", data.name);
            button.gameObject.SetActive(false);
            contentText.text = str;
        }

        AssetLoadManager.Instance.LoadAsset<Texture2D>(data.headImg, (tex) =>
        {
            headImage.texture = tex;
        });
    }
}
