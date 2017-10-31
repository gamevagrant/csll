using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GetMoneyBoxAnimation : MonoBehaviour {

    public Sprite[] boxSprites;

    public Image boxImage;
    public GameObject energy;
    public GameObject money;

    public void SetData(bool getEnergy,bool getMoney)
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_box_down);
        energy.SetActive(false);
        money.SetActive(false);
        boxImage.sprite = boxSprites[0];
        Sequence sq = DOTween.Sequence();
        sq.Append((boxImage.transform as RectTransform).DOAnchorPos(new Vector2(0, 100), 1).SetEase(Ease.OutBounce).From().SetRelative(true));
        sq.AppendCallback(() =>
        {
            boxImage.sprite = boxSprites[1];
            if (getEnergy)
                energy.SetActive(true);
            if (getMoney)
                money.SetActive(true);
        });

    }
}
