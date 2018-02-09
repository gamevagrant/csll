using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDungeonWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIDungeonWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public override bool canOpen
    {
        get
        {
            if (GameMainManager.instance.model.userData.dungeon_keys<=0)
            {
                Alert.Show("在转盘中获得副本钥匙后开启！");
                return false;
            } 
            return true;
        }
    }
    
    
    public UIDungeonProgress progress;
    public UIDungeonBoss boss;
    public UIDungeonCardBoard cardBoard;
    public TextMeshProUGUI hurtText;
    public TextMeshProUGUI hurtRateText;
    public UIDungeonPopupPanels popupPanels;


    private DungeonInfoData dungeonInfo;

    protected override void StartShowWindow(object[] data)
    {
        GameMainManager.instance.netManager.DungeonGetInfo((ret,res)=> {

            if (res.isOK)
            {
                dungeonInfo = res.data.dungeon_info;
            }else
            {
                dungeonInfo = GameMainManager.instance.model.userData.dungeon_info;
            }

            Refresh();
        });
    }

    private void Refresh()
    {

        progress.SetData((float)dungeonInfo.cards / dungeonInfo.boss_hp, dungeonInfo.rewards, dungeonInfo.rewardIndex);
        boss.SetData(dungeonInfo);
        cardBoard.SetData(dungeonInfo.selected_cards);

        hurtText.text = dungeonInfo.cards.ToString();
        hurtRateText.text = ((float)dungeonInfo.cards / dungeonInfo.boss_hp).ToString("p");
    }

    public void OnSelectedCard(UIDungeonCard card)
    {
        if(card!=null && card.data!=null && card.data.num>0)
        {
            UIDungeonPopupPanels.instance.OpenAlert("要将这张卡牌放入卡槽吗？", () =>
            {
                GameMainManager.instance.netManager.DungeonSelectCard(dungeonInfo.create_time, card.data.uid, (ret, res) =>
                {
                    if(res.isOK)
                    {
                        dungeonInfo = res.data.dungeon_info;
                        Refresh();
                    }
                   
                });

                popupPanels.ClosePanel(popupPanels.alertPanel.transform as RectTransform);
                popupPanels.ClosePanel(popupPanels.cardsPoolPanel.transform as RectTransform);

            });
        }
        
    }

    public void OnClickCardsPoolBtn()
    {

    }

    public void OnClickInviteBtn()
    {

    }

    public void OnClickMasterCardBtn()
    {

    }

    public void OnClickCardFishBtn()
    {

    }
}
