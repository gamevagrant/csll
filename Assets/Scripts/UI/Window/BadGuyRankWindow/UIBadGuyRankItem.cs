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

    public Button wantedBtn;

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
        attackNumText.text = badGuy.attactTimes.ToString();

    }

    private void OnClickWantedBtn()
    {

    }
}
