using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIRankItem : BaseItemView {

    public HeadIcon head;
    public Image rankImage;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rankText;
    public Sprite[] sprites;
    public GameObject item1;
    public GameObject item2;

    private Image backGround;

    private FriendData friend;

    private void Awake()
    {
        backGround = GetComponent<Image>();
    }

    public override void SetData(object data)
    {
        item1.SetActive(data != null);
        item2.SetActive(data == null);
        if(data!=null)
        {
            friend = data as FriendData;
            head.setData(friend.name, friend.headImg, friend.crowns, friend.isVip);
            rankImage.sprite = sprites[Mathf.Max(0, Mathf.Min(3, friend.rank - 1))];
            timeText.text = friend.updateTime;
            rankImage.SetNativeSize();
            if (friend.rank < 4)
            {
                rankText.enabled = false;
                rankImage.enabled = true;
            }
            else if (friend.rank < 99)
            {
                rankText.enabled = true;
                rankImage.enabled = true;
                rankText.text = friend.rank.ToString();
            }
            else
            {
                rankText.enabled = true;
                rankImage.enabled = false;
                rankText.text = friend.rank.ToString();
            }

            if (string.IsNullOrEmpty(friend.updateTime))
            {
                nameText.alignment = TextAlignmentOptions.MidlineLeft;
            }
            else
            {
                nameText.alignment = TextAlignmentOptions.TopLeft;
            }

            if (friend.uid == GameMainManager.instance.model.userData.uid)
            {
                backGround.color = new Color(233 / 255f, 186 / 255f, 116 / 255f, 1);
            }
            else
            {
                backGround.color = new Color(1, 1, 1, 1);
            }
        }
       
    }

    public void OnCLickRecruitBtn()
    {
        GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UIRankWindow);
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIInviteWindow);
    }

    public void OnClickHeadBtn()
    {
        OtherData otherData = new OtherData();
        otherData.buildings = friend.buildings;
        otherData.name = friend.name;
        otherData.crowns = friend.crowns;
        otherData.headImg = friend.headImg;
        otherData.isVip = friend.isVip;
        otherData.islandId = friend.islandId;
        otherData.uid = friend.uid;
        otherData.isFriend = true;
        GameMainManager.instance.uiManager.ChangeState(new CheckPlayerState(otherData));

    }
}
