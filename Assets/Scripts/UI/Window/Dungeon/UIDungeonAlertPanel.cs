using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIDungeonAlertPanel : MonoBehaviour {

    public TextMeshProUGUI mainText;
    public TextMeshProUGUI subText;
    public CounterControler counter;
    public QY.UI.Button btnCancel;
    public QY.UI.Button btnOk;
    public QY.UI.Button btnClose;
    public Image icon;
    public Sprite[] iconSprites;

    private Action<int> onClickOkWithInt;
    private Action onClickOk;

    public void Alert(string title,Action onClickOk)
    {
        mainText.text = title;
        icon.enabled = false;

        this.onClickOk = onClickOk;
        this.onClickOkWithInt = null;

        counter.gameObject.SetActive(false);
        subText.gameObject.SetActive(false);
        btnCancel.gameObject.SetActive(true);
        btnOk.gameObject.SetActive(true);
        btnClose.gameObject.SetActive(false);
    }
    /// <summary>
    /// 食卡鱼不足
    /// </summary>
    /// <param name="title"></param>
    /// <param name="des"></param>
	public void CardFishNotEnough(string title,string des)
    {
        mainText.text = title;
        subText.text = des;
        icon.enabled = true;
        icon.sprite = iconSprites[1];
        

        onClickOkWithInt = null;
        onClickOk = null;

        counter.gameObject.SetActive(false);
        subText.gameObject.SetActive(true);
        btnCancel.gameObject.SetActive(false);
        btnOk.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(true);
    }
    /// <summary>
    /// 使用食卡鱼
    /// </summary>
    /// <param name="title"></param>
    /// <param name="maxCount"></param>
    /// <param name="onClickOk"></param>
    public void UseCardFish(string title,int maxCount,System.Action<int> onClickOk)
    {
        icon.enabled = true;
        icon.sprite = iconSprites[1];
       
        counter.min = 1;
        counter.max = maxCount;
        mainText.text = title;
        subText.text = "剩余：" + maxCount.ToString();

        this.onClickOkWithInt = onClickOk;
        this.onClickOk = null;

        counter.gameObject.SetActive(true);
        subText.gameObject.SetActive(true);
        btnCancel.gameObject.SetActive(true);
        btnOk.gameObject.SetActive(true);
        btnClose.gameObject.SetActive(false);
    }
    /// <summary>
    /// 使用万能牌
    /// </summary>
    /// <param name="title"></param>
    /// <param name="maxCount"></param>
    /// <param name="onClickOk"></param>
    public void UseMasterCard(string title,int maxCount,System.Action onClickOk)
    {
        icon.enabled = true;
        icon.sprite = iconSprites[0];
        mainText.text = title;
        subText.text = "剩余：" + maxCount.ToString();

        this.onClickOk = onClickOk;
        this.onClickOkWithInt = null;

        counter.gameObject.SetActive(false);
        subText.gameObject.SetActive(true);
        btnCancel.gameObject.SetActive(true);
        btnOk.gameObject.SetActive(true);
        btnClose.gameObject.SetActive(false);
    }

    public void OnClickCancleBtn()
    {

        UIDungeonPopupPanels.instance.ClosePanel(this.transform as RectTransform);
    }

    public void OnClickOkBtn()
    {
        if(onClickOkWithInt !=null && counter.num>0)
        {
            onClickOkWithInt(counter.num);
        }else if(onClickOk!=null)
        {
            onClickOk();
        }
    }
}
