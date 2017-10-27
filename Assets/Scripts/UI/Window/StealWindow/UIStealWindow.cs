﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public RectTransform bottomBar;
    public HeadIcon targetHead;
    public Text bottomBarTips;
    public Text stealTips;
    public Text targetMooneyLabel;
    public RectTransform victoryTip;
    public RectTransform effect;

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

        bottomBar.gameObject.SetActive(false);
        bottomBar.anchoredPosition = new Vector2(0, -350);

        victoryTip.gameObject.SetActive(false);
        effect.gameObject.SetActive(false);
        stealTips.gameObject.SetActive(false);

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
        bottomBar.DOAnchorPos(new Vector2(0, -350), 0.5f).SetEase(Ease.OutCubic);
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
        bottomBar.gameObject.SetActive(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        GameMainManager.instance.netManager.Steal(index, (ret, res) =>
        {
            if (res.isOK)
            {
                StealData stealData = res.data;
                TargetData selectedTarget = stealData.targets[index - 1];

                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.steal_result);
                Sequence sq = DOTween.Sequence();
                sq.Append(topBar.DOAnchorPos(new Vector2(0, 150), 0.5f).SetEase(Ease.OutCubic));
                for (int i = 0; i < islands.Length; i++)
                {
                    if (i != index - 1)
                    {
                        islands[i].setData(stealData.targets[i]);
                        sq.Insert(2, (islands[i].transform as RectTransform).DOAnchorPos(goAwayPos[i], 1));
                    }
                    else
                    {
                        sq.Insert(2, (islands[i].transform as RectTransform).DOAnchorPos(new Vector2(0, -100), 1));
                        sq.Insert(2, (islands[i].transform as RectTransform).DOScale(new Vector3(2,2,2), 1));
                    }
                }
                
                sq.InsertCallback(2f, () =>
                {
                    effect.gameObject.SetActive(true);

                });
                sq.InsertCallback(4, () =>
                {

                    effect.gameObject.SetActive(false);
                    islands[index-1].setData(selectedTarget);
                    if(stealData.targets[index - 1].isRichMan)
                    {
                        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.steal_got_king);
                        victoryTip.gameObject.SetActive(true);
                        bottomBarTips.text = string.Format("恭喜你猜到富豪！\n获得{0}金币",GameUtils.GetCurrencyString(selectedTarget.money));
                    }else
                    {
                        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.steal_miss_king);
                        bottomBarTips.text = string.Format("很遗憾没有猜到富豪！\n获得{0}金币", GameUtils.GetCurrencyString(selectedTarget.money));
                    }
                });
                sq.Insert(6, bottomBar.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic));

            }
        });
    }

    public void OnClickOkBtn()
    {
        Dictionary<UISettings.UIWindowID, object> data = new Dictionary<UISettings.UIWindowID, object>();
        data.Add(UISettings.UIWindowID.UIWheelWindow, null);
        GameMainManager.instance.uiManager.ChangeState(new UIStateChangeBase(data,null,3));
    }

}