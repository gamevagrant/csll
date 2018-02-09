using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class UIDungeonIcon : MonoBehaviour {

    private enum DungeonState
    {
        Open,
        Reward,
        Close,
    }

    public TextMeshProUGUI countDownText;
    public Image image;
    public Sprite[] sprites; 


    private DungeonInfoData data;
    private int lastTime;
    private int countDownTime
    {
        get
        {
            if(data!=null)
            {
                int t = data.countDown;
                return t;
            }
            return 0;
        }
    }

    private DungeonState state
    {
        get
        {
            if(data!=null && data.is_reward == 1)
            {
                return DungeonState.Reward;
            }else if(data != null && GameMainManager.instance.model.userData.dungeon_keys>0)
            {
                return DungeonState.Open;
            }else
            {
                return DungeonState.Close;
            }
            
        }
    }
    // Use this for initialization
    void Start () {
		
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

    private void Refresh()
    {
        data = GameMainManager.instance.model.userData.dungeon_info;
        switch(state)
        {
            case DungeonState.Open:
                image.sprite = sprites[0];
                countDownText.gameObject.SetActive(true);
                break;
            case DungeonState.Reward:
                image.sprite = sprites[1];
                countDownText.gameObject.SetActive(false);
                break;
            case DungeonState.Close:
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
        if(state == DungeonState.Reward)
        {
            //打开奖品领取
        }else
        {
            GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIDungeonWindow);
        }
    }
}
