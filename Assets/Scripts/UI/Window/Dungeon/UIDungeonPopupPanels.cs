using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonPopupPanels : MonoBehaviour {

    private static UIDungeonPopupPanels _instance;
    public static UIDungeonPopupPanels instance
    {
        get
        {
            return _instance;
        }
    }
    

    public RectTransform background;
    public UIDungeonCardsPoolPanel cardsPoolPanel;
    public UIDungeonAlertPanel alertPanel;
    public UIDungeonInvitePanel invitePanel;
    public UIDungeonBuyMasterCardPanel buyMasterCardPanel;
    public RectTransform helpPanel;
    public UIDungeonContent getRewardPanel;

    private DungeonInfoData data;
	// Use this for initialization
	void Start ()
    {
        _instance = this;
        data = GameMainManager.instance.model.userData.dungeon_info;

    }

    public void OpenCardsPoolPanel()
    {
        data = GameMainManager.instance.model.userData.dungeon_info;
        OpenPanel(cardsPoolPanel.transform as RectTransform);
        cardsPoolPanel.SetData(data.card_big,data.card_small);
    }

    public void OpenInvitePanel()
    {
        data = GameMainManager.instance.model.userData.dungeon_info;
        GameMainManager.instance.netManager.DungeonGetInviteList(data.create_time, (ret, res) =>
        {
            if(res.isOK)
            {
                OpenPanel(invitePanel.transform as RectTransform);
                invitePanel.SetData(res.data.friend_list);

            }
        });
    }

    public void OpenAlert(string title,System.Action onClickOk)
    {
        OpenPanel(alertPanel.transform as RectTransform);
        alertPanel.Alert(title, onClickOk);
    }

    public void OpenAlertCardFishNotEnough(string title, string des)
    {
        OpenPanel(alertPanel.transform as RectTransform);
        alertPanel.CardFishNotEnough(title, des);
    }

    public void OpenAlertUseCardFish(string title, int maxCount, System.Action<int> onClickOk)
    {
        OpenPanel(alertPanel.transform as RectTransform);
        alertPanel.UseCardFish(title, maxCount, onClickOk);
    }

    public void OpenAlertUseMasterCard(string title, int maxCount, System.Action onClickOk)
    {
        OpenPanel(alertPanel.transform as RectTransform);
        alertPanel.UseMasterCard(title, maxCount, onClickOk);
    }

    public void OpenBuyMasterCardPanel()
    {
        data = GameMainManager.instance.model.userData.dungeon_info;
        OpenPanel(buyMasterCardPanel.transform as RectTransform);
        buyMasterCardPanel.SetData(data.buy_master_card);
    }

    public void OpenHelpPanel()
    {
        OpenPanel(helpPanel);
    }

    public void OpenGetRewardPanel()
    {
        data = GameMainManager.instance.model.userData.dungeon_info;
        if(data.self_rewards!=null && data.self_rewards.Length>0)
        {
            OpenPanel(getRewardPanel.transform as RectTransform);
            getRewardPanel.SetData(data.self_rewards);
        }
        
    }

    private void OpenPanel(RectTransform panel)
    {
        background.SetAsLastSibling();
        panel.SetAsLastSibling();

        background.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);
       
    }

    public void ClosePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        for(int i = transform.childCount-1; i>=0;i--)
        {
            Transform tf = transform.GetChild(i);
            if(tf.gameObject.activeSelf && tf!=background)
            {
                background.gameObject.SetActive(true);
                background.SetSiblingIndex(i);
                
            }
        }
    }


}
