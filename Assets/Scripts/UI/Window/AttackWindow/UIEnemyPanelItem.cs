using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyPanelItem : BaseItemView {

    public HeadIcon headIcon;
    public Text tipsLabel;
    public Text attCountLabel;
    public Button attBtn;
    public GameObject img300k;
    public GameObject attImage;
    public GameObject nullImage;
    public Sprite[] btnSprites;

    public SelectPlayerData selectData;

    public override void SetData(object data)
    {
        selectData = (SelectPlayerData)data;

        headIcon.setData(selectData.name, selectData.headImg, selectData.crowns, selectData.isVip);
        if(selectData.attactTimes>0)
        {
            tipsLabel.gameObject.SetActive(true);
            attImage.SetActive(true);

            tipsLabel.text = "此人攻击过你";
            attCountLabel.text = selectData.attactTimes.ToString();
        }
        else if(selectData.stealMoney>0)
        {
            tipsLabel.gameObject.SetActive(true);
            attImage.SetActive(false);

            tipsLabel.text = string.Format("偷取了你{0}金币", GameUtils.GetCurrencyString(selectData.stealMoney));
            
        }else
        {
            tipsLabel.gameObject.SetActive(false);
            attImage.SetActive(false);
        }

        if(selectData.isEmpty)
        {
            nullImage.SetActive(true);
        }else
        {
            nullImage.SetActive(false);
        }
        if(selectData.isWanted)
        {
            img300k.SetActive(true);
        }else
        {
            img300k.SetActive(false);
        }

        if(selectData.isRandomUser)
        {
            if(selectData.isSelected)
            {
                attBtn.image.sprite = btnSprites[1];
            }
            else
            {
                attBtn.image.sprite = btnSprites[0];
            }
           
        }else
        {
            if (selectData.isSelected)
            {
                attBtn.image.sprite = btnSprites[3];
            }
            else
            {
                attBtn.image.sprite = btnSprites[2];
            }
        }
    }
}
