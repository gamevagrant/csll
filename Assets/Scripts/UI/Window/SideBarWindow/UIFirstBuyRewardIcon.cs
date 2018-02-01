using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class UIFirstBuyRewardIcon : MonoBehaviour {

    public TextMeshProUGUI text;

    private FirstBuyingReward data;
    private long lastTime;
    private long countDownTime
    {
        get
        {
            long t = (long)Mathf.Max(0, data.countdown - (Time.unscaledTime - data.timeTag));
            return t;
        }
    }
    // Use this for initialization
    void Start () {


    }

    private void Update()
    {
        UpdateCountDown();
        
    }

    private void OnEnable()
    {
        if(Application.isPlaying)
        {

            Refresh();
        }
    }

    private void Refresh()
    {
        data = GameMainManager.instance.model.userData.one_yuan_buying;
        gameObject.SetActive(data.isShow);
    }

    private void UpdateCountDown()
    {
        long t = countDownTime;
        if (t != lastTime)
        {
            TimeSpan ts = new TimeSpan(t * 10000000L);
            string str = "";
            str = string.Format("{0}:{1}:{2}", ts.Hours.ToString("D2"), ts.Minutes.ToString("D2"), ts.Seconds.ToString("D2"));
            text.text = str;
            lastTime = t;

            if (t == 0)
            {
                gameObject.SetActive(false);
            }
        }


    }
}
