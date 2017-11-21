using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIRankWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIRankWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.NormalNavigation;
            }

            return _windowData;
        }
    }

    public Toggle allToggle;
    public Toggle friendToggle;

    public RectTransform TopBar;

    public RectTransform allPanel;
    public RectTransform friendPanel;

    public VerticalScrollView allScrollView;
    public VerticalScrollView friendScrollView;


    private RectTransform currPanel;

    private void Awake()
    {
        allToggle.onValueChanged.AddListener(OnAllValueChange);
        friendToggle.onValueChanged.AddListener(OnMyValueChange);
    }

    private void OnDestroy()
    {
        allToggle.onValueChanged.RemoveAllListeners();
        friendToggle.onValueChanged.RemoveAllListeners();
    }


    protected override void StartShowWindow(object[] data)
    {
        GameMainManager.instance.netManager.AllRank((ret, res) =>
        {
            if(res.isOK)
            {
                AllRankData allRankData = res.data;
                List<SubListData> list = new List<SubListData>();
                list.Add(new SubListData("", new List<object>(allRankData.top)));
                list.Add(new SubListData("我的排名", new List<object>(allRankData.my)));
                allScrollView.SetData(list);
            }
        });

        GameMainManager.instance.netManager.FriendRank((ret, res) =>
        {
            if (res.isOK)
            {
                List<SubListData> list = new List<SubListData>();
                List<object> friends = new List<object>(res.data);
                for(int i = 0;i< friends.Count;i++)
                {
                    FriendData friend = friends[i] as FriendData;
                    friend.rank = i + 1;
                }
                friends.Add(null);
                list.Add(new SubListData("", friends));
                friendScrollView.SetData(list);
            }
        });

        allToggle.isOn = true;
        allToggle.enabled = false;
        allToggle.enabled = true;

        TopBar.anchoredPosition = new Vector2(0, 160);
        //toggleGroup.NotifyToggleOn(energyToggle);
        allPanel.gameObject.SetActive(true);
        allPanel.anchoredPosition = new Vector2(0, 950);

        friendPanel.gameObject.SetActive(false);
        friendPanel.anchoredPosition = new Vector2(0, 950);

        currPanel = allPanel;
    }

    protected override void EnterAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        ShowPanel(currPanel);
        sq.Insert(0.2f, TopBar.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic));
        sq.InsertCallback(1, () =>
        {
            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();
        HidePanel(currPanel);
        sq.Insert(0.1f, TopBar.DOAnchorPos(new Vector2(0, 160), 0.5f).SetEase(Ease.InCubic));
        sq.InsertCallback(1, () =>
        {
            onComplete();
        });
    }

    private void ShowPanel(RectTransform panel)
    {
        //Sequence sq = DOTween.Sequence();
        panel.gameObject.SetActive(true);
        panel.DOAnchorPos(new Vector2(0, 0), 1f).SetEase(Ease.OutBack);
    }

    private void HidePanel(RectTransform panel)
    {
        panel.DOAnchorPos(new Vector2(0, 950), 1f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }

    private void OnAllValueChange(bool value)
    {
        currPanel = allPanel;
        if (value)
        {
            ShowPanel(currPanel);
        }
        else
        {
            HidePanel(currPanel);
        }
    }
    private void OnMyValueChange(bool value)
    {
        currPanel = friendPanel;
        if (value)
        {
            ShowPanel(currPanel);
        }
        else
        {
            HidePanel(currPanel);
        }
    }
}
