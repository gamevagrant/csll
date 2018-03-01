using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonInvitePanel : MonoBehaviour {

    public BaseScrollView scrollView;
    public QY.UI.Toggle toggle;

    private List<UIDungeonInviteItem.InviteItemData> data;

    public void SetData(FriendData[] list)
    {
        data = new List<UIDungeonInviteItem.InviteItemData>();
        data.Add(new UIDungeonInviteItem.InviteItemData());
        for (int i =0;i<list.Length;i++)
        {
            UIDungeonInviteItem.InviteItemData itemData = new UIDungeonInviteItem.InviteItemData()
            {
                friend = list[i],
                isSelected = true,
            };
            data.Add(itemData);
        }
        scrollView.SetData(data);

    }

    public void OnToggleChange(bool isSelectedAll)
    {
        foreach(UIDungeonInviteItem.InviteItemData itemData in data)
        {
            itemData.isSelected = isSelectedAll;

        }
        scrollView.SetData(data);
    }

    public void OnClickInviteBtn()
    {
        if(GameMainManager.instance.model.userData.dungeon_info!=null)
        {
            int createTime = GameMainManager.instance.model.userData.dungeon_info.create_time;
            List<string> ids = new List<string>();
            foreach(UIDungeonInviteItem.InviteItemData item in data)
            {
                if(item.isSelected)
                {
                    ids.Add(item.friend.uid.ToString());
                }
            }
            if(ids.Count>0)
            {
                GameMainManager.instance.netManager.DungeonInvite(createTime, ids.ToArray(), (ret, res) =>
                {
                    UIDungeonPopupPanels.instance.ClosePanel(transform as RectTransform);
                });
            }else
            {
                UIDungeonPopupPanels.instance.OpenAlert("请先选择需要邀请的好友",()=> {
                    UIDungeonPopupPanels.instance.ClosePanel(UIDungeonPopupPanels.instance.alertPanel.transform as RectTransform);
                });
            }
           
        }
        
    }
}
