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

    }
}
