using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBadGuyRankItem : BaseItemView {

    public Image headFrame;
    public RawImage headImage;
    public Sprite[] sprites;

    public TextMeshProUGUI starText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI tipsText;
    public TextMeshProUGUI attackNumText;

    public QY.UI.Button wantedBtn;

    BadGuyData badGuy;
    // Use this for initialization
    private void Awake()
    {
        wantedBtn.onClick.AddListener(OnClickWantedBtn);
    }

    private void OnDestroy()
    {
        wantedBtn.onClick.RemoveAllListeners();
    }

    override public void SetData(object obj)
    {
        badGuy = obj as BadGuyData;
        headFrame.sprite = badGuy.isVip ? sprites[1] : sprites[0];
        AssetLoadManager.Instance.LoadAsset<Texture2D>(badGuy.headImg, (tex) =>
        {
            headImage.texture = tex;
        });
        starText.text = badGuy.crowns.ToString();
        nameText.text = badGuy.name;
        tipsText.text = badGuy.isWanted ? "已经被通缉！" : "还在逍遥法外！";
        wantedBtn.interactable = !badGuy.isWanted;
        attackNumText.text = badGuy.attactTimes.ToString();

    }

    private void OnClickWantedBtn()
    {
        if(GameMainManager.instance.model.userData.wantedCount>0)
        {
            Alert.Show("确认使用1个通缉令吗？",Alert.OK|Alert.CANCEL , (btn) =>
            {
                if(btn == Alert.OK)
                {
                    GameMainManager.instance.netManager.Wanted(badGuy.uid, (ret, res) =>
                    {
                        if(res.isOK)
                        {
                            SetData(res.data.otherData);


                        }
                    });
                }
            });
        }else
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIBuyWantedWindow);
        }
        
    }

    public void OnClickHead()
    {
        GameMainManager.instance.netManager.Show(badGuy.uid, (ret,res) =>
        {
            if(res.isOK)
            {
                GameMainManager.instance.uiManager.ChangeState(new CheckPlayerState(res.data.otherData));
            }
            
        });
        
    }
}
