using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDungeonInviteItem : BaseItemView {

    public GameObject inviteItem;
    public GameObject friendItem;

    public RawImage headImage;
    public TextMeshProUGUI nameText;
    public QY.UI.Toggle toggle;

    private InviteItemData data;

	public override void SetData(object data)
    {
        this.data = data as InviteItemData;
        inviteItem.SetActive(this.data.friend == null);
        friendItem.SetActive(this.data.friend != null);

        if (this.data.friend != null)
        {
            nameText.text = this.data.friend.name;
            toggle.isOn = this.data.isSelected;
            AssetLoadManager.Instance.LoadAsset<Texture2D>(this.data.friend.headImg, (tex) =>
            {
                headImage.texture = tex;
            });
        }
    }

    public void OnClickInviteBtn()
    {
        GameMainManager.instance.open.Invite("选择邀请的好友", "快来和我一起攻击蚌精吧",(res)=> {

            
        });
    }


    public class InviteItemData
    {
        public FriendData friend;
        public bool isSelected;
    }
}
