using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIStealIsland : MonoBehaviour {

    public IslandFactory island;
    public HeadIcon head;
    public TextMeshProUGUI labelMoney;
    public GameObject richIcon;

    public void setData(StealIslandData islandData)
    {
        island.UpdateCityData(islandData.islandId, islandData.buildings);
        head.gameObject.SetActive(false);
        labelMoney.gameObject.SetActive(false);
        richIcon.SetActive(false);
    }

    public void setData(TargetData targetData)
    {
        head.setData(targetData.name, targetData.headImg, targetData.crowns, targetData.isVip);
        head.gameObject.SetActive(true);
        richIcon.SetActive(targetData.isRichMan);

        labelMoney.text = GameUtils.GetCurrencyString(targetData.money);
        labelMoney.gameObject.SetActive(true);

        (labelMoney.transform as RectTransform).DOAnchorPos(new Vector2(0, 100), 1).SetEase(Ease.OutBounce).From().SetRelative(true);
    }

}
