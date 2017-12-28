using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIStealWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
           if(_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIStealWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public UIStealIsland[] islands;
    public RectTransform[] buttons;
    public RectTransform islandRoot;
    public RectTransform topBar;
    //public RectTransform bottomBar;
    public HeadIcon targetHead;
    //public TextMeshProUGUI bottomBarTips;
    public TextMeshProUGUI stealTips;
    public TextMeshProUGUI targetMooneyLabel;
    public RectTransform victoryTip;
    public RectTransform effect;
    public GameObject goldEffect;

    private Vector2[] goAwayPos = new Vector2[] {new Vector2(-500,0),new Vector2(500,0),new Vector2(0,-800) };
    private Vector2[] islandPos = new Vector2[3];
    private int selectedIndex;

    private void Awake()
    {
        for (int i = 0; i < islands.Length; i++)
        {
            islandPos[i] = (islands[i].transform as RectTransform).anchoredPosition;
        }
       
    }

    protected override void StartShowWindow(object[] data)
    {
        StealIslandData[] stealTargets = data[0] as StealIslandData[];

        topBar.gameObject.SetActive(false);
        topBar.anchoredPosition = new Vector2(0, 150);

        //bottomBar.gameObject.SetActive(false);
        //bottomBar.anchoredPosition = new Vector2(0, -350);

        victoryTip.gameObject.SetActive(false);
        effect.gameObject.SetActive(false);
        stealTips.gameObject.SetActive(false);
        goldEffect.SetActive(false);

        islandRoot.localScale = new Vector3(0.3f,0.3f,0.3f);
        islandRoot.gameObject.SetActive(false);
        islandRoot.anchoredPosition = new Vector2(500, 0);

        TargetData target = GameMainManager.instance.model.userData.stealTarget;
        targetHead.setData(target.name,target.headImg,target.crowns,target.isVip);
        targetMooneyLabel.text = GameUtils.GetCurrencyString(target.money);

        for (int i =0;i<stealTargets.Length;i++)
        {
            StealIslandData stealData = stealTargets[i];
            if(i<islands.Length)
            {
                islands[i].setData(stealData);
                islands[i].island.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                (islands[i].transform as RectTransform).anchoredPosition = islandPos[i];
            }
                
            if(i<buttons.Length)
                buttons[i].gameObject.SetActive(false);
        }

        QY.Guide.GuideManager.instance.state = "steal";
    }

    protected override void EnterAnimation(Action onComplete)
    {
        islandRoot.gameObject.SetActive(true);
        topBar.gameObject.SetActive(true);

        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_island_come);
        Sequence sq = DOTween.Sequence();
        sq.Append(islandRoot.DOAnchorPos(new Vector2(0, 0), 1));
        sq.Insert(0,topBar.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic));
        sq.AppendCallback(() =>
        {
            effect.gameObject.SetActive(true);
        });
        sq.Append(islandRoot.DOScale(Vector3.one, 1).SetEase(Ease.OutBack));
        sq.AppendCallback(() =>
        {
            stealTips.gameObject.SetActive(true);
            effect.gameObject.SetActive(false);
            for (int i = 0; i< buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
                
            }
        });
        
        sq.onComplete += () => {
            onComplete();
        };
    }

    protected override void ExitAnimation(Action onComplete)
    {
        stealTips.gameObject.SetActive(false);
        victoryTip.gameObject.SetActive(false);
        
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        topBar.DOAnchorPos(new Vector2(0, 150), 0.5f).SetEase(Ease.OutCubic);
        //bottomBar.DOAnchorPos(new Vector2(0, -350), 0.5f).SetEase(Ease.OutCubic);
        Sequence sq = DOTween.Sequence();
        for (int i = 0; i < islands.Length; i++)
        {
            sq.Insert(0, (islands[i].transform as RectTransform).DOAnchorPos(islandPos[i], 0.5f));
            sq.Insert(0, (islands[i].transform as RectTransform).DOScale(Vector3.one, 0.5f));
        }
        
        sq.AppendCallback(() =>
        {
            effect.gameObject.SetActive(true);
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.shoot_island_come);
            
        });
        sq.Append(islandRoot.DOScale(new Vector3(0.3f,0.3f,0.3f), 1).SetEase(Ease.OutCubic));
        sq.AppendCallback(() =>
        {
            effect.gameObject.SetActive(false);
        });
        sq.Append(islandRoot.DOAnchorPos(new Vector2(500, 0), 1));
        sq.onComplete += () => {
            onComplete();
        };
        
    }

    public void OnClickSelectBtn(int index)
    {
        selectedIndex = index;
        stealTips.gameObject.SetActive(false);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        GameMainManager.instance.netManager.Steal(selectedIndex, (ret, res) =>
        {
            if (res.isOK)
            {
                StealData stealData = res.data;
                TargetData selectedTarget = stealData.targets[selectedIndex];

                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.steal_result);
                Sequence sq = DOTween.Sequence();
               
                for (int i = 0; i < islands.Length; i++)
                {
                    if (i != selectedIndex)
                    {
                        islands[i].setData(stealData.targets[i]);
                        sq.Insert(3, (islands[i].transform as RectTransform).DOAnchorPos(goAwayPos[i], 1));
                    }
                    else
                    {
                        sq.Insert(3, (islands[i].transform as RectTransform).DOAnchorPos(new Vector2(0, -100), 1));
                        sq.Insert(3, (islands[i].transform as RectTransform).DOScale(new Vector3(2,2,2), 1));
                    }
                }
                sq.Append(topBar.DOAnchorPos(new Vector2(0, 150), 0.5f).SetEase(Ease.OutCubic));
                sq.InsertCallback(3f, () =>
                {
                    effect.gameObject.SetActive(true);

                });
                sq.InsertCallback(5, () =>
                {

                    effect.gameObject.SetActive(false);
                    islands[selectedIndex].setData(selectedTarget);
                    goldEffect.SetActive(true);
                    EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money, 0));
                    if (stealData.targets[selectedIndex].isRichMan)
                    {
                        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.steal_got_king);
                        victoryTip.gameObject.SetActive(true);

                        Alert.ShowPopupBox(string.Format("恭喜你猜到富豪！\n获得<#D34727FF>{0}</color>金币", GameUtils.GetCurrencyString(selectedTarget.money)), OnClickOkBtn);
                       
                    }else
                    {
                        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.steal_miss_king);
                        
                        Alert.ShowPopupBox(string.Format("很遗憾没有猜到富豪！\n只得到<#D34727FF>{0}</color>金币", GameUtils.GetCurrencyString(selectedTarget.money)), OnClickOkBtn);
                       
                    }
                });


            }
        });
    }

    public void OnClickOkBtn()
    {
        Dictionary<UISettings.UIWindowID, object> data = new Dictionary<UISettings.UIWindowID, object>();
        data.Add(UISettings.UIWindowID.UITopBarWindow, null);
        data.Add(UISettings.UIWindowID.UIWheelWindow, null);
        data.Add(UISettings.UIWindowID.UISideBarWindow, null);
        GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(data,null,3));
    }

}
