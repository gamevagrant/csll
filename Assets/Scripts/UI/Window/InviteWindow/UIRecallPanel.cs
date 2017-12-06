using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRecallPanel : MonoBehaviour {

    public AutoScrollView scrollView;
    private List<RecallFriendData> recallableList;

    public void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        GameMainManager.instance.netManager.GetRecallableFriends((ret, res) =>
        {
            if(res.isOK)
            {
                if(res.data.recall_friend_rewards!=null)
                {
                    recallableList = new List<RecallFriendData>();
                    foreach (ShareData.RecallableFriendData friend in res.data.recall_friend_rewards)
                    {
                        RecallFriendData data = new RecallFriendData();
                        data.data = friend;
                        data.isSelected = true;
                        recallableList.Add(data);
                    }
                    scrollView.SetData(recallableList);
                }
                
            }else
            {
                Alert.Show(res.errmsg);
            }
        });
    }

    public void OnClickSelectAllBtn(bool isSelected)
    {
        foreach (RecallFriendData itemData in recallableList)
        {
            itemData.isSelected = isSelected;
        }
        scrollView.SetData(recallableList);
    }

    public void OnClickRecallBtn()
    {
        List<string> list = new List<string>();
        foreach (RecallFriendData itemData in recallableList)
        {
            if (itemData.isSelected)
            {
                list.Add(itemData.data.uid.ToString());
            }
        }
        if (list.Count > 0)
        {
            GameMainManager.instance.netManager.RecallFriends(recallableList.Count, list.ToArray() , (ret, res) =>
            {
                if(res.isOK)
                {

                }
                else
                {
                    Alert.Show(res.errmsg);
                }
            });
        }
        else
        {
            Alert.Show("您还没有选择要邀请的好友！");
        }

    }

    public void OnClickHelpBtn()
    {
        Alert.Show("1.成功召回一个好友可获得15点能量，成功召回好友才可领取奖励\n" +
            "2.召回邀请7日内有效，好友通过您的邀请链接7日内回到游戏，则算为成功召回\n" +
            "3.当多名玩家同时向一人发送召回邀请时，只有前30发送召回邀请的人才能获得召回奖励\n" +
            "4.你可以召回任意好友，但每个好友最多只能召回1次", Alert.OK, null, "", "", TextAlignmentOptions.Left);
    }
}
