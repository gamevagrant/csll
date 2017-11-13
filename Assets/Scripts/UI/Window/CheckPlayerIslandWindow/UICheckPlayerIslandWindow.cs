using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UICheckPlayerIslandWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UICheckPlayerIslandWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public HeadIcon head;
    public GameObject backBtn;
    public IslandFactory island;

    private OtherData otherData;

    private void Awake()
    {
        head.gameObject.SetActive(false);
        (backBtn.transform as RectTransform).anchoredPosition = new Vector2(-150, 200);
        (island.transform as RectTransform).anchoredPosition = new Vector2(600, 0);
    }

    protected override void StartShowWindow(object[] data)
    {
        otherData = data[0] as OtherData;
        island.UpdateCityData(otherData.islandId, otherData.buildings);
        head.setData(otherData.name, otherData.headImg, otherData.crowns, otherData.isVip);

    }

    protected override void EnterAnimation(Action onComplete)
    {
        (backBtn.transform as RectTransform).DOAnchorPos(new Vector2(25, 200),0.5f);
        (island.transform as RectTransform).DOAnchorPos(Vector2.zero, 0.5f).OnComplete(()=> {
            head.gameObject.SetActive(true);
            onComplete();
        });

    }

    protected override void ExitAnimation(Action onComplete)
    {
        head.gameObject.SetActive(false);
        (backBtn.transform as RectTransform).DOAnchorPos(new Vector2(-150, 200), 0.5f);
        (island.transform as RectTransform).DOAnchorPos(new Vector2(600, 0), 0.5f).OnComplete(() => {
            onComplete();
        });
    }

    public void OnClickReturnBtn()
    {
        GameMainManager.instance.uiManager.ChangeState(new MainState());
    }
}
