using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFirstBuyingRewardItem : MonoBehaviour {

    public Image image;
    public TextMeshProUGUI text;
    public Sprite[] sprites;

	public void SetData(RewardData reward)
    {
        if(reward == null)
        {
            image.enabled = false;
            text.text = "";
        }else
        {
            image.enabled = true;
            switch (reward.type)
            {
                case "gold":
                    image.sprite = sprites[0];
                    break;
                case "energy":
                    image.sprite = sprites[1];
                    break;
            }

            text.text = GameUtils.GetShortMoneyStr(reward.num);
        }
        
    }
	

}
