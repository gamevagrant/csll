using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public Toggle myFriendsToggle;
    public Toggle addFriendToggle;
    public Toggle applyFriendsToggle;

    public GameObject myFriendsPanel;
    public GameObject addFriendPanel;
    public GameObject applyFriendsPanel;

    public DynamicScrollView myFriendsScrollView;
    public DynamicScrollView notAgreeScrollView;

    public TextMeshProUGUI receiveCountText;
    public Button receiveBtn;
    public TextMeshProUGUI friendCodeText;
    public TMP_InputField inputFiled;
    public Transform menuePanel;

    private FriendData selectedFriend;

    private void Awake()
    {
        myFriendsToggle.onValueChanged.AddListener(OnChangeMyFriendsToggle);
        addFriendToggle.onValueChanged.AddListener(OnChangeAddFriendsToggle);
        applyFriendsToggle.onValueChanged.AddListener(OnChangeApplyFriendsToggle);

        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_FRIENDS, OnUpdateFriendsHandle);
        EventDispatcher.instance.AddEventListener(EventEnum.SELECTED_FRIEND, OnSelectedFriendHandle);
    }

    private void OnDestroy()
    {
        myFriendsToggle.onValueChanged.RemoveAllListeners();
        addFriendToggle.onValueChanged.RemoveAllListeners();
        applyFriendsToggle.onValueChanged.RemoveAllListeners();

        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_FRIENDS, OnUpdateFriendsHandle);
        EventDispatcher.instance.RemoveEventListener(EventEnum.SELECTED_FRIEND, OnSelectedFriendHandle);
    }


    protected override void StartShowWindow(object[] data)
    {
        UpdateMyFriendsData();
        UpdateAddFriendData();
        UpdateNotAgreeFriendsData();
        myFriendsToggle.isOn = true;
        myFriendsToggle.enabled = false;
        myFriendsToggle.enabled = true;
        addFriendPanel.SetActive(false);
        applyFriendsPanel.SetActive(false);
        menuePanel.parent.gameObject.SetActive(false);
    }

    private void OnChangeMyFriendsToggle(bool value)
    {
        myFriendsPanel.SetActive(value);

    }

    private void OnChangeAddFriendsToggle(bool value)
    {
        addFriendPanel.SetActive(value);
    }

    private void OnChangeApplyFriendsToggle(bool value)
    {
        applyFriendsPanel.SetActive(value);
    }

    private void OnUpdateFriendsHandle(BaseEvent e)
    {
        UpdateFriendsEvent evt = e as UpdateFriendsEvent;
        if(evt.type == UpdateFriendsEvent.UpdateType.AgreeFriend || evt.type == UpdateFriendsEvent.UpdateType.RemoveFriend)
        {
            UpdateMyFriendsData();
        }
        if (evt.type == UpdateFriendsEvent.UpdateType.AgreeFriend || evt.type == UpdateFriendsEvent.UpdateType.IgnoreFriend)
        {
            UpdateNotAgreeFriendsData();
        }
    }

    private void OnSelectedFriendHandle(BaseEvent e)
    {
        SelectedFriendEvent evt = e as SelectedFriendEvent;
        selectedFriend = evt.friend;
        menuePanel.position = evt.pos;
        menuePanel.parent.gameObject.SetActive(true);
    }


    private void UpdateMyFriendsData()
    {
        //receiveCountText.text = string.Format("今日领取次数：{0}/{1}",GameMainManager.instance.model.userData.rec)

        List<FriendItemData> friendItems = new List<FriendItemData>();
        friendItems.Add(new FriendItemData(0,null));
        FriendData[] friendDatas = GameMainManager.instance.model.userData.friendInfo;
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
        myFriendsScrollView.setDatas(friendItems);
    }

    private void UpdateAddFriendData()
    {
        friendCodeText.text = "我的邀请码：" + GameMainManager.instance.model.userData.friendshipCode;
    }

    private void UpdateNotAgreeFriendsData()
    {
        FriendData[] friendDatas = GameMainManager.instance.model.userData.friendNotAgreeInfo;
        notAgreeScrollView.setDatas(friendDatas);
    }

    public void OnClickAllAgreeBtn()
    {
        GameMainManager.instance.netManager.AgreeAddAllFriend((ret,res) =>
        {
            if(res.isOK)
            {
                GameMainManager.instance.uiManager.OpenModalBoxWindow("添加成功", "", ()=> {

                    UpdateNotAgreeFriendsData();
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
                GameMainManager.instance.uiManager.OpenModalBoxWindow("忽略成功", "", ()=>
                {
                    UpdateNotAgreeFriendsData();
                });
            }
        });
    }

    public void OnClickReceiveBtn()
    {
        GameMainManager.instance.netManager.ReceiveEnergy(0,(ret, res) =>
        {
            if (res.isOK)
            {
                GameMainManager.instance.uiManager.OpenModalBoxWindow("一键领取成功", "", ()=> {
                    UpdateMyFriendsData();

                });
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
                    GameMainManager.instance.uiManager.OpenModalBoxWindow("添加好友成功", "", null);
                }
                else
                {
                    GameMainManager.instance.uiManager.OpenModalBoxWindow(res.errmsg, "", null);
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
