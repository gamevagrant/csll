using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QY.UI;

public class UIMiningBuyMinerItem : MonoBehaviour {

    public TextMeshProUGUI priceText;
    public GameObject costPanel;
    public GameObject purchasedTag;
    public Button button;
    public int index;

    private MinesData data;

    private void Awake()
    {
        button.onClick.AddListener(OnClickBuyBtn);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
    public void SetData(MinesData data)
    {
        this.data = data;
        
        if(index == data.miner)
        {
            button.interactable = true;
            button.enabled = true;
            priceText.text = GameUtils.GetCurrencyString(data.costs[index]);
            costPanel.SetActive(true);
            purchasedTag.SetActive(false);
        }else if(index < data.miner)
        {
            button.interactable = true;
            button.enabled = false;
            costPanel.SetActive(false);
            purchasedTag.SetActive(true);
        }else
        {
            button.enabled = false;
            button.interactable = true;
            costPanel.SetActive(false);
            purchasedTag.SetActive(false);
        }
    }

    private void OnClickBuyBtn()
    {
        if(GameMainManager.instance.model.userData.money< data.costs[index])
        {
            Alert.Show("金币不足");
            return;
        }
        GameMainManager.instance.netManager.BuyMiner(data.island, (ret, res) =>
        {

        });
    }
}
