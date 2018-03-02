using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class UIGetRewardWindow : UIWindowBase {

    public TextMeshProUGUI text;
    public Image image;
    public Sprite[] sprites;//0:钱 1：能量 2:vip 3:通缉令

    private Queue<GetRewardWindowData> queue;
    GetRewardWindowData data;

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIGetRewardWindow;
                _windowData.type = UISettings.UIWindowType.Cover;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.Transparent;
            }
            return _windowData;
        }
    }

    private void Awake()
    {
        queue = new Queue<GetRewardWindowData>();
    }

    protected override void StartShowWindow(object[] data)
    {
       
        GetRewardWindowData getRewardWindowData = data[0] as GetRewardWindowData;
        queue.Enqueue(getRewardWindowData);
        if(queue.Count==1)
        {
            ShowNext();
        }
    }

    protected override void EnterAnimation(Action onComplete)
    {
        onComplete();
    }

    protected override void ExitAnimation(Action onComplete)
    {
        onComplete();
    }


    private void ShowNext()
    {
        if(queue.Count>0)
        {
            data = queue.Peek();
            RewardData reward = data.reward;

            switch (reward.type)
            {
                case "money":
                case "gold":
                    image.sprite = sprites[0];
                    text.text = "金币 x" + reward.num.ToString();
                    break;
                case "energy":
                    image.sprite = sprites[1];
                    text.text = "能量 x" + reward.num.ToString();
                    break;
                case "vip":
                    image.sprite = sprites[2];
                    text.text = "vip" + reward.num.ToString();
                    break;
                case "wanted":
                    image.sprite = sprites[3];
                    text.text = "通缉令 x" + reward.num.ToString();
                    break;
                case "piece":
                    image.sprite = sprites[4];
                    text.text = "万能碎片 x" + reward.num.ToString();
                    break;
                case "card_fish":
                    image.sprite = sprites[5];
                    text.text = "食卡鱼 x" + reward.num.ToString();
                    break;
            }

            image.SetNativeSize();
            Show();
        }
       
    }

    private void Confirm()
    {
       
        Hide(() =>
        {
            queue.Dequeue();
            if (queue.Count > 0)
            {
                ShowNext();
            }
            else
            {
                OnClickClose();
            }
        });

        switch(data.reward.type)
        {
            case "money":
            case "gold":
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money, 0));
                break;
            case "energy":
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Energy, 0));
                break;
            case "vip":
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.vip, 0));
                break;
            case "wanted":
                EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.wanted, 0));
                break;
           
        }

    }
    private void Show()
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_box_down);
        image.transform.localScale = new Vector3(0,0,1);
        image.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic);
    }

    private void Hide(Action onComplate)
    {
        onComplate();
        

    }
    public void OnClickGetRewardBtn()
    {
        Confirm();

       
    }
}

public class GetRewardWindowData
{
    public RewardData reward;
    public System.Action onGetReward;
}
