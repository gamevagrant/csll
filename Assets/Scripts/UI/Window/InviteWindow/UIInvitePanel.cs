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
    public GameObject helpPanel;
    public TextMeshProUGUI inviteEnergyText;
    public QY.UI.Button inviteAllBtn;

    private List<InviteItemData> invitableList;
    private Dictionary<string, string> invitedFriends;//已经邀请过的好友

    public void Start()
    {
        slider.value = 0;
        sliderText.text = "";
        helpPanel.SetActive(false);
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
                inviteEnergyText.text = res.data.is_first ? "50" : "20";

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
            GameMainManager.instance.model.userData.invitableList = new List<InvitableFriendsData>();
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
                    GameMainManager.instance.model.userData.invitableList.Add(data);
                }
                
            }

            scrollView.SetData(invitableList);
            if(sliderText.text == "")
            {
                SetProgress(0, invitableList.Count);
            }
            GameMainManager.instance.netManager.SetInviteProgress(invitableList.Count, (ret, rs) =>{});
            inviteAllBtn.interactable = invitableList.Count > 0;
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
        inviteEnergyText.text = "20";
        inviteAllBtn.interactable = invitableList.Count > 0;
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
            GameMainManager.instance.open.Invite("快来和我一起玩财神来了", 
                list.ToArray(), 
                "您正在邀请的好友",
                (response) => {
                    LocalDatasManager.invitedFriends = invitedFriends;
                    RemoveItems(list);
                    GameMainManager.instance.netManager.InviteFriends(response.request, response.to.Split(','), (ret, res) =>
                    {
                        if(ret && res.isOK)
                        {
                            if(res.data.reward_list!=null && res.data.reward_list.Length>0)
                            {
                                foreach(RewardData reward in res.data.reward_list)
                                {
                                    GetRewardWindowData getRewardData = new GetRewardWindowData();
                                    getRewardData.reward = reward;
                                    GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, getRewardData);
                                }
                                
                            }
                        }
                        
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
        helpPanel.SetActive(true);
        //Alert.Show("1.邀请所有好友获得20能量\n2.每邀请成功一个好友可额外获得20能量和其他奖励",Alert.OK,null,"","",TextAlignmentOptions.Left);
    }

    public void OnClickCloseHelpBtn()
    {
        helpPanel.SetActive(false);
    }
	
}
