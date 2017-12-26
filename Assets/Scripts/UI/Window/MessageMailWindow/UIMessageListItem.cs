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
    public QY.UI.Button button;
    public Sprite[] headFrameSprites;

    MessageResponseData data;

    override public void SetData(object obj)
    {
        data = (MessageResponseData)obj;
        headFrame.sprite = data.isVip ? headFrameSprites[1] : headFrameSprites[0];
        timeText.text = data.time;
        startCountText.text = data.crowns.ToString();
        
        if(data.action == 1)//被攻击
        {
            string str = "";
            if ((bool)data.extra["isShielded"])
            {
                str = string.Format("你成功防御了<color=#BA7F00FF><size=110%>{0}</size></color>的攻击", data.name);
                button.gameObject.SetActive(false);
            }
            else
            {
                if ((int)data.extra["building"]["status"] == 2)
                {
                    str = string.Format("<color=#BA7F00FF><size=110%>{0}</size></color>损坏了你的{1}", data.name, GameMainManager.instance.configManager.islandConfig.GetBuildingName((int)data.extra["building_index"]));
                }
                else
                {
                    str = string.Format("<color=#BA7F00FF><size=110%>{0}</size></color>摧毁了你的{1}", data.name, GameMainManager.instance.configManager.islandConfig.GetBuildingName((int)data.extra["building_index"]));
                }
                button.gameObject.SetActive(true);
            }
            contentText.text = str;
        }
        else if(data.action == 2)//被偷窃
        {
            string str = "";
            str = string.Format("<color=#BA7F00FF><size=110%>{0}</size></color>偷走了{1}金币", data.name, data.extra["reward"]);
            button.gameObject.SetActive(true);
            contentText.text = str;
        }
        else if (data.action == 5)//通缉
        {
            string str = "";
            str = string.Format("<color=#BA7F00FF><size=110%>{0}</size></color>正在通缉<color=#BA7F00FF><size=110%>{1}</size></color>,帮助好友攻击可以获得300k奖金", data.name, data.extra["name"]);
            button.gameObject.SetActive(false);
            contentText.text = str;
        }
        else if(data.action == 6)//被通缉
        {
            string str = "";
            str = string.Format("<color=#BA7F00FF><size=110%>{0}</size></color>正在通缉你，战斗号角已经吹响！", data.name);
            button.gameObject.SetActive(false);
            contentText.text = str;
        }else
        {
            string str = "";
            button.gameObject.SetActive(false);
            contentText.text = str;
        }

        AssetLoadManager.Instance.LoadAsset<Texture2D>(data.headImg, (tex) =>
        {
            headImage.texture = tex;
        });
    }

    public void OnClickWantedBtn()
    {
        if (GameMainManager.instance.model.userData.wantedCount > 0)
        {
            Alert.Show("确认使用1个通缉令吗？", Alert.OK | Alert.CANCEL, (btn) =>
            {
                if (btn == Alert.OK)
                {
                    GameMainManager.instance.netManager.Wanted(data.uid, (ret, res) =>
                    {
                        if (res.isOK)
                        {
                            button.gameObject.SetActive(false);


                        }
                    });
                }
            });
        }
        else
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIBuyWantedWindow);
        }
    }
}
