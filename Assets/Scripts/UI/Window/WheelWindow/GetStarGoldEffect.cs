using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GetStarGoldEffect : MonoBehaviour {

    public GameObjectPool starPool;//星星倍的星星
    public TextMeshProUGUI starGoldText;//星星钱数量
    public TextMeshProUGUI starRateText;//星星倍率
    private int _starCount;
    private int starCount
    {
        set
        {
            _starCount = value;
            tmp = value / 2;
        }
        get
        {
            return _starCount;
        }
    }

    private int tmp;

    private void Awake()
    {
        starGoldText.gameObject.SetActive(true);
        starRateText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(starCount != 0)
        {
            tmp = Mathf.CeilToInt(Vector3.Lerp(new Vector3(tmp, 0, 0), new Vector3(starCount, 0, 0), 0.9f * Time.deltaTime * 10f).x);
            starGoldText.text = tmp.ToString();
        }else
        {
            starGoldText.text = "";
        }
        
    }
    public void Show(int star,int rate,System.Action onComplate)
    {
        starPool.resetAllTarget();
        RectTransform backStar = starPool.getIdleTarget<RectTransform>();
        backStar.transform.localScale = Vector3.zero;
        starRateText.gameObject.SetActive(false);
        starRateText.transform.localScale = new Vector3(2,2,2);
        starCount = 0;

        int money = star * rate;
        GetStarPosEvent evt = new GetStarPosEvent((pos) =>
        {
            
            Sequence sq = DOTween.Sequence();
            sq.Append(backStar.DOScale(new Vector3(3,3,3), 0.5f).SetEase(Ease.OutBack));
            for (int i = 0; i < 5; i++)
            {
                Image image = starPool.getIdleTarget<Image>();
                image.transform.position = pos;
                image.transform.DOMove(transform.position, 0.5f).SetEase(Ease.InCubic).SetDelay(i * 0.2f);
                image.DOFade(1, 0.3f).SetDelay(i * 0.2f + 0.2f).OnComplete(() => {
                    starCount = star;
                    image.gameObject.SetActive(false);
                });

            }
            sq.AppendInterval(5 * 0.2f);
            sq.AppendCallback(()=> {
                
                starRateText.gameObject.SetActive(true);
                starRateText.text = "X" + rate.ToString();
            });
            sq.Append(starRateText.transform.DOScale(new Vector3(1,1,1),0.7f).SetEase(Ease.InBack));
            sq.AppendCallback(() =>
            {

                if(onComplate != null)
                {
                    onComplate();
                }
            });
            sq.AppendInterval(1);
            sq.OnComplete(() =>
            {
                gameObject.SetActive(false);
            });

        });

        EventDispatcher.instance.DispatchEvent(evt);
    }
}
