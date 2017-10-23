using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIStealIsland : MonoBehaviour {

    public IslandFactory island;
    public HeadIcon head;
    public Text labelMoney;

    public void setData(StealIslandData islandData)
    {
        island.UpdateCityData(islandData.islandId, islandData.buildings);
        head.gameObject.SetActive(false);
        labelMoney.gameObject.SetActive(false);
    }

    public void setData(TargetData targetData)
    {
        head.setData(targetData.name, targetData.headImg, targetData.crowns, targetData.isVip);
        head.gameObject.SetActive(true);

        labelMoney.text = targetData.money.ToString();
        labelMoney.gameObject.SetActive(true);

        (labelMoney.transform as RectTransform).DOAnchorPos(new Vector2(0, 100), 1).SetEase(Ease.OutBounce).From().SetRelative(true);
    }

}
