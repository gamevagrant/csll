using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using QY.UI;
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

    private void Start()
    {

        TopBar.anchoredPosition = new Vector2(0, 160);
        allPanel.anchoredPosition = new Vector2(0, 950);
        friendPanel.anchoredPosition = new Vector2(0, 950);
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

        friendToggle.isOn = true;

        TopBar.gameObject.SetActive(true);
        TopBar.anchoredPosition = new Vector2(0, 160);

        allPanel.gameObject.SetActive(false);
        allPanel.anchoredPosition = new Vector2(0, 950);

        friendPanel.gameObject.SetActive(true);
        friendPanel.anchoredPosition = new Vector2(0, 950);

        currPanel = friendPanel;
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
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_in);
        currPanel = panel;
        panel.gameObject.SetActive(true);
        panel.DOAnchorPos(new Vector2(0, 0), 1f).SetEase(Ease.OutBack);
    }

    private void HidePanel(RectTransform panel)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.panel_out);
        panel.DOAnchorPos(new Vector2(0, 950), 1f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            //panel.gameObject.SetActive(false);
        });
    }

    private void OnAllValueChange(bool value)
    {
        if (value)
        {
            ShowPanel(allPanel);
        }
        else
        {
            HidePanel(allPanel);
        }
    }
    private void OnMyValueChange(bool value)
    {
        if (value)
        {
            ShowPanel(friendPanel);
        }
        else
        {
            HidePanel(friendPanel);
        }
    }
}
