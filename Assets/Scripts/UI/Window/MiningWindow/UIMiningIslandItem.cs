using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;

public class UIMiningIslandItem : BaseItemView {

    public SpriteAtlas spriteAtlas;
    public Image[] heads;
    public GameObject[] miners;
    public GameObject panel;
    public GameObject minerPanel;
    public GameObject fullMiner;
    public TextMeshProUGUI islandNameText;
    public TextMeshProUGUI incomeText;


    private Image image;

    private MiningItemData data;
    //private static bool isLoad = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        
    }

    public override void SetData(object data)
    {
        this.data = data as MiningItemData;
        int state = GameMainManager.instance.model.userData.islandId>=this.data.islandID?0:1;
        if(this.data.islandID == 0)
        {
            state = 0;
        }
        image.sprite = spriteAtlas.GetSprite(string.Format("island_{0}_{1}", this.data.islandID, state));
        islandNameText.text = this.data.name;
        incomeText.text = "";
        panel.SetActive(false);
        minerPanel.SetActive(false);
        fullMiner.SetActive(false);

        if (this.data.minesData != null)
        {
            if(this.data.minesData.miner == 0)
            {
                incomeText.text = "";
            }else if(this.data.minesData.miner - 1 < this.data.minesData.produces.Length)
            {
                incomeText.text = string.Format("{0}/天", GameUtils.GetCurrencyString(this.data.minesData.produces[this.data.minesData.miner - 1]));
            }
            
            for (int i = 0; i < heads.Length; i++)
            {
                if (i < this.data.minesData.miner)
                {
                    heads[i].gameObject.SetActive(true);
                    miners[i].SetActive(true);
                }
                else
                {
                    heads[i].gameObject.SetActive(false);
                    miners[i].SetActive(false);
                }

            }
        }

        Debug.Log("setData");
    }

    public override void OnSelected(bool isSelected)
    {
        if(this.data.minesData != null && this.data.minesData.island < GameMainManager.instance.model.userData.islandId)
        {
            if (data.minesData.miner < data.minesData.produces.Length)
            {
                panel.SetActive(isSelected);
                Debug.Log("panel"+ isSelected);
            }else
            {
                fullMiner.SetActive(isSelected);
                Debug.Log("fullMiner" + isSelected);
            }
            minerPanel.SetActive(isSelected);
            Debug.Log("minerPanel" + isSelected);
            Debug.Log(data.islandID + data.name);
        }
       
        
    }

    public void OnClickAddMinerBtn()
    {
        if(data.onAddMiner!= null)
        {
            data.onAddMiner(data.minesData);
        }
    }

    public class MiningItemData
    {
        public int islandID;
        public string name;
        public MinesData minesData;
        public System.Action<MinesData> onAddMiner;
    }
}
