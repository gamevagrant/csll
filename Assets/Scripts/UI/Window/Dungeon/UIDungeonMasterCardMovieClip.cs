using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIDungeonMasterCardMovieClip : MonoBehaviour {

    public RectTransform lightImage;
    public RectTransform cardImage;
    public RectTransform textImage;
    public RectTransform target;

    private void OnEnable()
    {
        if(Application.isPlaying)
        {
            StartPlay();
        }
    }

    private void StartPlay()
    {
        lightImage.gameObject.SetActive(true);
        cardImage.localScale = Vector3.zero;
        cardImage.localPosition = Vector3.zero;
        textImage.localScale = Vector3.zero;

        Sequence sq = DOTween.Sequence();
        sq.Append(lightImage.DOLocalRotate(new Vector3(0, 0, 360), 1, RotateMode.FastBeyond360));
        sq.Insert(0, cardImage.DOScale(1, 0.5f).SetEase(Ease.OutBack));
        sq.AppendCallback(() => {
            lightImage.gameObject.SetActive(false);
        });
        sq.Append(cardImage.DOMove(target.position, 0.5f));
        sq.Append(textImage.DOScale(1, 0.5f).SetEase(Ease.OutBack));
        sq.AppendInterval(0.5f);
        sq.OnComplete(() => {
            gameObject.SetActive(false);
        });
    }

}
