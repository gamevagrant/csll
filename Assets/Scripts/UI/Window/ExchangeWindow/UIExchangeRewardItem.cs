using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIExchangeRewardItem : BaseItemView {

    public Image icon;
    public TextMeshProUGUI text;
    public TextMeshProUGUI numText;
    public Sprite[] iconSprites;//

    private RewardData data;
    public override void SetData(object data)
    {
        this.data = data as RewardData;
        numText.text = "";
        text.text = this.data.num.ToString();
        switch (this.data.type)
        {
            
            case "money":
                icon.sprite = iconSprites[0];
                text.text = GameUtils.GetShortMoneyStr(this.data.num);
                break;
            case "energy":
                icon.sprite = iconSprites[1];
                break;
            case "vip":
                icon.sprite = iconSprites[2];
                break;
            case "wanted":
                icon.sprite = iconSprites[3];
                break;
            case "piece1":
            case "piece2":
            case "piece3":
            case "piece4":
            case "piece5":
            case "piece6":
            case "piece7":
            case "piece8":
            case "piece9":
                numText.text = this.data.type.Replace("piece", "");
                icon.sprite = iconSprites[4];
                break;
            case "master_piece":
                icon.sprite = iconSprites[5];
                break;
            case "card_fish":
                icon.sprite = iconSprites[6];
                break;
            case "master_card":
                icon.sprite = iconSprites[7];
                break;
            case "dungeon_keys":
                icon.sprite = iconSprites[5];
                break;
            default:
                icon.sprite = iconSprites[5];
                break;
        }
    }
}
