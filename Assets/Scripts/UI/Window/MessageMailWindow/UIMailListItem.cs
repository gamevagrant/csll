using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    }

    private void OnDestroy()
    {
        getBtn.onClick.RemoveAllListeners();
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
            getBtn.interactable = false;
        }else
        {
            getBtn.gameObject.SetActive(true);
            getBtn.interactable = true;
        }
    }

    private void OnClickGetRewardBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIGetRewardWindow, mailData.reward);
    }
}
