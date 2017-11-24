using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAgreeFriendItem : BaseItemView {

    public TextMeshProUGUI timeText;
    public HeadIcon head;
    FriendData friend;

    public override void SetData(object data)
    {
        friend = data as FriendData;
        head.setData(friend.name, friend.headImg, 0, friend.isVip);
        timeText.text = "上次活跃："+ friend.updateTime;
    }

    public void OnClickIgnoreBtn()
    {
        GameMainManager.instance.netManager.RemoveFriend(friend.uid, (ret, res) =>
        {
            if (res.isOK)
            {
                Alert.Show("忽略成功");
                /*
                GameMainManager.instance.uiManager.OpenPopupModalBox("忽略成功", "", () =>
                {
                   
                });*/
            }else
            {
                Alert.Show(res.errmsg);
                // GameMainManager.instance.uiManager.OpenPopupModalBox(res.errmsg, "", null);
            }
        });
    }

    public void OnClickAgreeBtn()
    {
        GameMainManager.instance.netManager.AgreeAddFriend(friend.uid, (ret, res) =>
        {
            if (res.isOK)
            {
                Alert.Show("添加成功");
                /*
                GameMainManager.instance.uiManager.OpenPopupModalBox("添加成功", "", () =>
                {

                });*/
            }
            else
            {
                Alert.Show(res.errmsg);
                //GameMainManager.instance.uiManager.OpenPopupModalBox(res.errmsg, "", null);
            }
        });
    }
}
