using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFreeRewardItem : BaseItemView {

    [SerializeField]
    private Image[] rewardImage;//energy,piece,card_fish,vip
    [SerializeField]
    private TextMeshProUGUI[] rewardNum;
    [SerializeField]
    private QY.UI.Button getBtn;
    [SerializeField]
    private HeadIcon head;
    [SerializeField]
    private Image inviteImage;
    [SerializeField]
    private Sprite[] btnStateSprites;

    private ShareData.InviteReward data;
    public override void SetData(object data)
    {
        this.data = data as ShareData.InviteReward;

        if(this.data.status == 0)
        {
            head.gameObject.SetActive(false);
            inviteImage.gameObject.SetActive(true);
        }
        else
        {
            head.gameObject.SetActive(true);
            inviteImage.gameObject.SetActive(false);
            head.setData(this.data.username, this.data.avatar, 0, false);
        }

        getBtn.image.sprite = btnStateSprites[this.data.status];
        getBtn.interactable = this.data.status == 1 ? true : false;

        for (int i =0;i<rewardImage.Length;i++)
        {
            rewardImage[i].gameObject.SetActive(false);
            rewardNum[i].gameObject.SetActive(false);
        }
        foreach(RewardData reward in this.data.rewardList)
        {
            switch(reward.type)
            {
                case "energy":
                    rewardImage[0].gameObject.SetActive(true);
                    rewardNum[0].gameObject.SetActive(true);
                    rewardNum[0].text = reward.num.ToString();
                    break;
                case "piece":
                    rewardImage[1].gameObject.SetActive(true);
                    rewardNum[1].gameObject.SetActive(true);
                    rewardNum[1].text = reward.num.ToString();
                    break;
                case "card_fish":
                    rewardImage[2].gameObject.SetActive(true);
                    rewardNum[2].gameObject.SetActive(true);
                    rewardNum[2].text = reward.num.ToString();
                    break;   
                case "vip":
                    rewardImage[3].gameObject.SetActive(true);
                    rewardNum[3].gameObject.SetActive(true);
                    rewardNum[3].text = reward.num.ToString();
                    break;
            }

        }
    }

    public void OnClickGetRewardBtn()
    {
        GameMainManager.instance.netManager.GetInviteReward(this.data.inviteid, (ret, res) =>
         {
             if(res.isOK)
             {
                 data.status = 2;
                 SetData(data);

                 foreach(RewardData reward in data.rewardList)
                 {
                     GetRewardWindowData rewardData = new GetRewardWindowData();
                     rewardData.reward = reward;
                     GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, rewardData);
                 }
                
             }
         });
    }

    public void OnClickInviteBtn()
    {
        GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UIFreeRewardWindow);

        if (AccountManager.instance.isLoginAccount)
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIInviteWindow);
        }
        else
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIFacebookTipsWindow);
        }
    }
}
