using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class UIDungeonBoss : MonoBehaviour {

    [SerializeField]
    private Image bossBody;
    [SerializeField]
    private Image bossEyes;
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private RectTransform cannon;
    [SerializeField]
    private RectTransform shell;
    [SerializeField]
    private TextMeshProUGUI hitText;
    [SerializeField]
    private Image box;
    [SerializeField]
    private Sprite[] boxStateSprite;
    [SerializeField]
    private Image boom;
    [SerializeField]
    private Image smokeBoss;
    [SerializeField]
    private Image smokeCannon;

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

    private enum BossState
    {
        Normal,
        Bruise,//受伤
        Anger//愤怒
    }
	// Use this for initialization
	void Start () {

        cannon.gameObject.SetActive(false);
        shell.gameObject.SetActive(false);
        hitText.gameObject.SetActive(false);
        box.gameObject.SetActive(false);
        boom.gameObject.SetActive(false);
        smokeBoss.gameObject.SetActive(false);
        smokeCannon.gameObject.SetActive(false);
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

    private void OnDisable()
    {
        smokeBoss.gameObject.SetActive(false);
        smokeCannon.gameObject.SetActive(false);
        cannon.gameObject.SetActive(false);
        shell.gameObject.SetActive(false);
        hitText.gameObject.SetActive(false);
        box.gameObject.SetActive(false);
        boom.gameObject.SetActive(false);
        smokeBoss.gameObject.SetActive(false);
        smokeCannon.gameObject.SetActive(false);
    }

    public void SetData(DungeonInfoData info)
    {

        data = info;
        hpSlider.value = 1;
        hpText.text = string.Format("{0}/{1}", data.boss_hp, data.boss_hp);
        SetBossState(BossState.Normal);

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

    private void SetBossState(BossState state)
    {
        bossBody.gameObject.SetActive(true);
        switch (state)
        {
            case BossState.Anger:
                bossBody.sprite = bodySprites[1];
                bossBody.SetNativeSize();
                bossEyes.gameObject.SetActive(false);
                AudioManager.instance.PlaySound(AudioNameEnum.dungeon_bossWild);
                break;
            case BossState.Bruise:
                bossBody.sprite = bodySprites[2];
                bossBody.SetNativeSize();
                bossEyes.gameObject.SetActive(false);
                AudioManager.instance.PlaySound(AudioNameEnum.dungeon_bossHurt);
                break;
            case BossState.Normal:
                bossBody.sprite = bodySprites[0];
                bossBody.SetNativeSize();
                bossEyes.gameObject.SetActive(true);
                break;
        }

    }

    public void Attack()
    {
        AudioManager.instance.PlaySound(AudioNameEnum.dungeon_cardToCannon);
        smokeCannon.gameObject.SetActive(true);
        Sequence sq = DOTween.Sequence();
        sq.AppendInterval(1f);
        sq.AppendCallback(() =>
        {
            AudioManager.instance.PlaySound(AudioNameEnum.shoot_aim_target);
            cannon.gameObject.SetActive(true);
            shell.gameObject.SetActive(true);
            shell.anchoredPosition = new Vector2(0, 200);
            shell.localScale = Vector3.one;
        });
        sq.AppendInterval(1f);
        sq.AppendCallback(() => {
            AudioManager.instance.PlaySound(AudioNameEnum.shoot_fire);
            AudioManager.instance.PlaySound(AudioNameEnum.shoot_bomb_fly);
        });
        sq.Append(shell.DOMove(bossBody.transform.position , 1).SetEase(Ease.OutBack));
        sq.Insert(2f, shell.DOScale(new Vector3(0.5f, 0.5f, 1), 1).SetEase(Ease.OutQuad));
        sq.AppendCallback(() =>
        {
            AudioManager.instance.PlaySound(AudioNameEnum.shoot_boob_explode);
            shell.gameObject.SetActive(false);
            boom.gameObject.SetActive(true);
            if(data.cards>data.boss_hp)
            {
                SetBossState(BossState.Anger); 
            }else
            {
                SetBossState(BossState.Bruise);
            }

            hitText.gameObject.SetActive(true);
            hitText.text = "-" + data.cards.ToString();
            hpText.text = string.Format("{0}/{1}", data.boss_hp - data.cards, data.boss_hp);

        });
        sq.Append(hpSlider.DOValue((data.boss_hp - data.cards)/(float)data.boss_hp,0.5f));
        sq.AppendInterval(0.5f);
        sq.AppendCallback(() =>
        {
            
            AudioManager.instance.PlaySound(AudioNameEnum.dungeon_bossToBox);
            smokeBoss.gameObject.SetActive(true);
        });
        sq.AppendInterval(0.5f);
        sq.AppendCallback(()=> {
            AudioManager.instance.PlaySound(AudioNameEnum.building_box_down);
            bossBody.gameObject.SetActive(false);
            hitText.gameObject.SetActive(false);
            box.gameObject.SetActive(true);
            box.sprite = boxStateSprite[0];
        });
        sq.AppendInterval(0.5f);
        sq.AppendCallback(() =>
        {
            box.sprite = boxStateSprite[1];
        });
    }
}
