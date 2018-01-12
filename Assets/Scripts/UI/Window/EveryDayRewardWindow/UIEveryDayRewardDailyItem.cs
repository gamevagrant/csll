using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIEveryDayRewardDailyItem :BaseItemView {

    [SerializeField]
    private Image icon;
    [SerializeField]
    private GameObject getMask;
    [SerializeField]
    private TextMeshProUGUI numText;
    [SerializeField]
    private Sprite[] iconSprites;

    private DailyPrizeConfData data;
    // Use this for initialization
    void Start () {
		
	}

    public override void SetData(object data)
    {
        icon.gameObject.SetActive(data != null);
        getMask.SetActive(data != null);
        numText.gameObject.SetActive(data != null);


        this.data = data as DailyPrizeConfData;
        numText.text = GameUtils.GetShortMoneyStr(this.data.num);
        if (this.data.type == "gold")
        {
            icon.sprite = iconSprites[0];
        }else
        {
            icon.sprite = iconSprites[1];
        }
        switch (this.data.status)
        {
            case 0:
            case 1:
                getMask.SetActive(false);
                break;
            case 2:
                getMask.SetActive(true);
                break;
        }
    }
}
