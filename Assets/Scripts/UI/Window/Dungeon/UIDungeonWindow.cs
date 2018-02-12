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
            if (GameMainManager.instance.model.userData.dungeonState == 3)
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
    public UIDungeonMasterCardMovieClip useMasterCardMovieClip;


    private DungeonInfoData dungeonInfo;
    private UserData userData;

    protected override void StartShowWindow(object[] data)
    {
        userData = GameMainManager.instance.model.userData;
        GameMainManager.instance.netManager.DungeonGetInfo((ret,res)=> {

            if (res.isOK)
            {
                dungeonInfo = res.data.dungeon_info;
            }else
            {
                dungeonInfo = userData.dungeon_info;
            }

            Refresh();
        });
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
        UIDungeonPopupPanels.instance.OpenCardsPoolPanel();
    }

    public void OnClickInviteBtn()
    {
        UIDungeonPopupPanels.instance.OpenInvitePanel();
    }

    public void OnClickMasterCardBtn()
    {
        if(userData.master_card>0)
        {
            UIDungeonPopupPanels.instance.OpenAlertUseMasterCard("确定现在使用万能牌吗？", userData.master_card, () =>
            {
                GameMainManager.instance.netManager.DungeonUseMasterCard(dungeonInfo.create_time, 1, (ret, res) =>
                {
                    if(res.isOK)
                    {
                        dungeonInfo = res.data.dungeon_info;
                        StartCoroutine(PlayMasterCardMovieClip());
                    }
                    
                });

                popupPanels.ClosePanel(popupPanels.alertPanel.transform as RectTransform);
            });
        }else
        {
            
        }
    }

    public void OnClickCardFishBtn()
    {
        if(userData.card_fish>0)
        {
            UIDungeonPopupPanels.instance.OpenAlertUseCardFish("每使用一个食卡鱼，卡面值-1，确定使用？", userData.card_fish,(count)=> {

                GameMainManager.instance.netManager.DungeonUseCardFish(dungeonInfo.create_time, count, (ret, res) =>
                {

                });

            });
        }
        else
        {
            UIDungeonPopupPanels.instance.OpenAlertCardFishNotEnough("您还没有食卡鱼哦！", "您可以通过拼图奖励、成就奖励获得食卡鱼。");
        }
    }

    private void Refresh()
    {
        progress.SetData((float)dungeonInfo.cards / dungeonInfo.boss_hp, dungeonInfo.rewards, dungeonInfo.rewardIndex);
        boss.SetData(dungeonInfo);
        cardBoard.SetData(dungeonInfo.selected_cards,dungeonInfo.selected_cards.Length==5 || dungeonInfo.is_used_master_card_by_buy==1);

        hurtText.text = dungeonInfo.cards.ToString();
        hurtRateText.text = ((float)dungeonInfo.cards / dungeonInfo.boss_hp).ToString("p");
    }

    private IEnumerator PlayMasterCardMovieClip()
    {
        useMasterCardMovieClip.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Refresh();
    }

}
