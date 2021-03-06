﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.UI;
using TMPro;

public class UIFriendsWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIFriendsWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }


    public GameObject myFriendsPanel;
    public GameObject addFriendPanel;
    public GameObject applyFriendsPanel;

    public BaseScrollView myFriendsScrollView;
    public BaseScrollView notAgreeScrollView;

    public TextMeshProUGUI receiveCountText;
    public Button receiveBtn;
    public TextMeshProUGUI friendCodeText;
    public TMP_InputField inputFiled;
    public Transform menuePanel;

    private FriendData selectedFriend;

    private void Awake()
    {

        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_FRIENDS, OnUpdateFriendsHandle);
        EventDispatcher.instance.AddEventListener(EventEnum.SELECTED_FRIEND, OnSelectedFriendHandle);
    }

    private void OnDestroy()
    {

        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_FRIENDS, OnUpdateFriendsHandle);
        EventDispatcher.instance.RemoveEventListener(EventEnum.SELECTED_FRIEND, OnSelectedFriendHandle);
    }


    protected override void StartShowWindow(object[] data)
    {
        GameMainManager.instance.netManager.Friend(0, (ret, res) =>
        {
            UpdateMyFriendsData(res.data.friends);

            UpdateNotAgreeFriendsData(res.data.friendsNotAgree);
        });
       
        UpdateAddFriendData();

        //addFriendPanel.SetActive(false);
        //applyFriendsPanel.SetActive(false);
        menuePanel.parent.gameObject.SetActive(false);
    }

    private void OnUpdateFriendsHandle(BaseEvent e)
    {
        UpdateFriendsEvent evt = e as UpdateFriendsEvent;
        if(evt.updateType == UpdateFriendsEvent.UpdateType.AgreeFriend || evt.updateType == UpdateFriendsEvent.UpdateType.RemoveFriend)
        {
            UpdateMyFriendsData(GameMainManager.instance.model.userData.friendInfo);
        }
        if (evt.updateType == UpdateFriendsEvent.UpdateType.AgreeFriend || evt.updateType == UpdateFriendsEvent.UpdateType.IgnoreFriend)
        {
            UpdateNotAgreeFriendsData(GameMainManager.instance.model.userData.friendNotAgreeInfo);
        }
    }

    private void OnSelectedFriendHandle(BaseEvent e)
    {
        SelectedFriendEvent evt = e as SelectedFriendEvent;
        selectedFriend = evt.friend;
        menuePanel.position = evt.pos;
        menuePanel.parent.gameObject.SetActive(true);
    }


    private void UpdateMyFriendsData(FriendData[] friendDatas)
    {
        //receiveCountText.text = string.Format("今日领取次数：{0}/{1}",GameMainManager.instance.model.userData.rec)

        List<FriendItemData> friendItems = new List<FriendItemData>();
        friendItems.Add(new FriendItemData(0,null));

        if(friendDatas!= null)
        {
            foreach (FriendData fd in friendDatas)
            {
                friendItems.Add(new FriendItemData(1, fd));
            }

            bool tag = false;
            foreach (FriendData fd in friendDatas)
            {
                if (fd.sendStatus == 0 || fd.receiveStatus == 1)
                {
                    tag = true;
                    break;
                }
            }
            receiveBtn.interactable = tag;
        }
        myFriendsScrollView.SetData (friendItems);
    }

    private void UpdateAddFriendData()
    {
        friendCodeText.text = "我的ID：" + GameMainManager.instance.model.userData.friendshipCode;
    }

    private void UpdateNotAgreeFriendsData(FriendData[] friendDatas)
    {

        notAgreeScrollView.SetData(friendDatas);
    }

    public void OnClickAllAgreeBtn()
    {
        GameMainManager.instance.netManager.AgreeAddAllFriend((ret,res) =>
        {
            if(res.isOK)
            {
                Alert.Show("添加成功", Alert.OK, (flag) =>
                {
                    UpdateNotAgreeFriendsData(res.data.friends);
                });

            }
        });
    }

    public void OnClickAllIgnoreBtn()
    {
        GameMainManager.instance.netManager.RemoveAllFriend((ret, res) =>
        {
            if (res.isOK)
            {
                Alert.Show("忽略成功", Alert.OK, (flag) =>
                {
                    UpdateNotAgreeFriendsData(res.data.friendsNotAgree);
                });

            }
        });
    }

    public void OnClickReceiveBtn()
    {
        if(GameMainManager.instance.model.userData.energy>= GameMainManager.instance.model.userData.maxEnergy)
        {
            Alert.Show("能量已满，用掉些再来吧");
            return;
        }
        GameMainManager.instance.netManager.ReceiveEnergy(0,(ret, res) =>
        {
            if (res.isOK)
            {
                UpdateMyFriendsData(res.data.friends);

            }
        });
    }

    public void OnClickAddFriendBtn()
    {
        if(!string.IsNullOrEmpty(inputFiled.text))
        {
            GameMainManager.instance.netManager.AddFriend(inputFiled.text, (ret, res) =>
            {
                if (res.isOK)
                {
                    Alert.Show("添加好友成功");
                   // GameMainManager.instance.uiManager.OpenPopupModalBox("添加好友成功", "", null);
                }
                else
                {
                    string tips = "";
                    switch(res.errcode)
                    {
                        case 2513:
                            tips = "亲，不能添加自己为好友哒";
                            break;
                        default:
                            tips = res.errmsg;
                            break;
                    }
                    Alert.Show(tips);
                    //GameMainManager.instance.uiManager.OpenPopupModalBox(res.errmsg, "", null);
                }
            });
        }
        
    }

    public void OnClickShowBtn()
    {
        OtherData otherData = new OtherData();
        otherData.buildings = selectedFriend.buildings;
        otherData.name = selectedFriend.name;
        otherData.crowns = selectedFriend.crowns;
        otherData.headImg = selectedFriend.headImg;
        otherData.isVip = selectedFriend.isVip;
        otherData.islandId = selectedFriend.islandId;
        otherData.uid = selectedFriend.uid;
        otherData.isFriend = true;
        GameMainManager.instance.uiManager.ChangeState(new CheckPlayerState(otherData));

    }

    public void OnClickDeleteBtn()
    {
        menuePanel.parent.gameObject.SetActive(false);
        GameMainManager.instance.netManager.RemoveFriend(selectedFriend.uid, (ret,res) =>
        {
         
        });
    }

    public void OnClickCloseMenue()
    {
        menuePanel.parent.gameObject.SetActive(false);
    }
}
