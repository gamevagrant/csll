using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIDungeonCardFishMovieClip : MonoBehaviour {

    public RectTransform lightImage;
    public RectTransform fishAnimation;
    public RectTransform targetFrom;

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            StartPlay();
        }
    }

    public void StartPlay()
    {
        lightImage.gameObject.SetActive(false);
        fishAnimation.anchoredPosition = Vector2.zero;

        Sequence sq = DOTween.Sequence();
        sq.Append(fishAnimation.DOMove(targetFrom.position, 0.5f).From());
        sq.AppendCallback(() =>
        {
            lightImage.gameObject.SetActive(true);
        });
        sq.Append(lightImage.DORotate(new Vector3(0,0,360), 2f, RotateMode.FastBeyond360));
        sq.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
