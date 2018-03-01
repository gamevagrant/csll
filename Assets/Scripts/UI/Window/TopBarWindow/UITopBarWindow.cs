using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class UITopBarWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UITopBarWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
                _windowData.siblingNum = 0.2f;
            }
            return _windowData;
        }
    }

    public RectTransform panel;
    public Image[] shieldImages;
    public TextMeshProUGUI starLabel;
    public TextMeshProUGUI moneyLabel;
    public TextMeshProUGUI testTag;

    private UserData user;

    private void Awake()
    {
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_BASE_DATA, OnUpdateData);
        EventDispatcher.instance.AddEventListener(EventEnum.GET_SHIELD, OnGetShield);
        EventDispatcher.instance.AddEventListener(EventEnum.GET_STAR, OnGetStar);
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_BASE_DATA, OnUpdateData);
        EventDispatcher.instance.RemoveEventListener(EventEnum.GET_SHIELD, OnGetShield);
        EventDispatcher.instance.RemoveEventListener(EventEnum.GET_STAR, OnGetStar);
    }

    public override void Init()
    {
        panel.anchoredPosition = new Vector2(0, 70);
    }
    protected override void EndShowWindow()
    {
        panel.anchoredPosition = Vector2.zero;
    }

    protected override void StartShowWindow(object[] data)
    {
        user = GameMainManager.instance.model.userData;

        UpdateMoney(user.money, 0);
        UpdateStar(user.crowns, 0);
        UpdateShield(user.shields, 0);

        if(GameMainManager.instance.open.IsDevelopment)
        {
            testTag.text = "内网";
        }else
        {
            testTag.text = "";
        }
        
    }

    protected override void StartHideWindow()
    {

       
    }



    protected override void EnterAnimation(Action onComplete)
    {
        panel.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic).OnComplete(()=> {

            onComplete();
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        panel.DOAnchorPos(new Vector2(0, 70), 0.5f).SetEase(Ease.OutCubic).OnComplete(() => {

            onComplete();
        });
    }

    

    public void OnClickBuyBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIShopWindow,new ShowShopWindowData(ShowShopWindowData.PanelType.Gold));
    }

    private void OnUpdateData(BaseEvent e)
    {
        UpdateBaseDataEvent evt = e as UpdateBaseDataEvent;
        switch (evt.updateType)
        {
            case UpdateBaseDataEvent.UpdateType.Money:
                UpdateMoney(user.money, evt.delay);
                break;
            case UpdateBaseDataEvent.UpdateType.sheild:
                UpdateShield(user.shields, evt.delay);
                break;
            case UpdateBaseDataEvent.UpdateType.star:
                UpdateStar(user.crowns, evt.delay);
                break;
        }
    }

    private void OnGetShield(BaseEvent e)
    {
        GetShieldPosEvent evt = e as GetShieldPosEvent;
        for(int i = 0;i<shieldImages.Length;i++)
        {
            if(!shieldImages[i].gameObject.activeSelf)
            {
                evt.emptyShieldPos(shieldImages[i].transform.position);
                return;
            }
        }
        evt.emptyShieldPos(shieldImages[shieldImages.Length-1].transform.position);
    }

    private void OnGetStar(BaseEvent e)
    {
        GetStarPosEvent evt = e as GetStarPosEvent;
        evt.starPos(starLabel.transform.position);

    }

    private void UpdateStar(int value,float delay)
    {
        if(starLabel.text== value.ToString())
        {
            return;
        }
        Sequence sq = DOTween.Sequence();
        sq.AppendInterval(delay);
        sq.Append(starLabel.transform.DOScale(1.3f, 0.3f).SetEase(Ease.OutCubic));
        sq.InsertCallback(delay,()=> {

            starLabel.text = value.ToString();
        });
        sq.Append(starLabel.transform.DOScale(1, 0.3f).SetEase(Ease.InCubic));
    }

    private void UpdateMoney(long value,float delay)
    {
        if (moneyLabel.text == GameUtils.GetCurrencyString(value))
        {
            return;
        }
        Sequence sq = DOTween.Sequence();
        sq.AppendInterval(delay);
        sq.Append(moneyLabel.transform.DOScale(1.3f, 0.3f).SetEase(Ease.OutCubic));
        sq.InsertCallback(delay + 0.1f,() => {

            moneyLabel.text = GameUtils.GetCurrencyString(value);
        });
        sq.Append(moneyLabel.transform.DOScale(1, 0.3f).SetEase(Ease.InCubic));
    }

    private void UpdateShield(int value,float delay)
    {
        Sequence sq = DOTween.Sequence();
        sq.AppendInterval(delay);
        sq.AppendCallback(() =>
        {
            for (int i = 0; i < shieldImages.Length; i++)
            {
                shieldImages[i].gameObject.SetActive(i < value);
            }
        });
    }

}
