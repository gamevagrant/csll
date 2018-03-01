using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardItem : MonoBehaviour {

    public Image icon;
    public TextMeshProUGUI text;
    [Header("0：金币 1：能量 2：vip 3：通缉令 4：万能碎片 5：食卡鱼 6：万能卡 7: 麻将 8：副本钥匙")]
    public Sprite[] iconSprites;
    // Use this for initialization
    public void SetData(RewardData data)
    {
        text.text = data.num.ToString();
        switch (data.type)
        {

            case "money":
                icon.sprite = iconSprites[0];
                text.text = GameUtils.GetShortMoneyStr(data.num);
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
            case "master_piece":
                icon.sprite = iconSprites[4];
                break;
            case "card_fish":
                icon.sprite = iconSprites[5];
                break;
            case "master_card":
                icon.sprite = iconSprites[6];
                break;
            case "master_tile":
                icon.sprite = iconSprites[7];
                gameObject.SetActive(false);
                break;
            case "dungeon_keys":
                if(iconSprites.Length>8)
                {
                    icon.sprite = iconSprites[8];
                }
                break;
            
            default:
                icon.sprite = iconSprites[6];
                break;
        }
    }
}
