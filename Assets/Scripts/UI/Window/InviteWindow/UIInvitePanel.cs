using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QY.Open;

public class UIInvitePanel : MonoBehaviour {

    public BaseScrollView scrollView;
    public Slider slider;
    public TextMeshProUGUI sliderText;
    public QY.UI.Toggle allSelectToggle;

    private List<InviteItemData> invitableList;
    private Dictionary<string, string> invitedFriends;//已经邀请过的好友

    public void Start()
    {
        slider.value = 0;
        sliderText.text = "";
    }

    public void Refresh()
    {
        invitedFriends = LocalDatasManager.invitedFriends;
        if(invitedFriends == null)
        {
            invitedFriends = new Dictionary<string, string>();
        }

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

           /* //假数据
            res = new InvitableFriendsData[10];
            for(int i = 0;i<res.Length;i++)
            {
                res[i] = new InvitableFriendsData();
                res[i].name = "游客7" + i.ToString();
                res[i].id = i.ToString();
            }*/
            
            for (int i = 0; i < res.Length; i++)
            {
                InvitableFriendsData data = res[i];
                if(!invitedFriends.ContainsKey(data.name))
                {

                    InviteItemData itemData = new InviteItemData();
                    itemData.id = data.id;
                    itemData.name = data.name;
                    itemData.isSelected = allSelectToggle.isOn;

                    invitableList.Add(itemData);
                }
                
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
        if(invitableList!=null)
        {
            foreach (InviteItemData itemData in invitableList)
            {
                itemData.isSelected = isSelected;
            }
            scrollView.SetData(invitableList);
        }
       
    }

    private void RemoveItems(List<string> ids)
    {
        List<InviteItemData> list = new List<InviteItemData>(invitableList);
        for (int i = 0; i < ids.Count; i++)
        {
            foreach (InviteItemData item in list)
            {
                if (item.id == ids[i])
                {
                    invitableList.Remove(item);
                }
            }
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
                invitedFriends.Add(itemData.name, itemData.id);
            }
        }
        if(list.Count>0)
        {
            LocalDatasManager.invitedFriends = invitedFriends;
           

            GameMainManager.instance.open.Invite("快来和我一起玩财神来了", 
                list.ToArray(), 
                "您正在邀请的好友",
                (response) => {
                    RemoveItems(list);
                    GameMainManager.instance.netManager.InviteFriends(response.request, response.to.Split(','), (ret, res) =>
                    {

                        
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
