using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIDungeonBoss : MonoBehaviour {

    [SerializeField]
    private Image bossBody;
    [SerializeField]
    private Image bossEyes;
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private Sprite[] bodySprites;
    [SerializeField]
    private Sprite[] eyesSprite;

    private DungeonInfoData data;
    private int lastTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(data!=null)
        {
            UpdateCountDown();
        }

        float f = Time.unscaledTime % 3;
        Sprite sp;
        if (f < 2.5f)
        {
            sp = eyesSprite[0];
        }
        else
        {
            sp = eyesSprite[1];
        }
        if (bossEyes.sprite != sp)
        {
            bossEyes.sprite = sp;
        }
    }

    public void SetData(DungeonInfoData info)
    {
        data = info;
        hpSlider.value = (float)data.cards / data.boss_hp;
        hpText.text = string.Format("{0}/{1}", data.cards, data.boss_hp);
    }

    private void UpdateCountDown()
    {
        int t = data.countDown;
        if (t != lastTime)
        {
            TimeSpan ts = new TimeSpan(0, 0, t);
            timeText.text = string.Format("{0}:{1}:{2}", ts.Hours.ToString("D2"), ts.Minutes.ToString("D2"), ts.Seconds.ToString("D2")); ;
            lastTime = t;

        }

    }
}
