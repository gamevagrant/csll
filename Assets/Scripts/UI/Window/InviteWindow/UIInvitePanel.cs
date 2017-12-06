using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QY.Open;

public class UIInvitePanel : MonoBehaviour {

    public AutoScrollView scrollView;
    public Slider slider;
    public TextMeshProUGUI sliderText;

    private List<InviteItemData> invitableList;

    public void Start()
    {
        slider.value = 0;
        sliderText.text = "";
        Refresh();
    }

    public void Refresh()
    {
        GameMainManager.instance.netManager.GetInviteProgress((ret, res) =>
        {
            if(res.isOK)
            {
                if(res.data.limit>0)
                {
                    SetProgress(res.data.times,res.data.limit);
                }
            }
            else
            {
                Alert.Show(res.errmsg);
            }
        });

        GameMainManager.instance.open.GetInvitableFriends((res) => {
            invitableList = new List<InviteItemData>();
            for (int i = 0; i < res.Length; i++)
            {
                InvitableFriendsData data = res[i];
                InviteItemData itemData = new InviteItemData();
                itemData.id = data.id;
                itemData.name = data.name;
                itemData.isSelected = true;

                invitableList.Add(itemData);
            }
            scrollView.SetData(invitableList);
            if(sliderText.text == "")
            {
                SetProgress(0, invitableList.Count);
            }
            GameMainManager.instance.netManager.SetInviteProgress(invitableList.Count, (ret, rs) =>{});
        });
    }

    private void SetProgress(int num,int maxNum)
    {
        if(maxNum>0 && num>=0)
        {
            slider.value = num / maxNum;
            sliderText.text = string.Format("{0}/{1}", num, maxNum);
        }
       
    }


    public void OnClickSelectAllBtn(bool isSelected)
    {
        foreach(InviteItemData itemData in invitableList)
        {
            itemData.isSelected = isSelected;
        }
        scrollView.SetData(invitableList);
    }

    public void OnClickInviteBtn()
    {
        List<string> list = new List<string>();
        foreach(InviteItemData itemData in invitableList)
        {
            if(itemData.isSelected)
            {
                list.Add(itemData.id);
            }
        }
        if(list.Count>0)
        {
            GameMainManager.instance.open.Invite("快来和我一起玩财神来了", 
                list.ToArray(), 
                "您正在邀请的好友",
                (response) => {
                    GameMainManager.instance.netManager.InviteFriends(response.request, response.to.Split(','), (ret, res) =>
                    {
                        Refresh();
                    });
                });
        }
        else
        {
            Alert.Show("您还没有选择要邀请的好友！");
        }
        
    }

    public void OnClickHelpBtn()
    {
        Alert.Show("1.邀请所有好友获得20能量\n2.每邀请成功一个好友可额外获得20能量和其他奖励",Alert.OK,null,"","",TextAlignmentOptions.Left);
    }
	
}
