using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIEveryDayRewardWeekilyItem : BaseItemView {

    [SerializeField]
    private QY.UI.Button iconBtn;
    [SerializeField]
    private GameObject protect;

    private DailyPrizeConfData data;

    private RotateShake _shake;
    private RotateShake shake
    {
        get
        {
            if(_shake==null)
            {
                _shake = iconBtn.gameObject.AddComponent<RotateShake>();
                _shake.speed = 4;
                _shake.angles = 20;
            }
            return _shake;
        }
    }
    // Use this for initialization
    void Start () {


    }

    public override void SetData(object data)
    {
        iconBtn.gameObject.SetActive(data != null);
        protect.gameObject.SetActive(data != null);

        this.data = data as DailyPrizeConfData;
        switch(this.data.status)
        {
            case 0://不可领取
                iconBtn.interactable = true;
                protect.SetActive(true);
                iconBtn.image.SetNativeSize();
                shake.enabled = false;
                (iconBtn.transform as RectTransform).anchoredPosition = new Vector2(0, 30);
                break;
            case 1://可领取
                iconBtn.interactable = true;
                protect.SetActive(false);
                iconBtn.image.SetNativeSize();
                shake.enabled = true;
                (iconBtn.transform as RectTransform).anchoredPosition = new Vector2(0, 30);
                break;
            case 2://已领取
                iconBtn.interactable = false;
                protect.SetActive(false);
                iconBtn.image.SetNativeSize();
                shake.enabled = false;
                (iconBtn.transform as RectTransform).anchoredPosition = new Vector2(10, 30);
                break;
        }

    }

    public void OnClickGetRewardBtn()
    {
        GameMainManager.instance.netManager.GetWeeklyLoginReward(data.day, (ret, res) => {
            if (res.isOK)
            {
                GetRewardWindowData rewardData = new GetRewardWindowData();
                rewardData.reward = new RewardData
                {
                    type = res.data.prize[0].type,
                    num = res.data.prize[0].num
                };
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, rewardData);
                res.data.prize[0].status = 2;
                SetData(res.data.prize[0]);

            }


        });
    }
}
