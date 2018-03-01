﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.UI;
using TMPro;
public class UIMailListItem : BaseItemView
{
    public GameObject gift;
    public Button getBtn;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI timeText;
    MailData mailData;

    private void Awake()
    {
        getBtn.onClick.AddListener(OnClickGetRewardBtn);
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_MAIL_DATA, OnUpdateMailDataHandle);
    }

    private void OnDestroy()
    {
        getBtn.onClick.RemoveAllListeners();
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_MAIL_DATA, OnUpdateMailDataHandle);
    }


    public override void SetData(object data)
    {
        mailData  = data as MailData;
        contentText.text = mailData.desc;
        timeText.text = mailData.time;
        gift.SetActive(mailData.reward.Length > 0);
        
        if (mailData.is_get == 0)
        {
            getBtn.gameObject.SetActive(false);
        }else if(mailData.is_get == 1)
        {
            getBtn.gameObject.SetActive(true);
            getBtn.interactable = true;
        }else
        {
            getBtn.gameObject.SetActive(true);
            getBtn.interactable = false;
        }
    }

    private void OnUpdateMailDataHandle(BaseEvent evt)
    {
        GetRewardData getRewardData = evt.datas[0] as GetRewardData;
        SetData(getRewardData.user_mail[mailData.index]);
    }

    private void OnClickGetRewardBtn()
    {
        if(mailData.type == 4)
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIDungeonGetMailRewardWindow, mailData);
        }else
        {
            GameMainManager.instance.netManager.GetReward(mailData.index, (ret, res) =>
            {
                if (res.isOK)
                {
                    SetData(res.data.user_mail[mailData.index]);
                    ShowReward(res.data.user_rewards);
                }
            });
        }
        
        
    }

    private void ShowReward(RewardData[] rewards)
    {
        int index = 0;
        GetRewardWindowData data = new GetRewardWindowData();
        data.reward = rewards[index];
        data.onGetReward = () => {
            index++;
            if(index<rewards.Length)
            {
                data.reward = rewards[index];
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, data);
            }
        };
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, data);
    }

}
