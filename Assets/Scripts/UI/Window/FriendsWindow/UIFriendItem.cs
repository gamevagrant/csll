using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.UI;
using TMPro;

public class UIFriendItem : BaseItemView {

    public GameObject item1;
    public GameObject item2;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI recruitRewardText;
    public HeadIcon head;
    public Button sendBtn;
    public Sprite[] sprites;

    FriendItemData friendItemData;
    FriendData friend;
    public override void SetData(object data)
    {
        friendItemData = data as FriendItemData;
        friend = friendItemData.friend;
        if (friendItemData.type == 0)
        {
            item1.SetActive(true);
            item2.SetActive(false);
            recruitRewardText.text = "20";
        }
        else
        {
            item1.SetActive(false);
            item2.SetActive(true);
            head.setData(friend.name, friend.headImg, 0, friend.isVip);
            timeText.text = "上次活跃："+friend.updateTime;
            if(friend.receiveStatus == 1)
            {
                sendBtn.interactable = true;
                sendBtn.image.sprite = sprites[1];
            }else if(friend.sendStatus == 0)
            {
                sendBtn.interactable = true;
                sendBtn.image.sprite = sprites[0];
            }else if (friend.sendStatus == 1 && friend.receiveStatus == 2)
            {
                UnityEngine.UI.SpriteState state = new UnityEngine.UI.SpriteState();
                state.disabledSprite = sprites[2];

                sendBtn.spriteState = state;
                sendBtn.interactable = false;
                
            }
            else
            {
                UnityEngine.UI.SpriteState state = new UnityEngine.UI.SpriteState();
                state.disabledSprite = sprites[3];

                sendBtn.spriteState = state;
                sendBtn.interactable = false;
            }
        }


    }

    public void OnClickSendBtn()
    {
        if (friend.receiveStatus == 1)//领取
        {
            if(GameMainManager.instance.model.userData.energy>=GameMainManager.instance.model.userData.maxEnergy)
            {
                Alert.Show("您的体力太多，消耗些再来领吧！");
                return;
            }
            GameMainManager.instance.netManager.ReceiveEnergy(friendItemData.friend.uid, (ret, res) =>
            {
                if (res.isOK)
                {
                    foreach(FriendData fd in res.data.friends)
                    {
                        if(fd.uid == friend.uid)
                        {
                            //friendItemData.friend = fd;
                            friendItemData.friend.receiveStatus = fd.receiveStatus;
                            break;
                        }
                    }

                    SetData(friendItemData);
                }
                else
                {

                }
            });
        }
        else if (friend.sendStatus == 0)//赠送
        {
            GameMainManager.instance.netManager.SendEnergy(friendItemData.friend.uid, (ret, res) =>
            {
                if (res.isOK && res.data != null)
                {
                    FriendData fd = res.data[0];
                    friendItemData.friend.sendStatus = fd.sendStatus;
                    
                    SetData(friendItemData);
                }
            });
        }
        
       
    }
    //招募
    public void OnClickRecruitBtn()
    {
        GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UIFriendsWindow);
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIInviteWindow);
    }

    public void OnClickHead()
    {
        EventDispatcher.instance.DispatchEvent(new SelectedFriendEvent(friend, head.transform.position));
    }
}

public class FriendItemData
{
    public int type = 0;//0:招募 1：好友
    public FriendData friend;

    public FriendItemData(int type,FriendData friend)
    {
        this.type = type;
        this.friend = friend;
    }
}
