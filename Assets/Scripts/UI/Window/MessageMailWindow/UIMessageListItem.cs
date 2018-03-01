using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMessageListItem : BaseItemView {

    private enum BtnState
    {
        TongJi,
        ChaKan,
    }

    public Image headFrame;
    public RawImage headImage;
    public TextMeshProUGUI startCountText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI contentText;
    public QY.UI.Button button;
    public Sprite[] headFrameSprites;
    public Sprite[] buttonSprite;

    MessageResponseData data;

    override public void SetData(object obj)
    {
        data = (MessageResponseData)obj;
        headFrame.sprite = data.isVip ? headFrameSprites[1] : headFrameSprites[0];
        timeText.text = data.time;
        startCountText.text = data.crowns.ToString();
        SetBtnState(BtnState.TongJi);
        button.interactable = true;
        if (data.action == 1)//被攻击
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
            button.gameObject.SetActive(false);
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
        }
        else if(data.action == 18)//收到副本邀请
        {
            /*
       {
        "uid": 423,
        "toid": 0,
        "action": 18,
        "result": 0,
        "time": "今天 09:48:23",
        "name": "Margaret",
        "headImg": "https://scontent.xx.fbcdn.net/v/t1.0-1/c15.0.50.50/p50x50/1379841_10150004552801901_469209496895221757_n.jpg?oh=63882fb3fcd87e9433dcbb490913a150\u0026oe=5B064733",
        "crowns": 2,
        "extra": {
          "createTime": 1519696099
        },
        "read": true,
        "isWanted": false,
        "isVip": false,
        "head_frame": 0
      }
             
            */

            string str = "";
            str = string.Format("<color=#BA7F00FF><size=110%>{0}</size></color>邀请你一起挑战蚌精！", data.name);
            button.gameObject.SetActive(true);
            SetBtnState(BtnState.ChaKan);
            button.interactable = data.result == 0 ? true : false;
            contentText.text = str;
        }
        else if(data.action == 19)//好友帮助抽到牌
        {
            /*
                   {
                    "uid": 422,
                    "toid": 0,
                    "action": 19,
                    "result": 0,
                    "time": "今天 11:34:55",
                    "name": "Tom",
                    "headImg": "https://scontent.xx.fbcdn.net/v/t1.0-1/c15.0.50.50/p50x50/10354686_10150004552801856_220367501106153455_n.jpg?oh=58094ae96718bcfcbd53cfea5151eaf5\u0026oe=5B137D2F",
                    "crowns": 2,
                    "extra": {
                      "card_type": 1,
                      "createTime": 1517884853
                    },
                    "read": true,
                    "isWanted": false,
                    "isVip": false,
                    "head_frame": 0
                  }
            */
            string str = "";
            str = string.Format("<color=#BA7F00FF><size=110%>{0}</size></color>帮你抽到一张{1}牌！", data.name, int.Parse(data.extra["card_type"].ToString())==0?"小":"大");
            button.gameObject.SetActive(true);
            SetBtnState(BtnState.ChaKan);
            button.interactable = data.result == 0 ? true : false;
            contentText.text = str;
        }
        else
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
        if(data.action == 18)
        {
            GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UIMessageMailWindow);
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIDungeonLottoWindow, data.uid, int.Parse(data.extra["createTime"].ToString()));
        }else if(data.action == 19)
        {
            GameMainManager.instance.netManager.DungeonCheckLottoMsg(int.Parse(data.extra["createTime"].ToString()),data.uid,(ret,res)=>
            {
                if(res.isOK)
                {
                    GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UIMessageMailWindow);
                    GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIDungeonWindow);
                }else
                {
                    button.interactable = false;
                }
            });
        }else
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

    private void SetBtnState(BtnState btnState)
    {
        switch(btnState)
        {
            case BtnState.TongJi:
                button.image.sprite = buttonSprite[0];
                button.spriteState = new SpriteState() { disabledSprite = buttonSprite[1] };
                break;
            case BtnState.ChaKan:
                button.image.sprite = buttonSprite[2];
                button.spriteState = new SpriteState() { disabledSprite = buttonSprite[3] };
                break;
        }
       // button.image.SetNativeSize();
    }
}
