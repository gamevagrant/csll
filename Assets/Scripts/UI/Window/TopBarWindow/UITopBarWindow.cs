using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
            }
            return _windowData;
        }
    }

    public RectTransform panel;
    public Image[] shieldImages;
    public Text starLabel;
    public Text moneyLabel;

    private UserData user;

    protected override void StartShowWindow(object[] data)
    {
        user = GameMainManager.instance.model.userData;
        panel.anchoredPosition = new Vector2(0, 70);
        updateData();
        EventDispatcher.instance.AddEventListener(EventEnum.LOGIN_COMPLATE, OnUpdateData);
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_USERDATA, OnUpdateData);
        EventDispatcher.instance.AddEventListener(EventEnum.GET_SHIELD, OnGetShield);
    }

    protected override void StartHideWindow()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOGIN_COMPLATE, OnUpdateData);
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_USERDATA, OnUpdateData);
        EventDispatcher.instance.RemoveEventListener(EventEnum.GET_SHIELD, OnGetShield);
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

    }

    private void OnUpdateData(BaseEvent e)
    {
        updateData();
    }

    private void OnGetShield(BaseEvent e)
    {
        GetShieldEvent evt = e as GetShieldEvent;
        for(int i = 0;i<shieldImages.Length;i++)
        {
            if(!shieldImages[i].gameObject.activeSelf)
            {
                evt.emptyShieldPos(shieldImages[i].transform.position);
                break;
            }
            if(i == shieldImages.Length - 1)
            {
                evt.emptyShieldPos(shieldImages[i].transform.position);
            }
        }

       
        Invoke("updateData",evt.delay);
    }

    private void updateData()
    {
        starLabel.text = user.crowns.ToString();
        moneyLabel.text = GameUtils.GetCurrencyString(user.money);
        Debug.Log("money = " + GameUtils.GetCurrencyString(user.money).ToString());

        for(int i = 0;i< shieldImages.Length;i++)
        {
            if(i< user.shields)
            {
                shieldImages[i].gameObject.SetActive(true);
            }else
            {
                shieldImages[i].gameObject.SetActive(false);
            }
           
        }
    }
}
