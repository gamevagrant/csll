using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class UIDungeonIcon : MonoBehaviour {

    public TextMeshProUGUI countDownText;
    public Image image;
    public Sprite[] sprites; 

    private DungeonInfoData data;
    private int lastTime = -1;
    private int countDownTime
    {
        get
        {
            if(data!=null)
            {
                return data.countDown;
            }
            return 0;
        }
    }

    // Use this for initialization
    void Start () {
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_DUNGEON, OnUpdateDungeonDataHandle);
        EventDispatcher.instance.AddEventListener(EventEnum.GET_DUNGEON_KEY, OnGetDungeonKeyHandle);
	}

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_DUNGEON, OnUpdateDungeonDataHandle);
        EventDispatcher.instance.RemoveEventListener(EventEnum.GET_DUNGEON_KEY, OnGetDungeonKeyHandle);
    }
    // Update is called once per frame
    void Update ()
    {
        UpdateCountDown();
    }

    private void OnEnable()
    {
        if (Application.isPlaying)
        {

            Refresh();
        }
    }

    private void OnUpdateDungeonDataHandle(BaseEvent evt)
    {
        Refresh();
    }

    private void OnGetDungeonKeyHandle(BaseEvent evt)
    {
        GetDungeonKeyEvent e = evt as GetDungeonKeyEvent;
        e.iconPos(transform.position);
    }
    private void Refresh()
    {
        data = GameMainManager.instance.model.userData.dungeon_info;
        int state = GameMainManager.instance.model.userData.dungeonState;
        switch(state)
        {
            case 0:
            case 1:
                image.sprite = sprites[0];
                countDownText.gameObject.SetActive(true);
                break;
            case 2:
                image.sprite = sprites[1];
                countDownText.gameObject.SetActive(false);
                break;
            case 3:
                image.sprite = sprites[0];
                countDownText.gameObject.SetActive(false);
                break;
        }

    }

    private void UpdateCountDown()
    {
        int t = countDownTime;
        if (t != lastTime)
        {
            TimeSpan ts = new TimeSpan(0, 0, t);
            countDownText.text = string.Format("{0}:{1}:{2}", ts.Hours.ToString("D2"), ts.Minutes.ToString("D2"), ts.Seconds.ToString("D2")); ;
            lastTime = t;

            if (t == 0)
            {
                countDownText.gameObject.SetActive(false);
            }
        }

    }

    public void OnClickBtn()
    {
        if(GameMainManager.instance.model.userData.dungeonState==2)
        {
            //打开奖品领取
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIDungeonGetRewardWindow, data);
        }else
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIDungeonWindow);
        }
    }

    
}
