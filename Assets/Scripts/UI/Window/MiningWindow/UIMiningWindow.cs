using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMiningWindow :UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIMiningWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public AutoScrollView scrollView;
    public TextMeshProUGUI goldEveryDayText;
    public TextMeshProUGUI curGoldText;
    public TextMeshProUGUI allGoldText;
    public RectTransform largeGold;
    public GameObject goldEfect;
    public Slider goldSlider;
    public Button reapBtn;

    public GameObject buyMinerPanel;
    public UIMiningBuyMinerItem[] buyMinerItems;

    private float updateTime;
    private long moneyBox;
    private int timeTag;

    private void Awake()
    {
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_MAPINFO, OnUpdateMapInfoHandle);
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_MAPINFO, OnUpdateMapInfoHandle);
    }

    
    private void Update()
    {
        int f = (int)((Time.time - updateTime) / 3);
        if (f != timeTag)
        {
            timeTag = f;
            UpdateMoneyBox(GameMainManager.instance.model.userData.mapInfo);
        }
        
    }

    protected override void StartShowWindow(object[] data)
    {
        
        updateMapInfo();
        
    }

    protected override void EnterAnimation(Action onComplete)
    {
        onComplete();
    }

    protected override void ExitAnimation(Action onComplete)
    {
        onComplete();
    }

    private void updateMapInfo()
    {
        goldEfect.SetActive(false);
        buyMinerPanel.gameObject.SetActive(false);
        if (GameMainManager.instance.model.userData.mapInfo != null)
        {
            updateTime = Time.time;
            MapInfoData mapinfo = GameMainManager.instance.model.userData.mapInfo;
            List<UIMiningIslandItem.MiningItemData> list = new List<UIMiningIslandItem.MiningItemData>();
            for (int i = 0; i < mapinfo.islandNames.Length; i++)
            {
                UIMiningIslandItem.MiningItemData itemData = new UIMiningIslandItem.MiningItemData();
                itemData.name = mapinfo.islandNames[i];
                itemData.onAddMiner = OnAddMinerHandle;
                itemData.islandID = (i + 1) % (mapinfo.islandNames.Length);
                if (i < mapinfo.mines.Length)
                {
                    itemData.minesData = mapinfo.mines[i];
                }
                list.Add(itemData);
            }
            scrollView.SetData(list);

            long produce = 0;
            foreach (MinesData md in mapinfo.mines)
            {
                if (md.miner != 0 && md.miner - 1 < md.produces.Length)
                {
                    produce += md.produces[md.miner - 1];
                }

            }

            goldEveryDayText.text = string.Format("{0}/天", GameUtils.GetCurrencyString(produce));
            UpdateMoneyBox(mapinfo);
            //curGoldText.text = GameUtils.GetCurrencyString((long)mapinfo.moneyBox);
            allGoldText.text = GameUtils.GetCurrencyString(mapinfo.limit);

            float f = (float)mapinfo.moneyBox / mapinfo.limit;
            largeGold.anchoredPosition = new Vector2(0, f * 100);
            goldSlider.value = f;
        }
    }

    private void UpdateMoneyBox(MapInfoData mapInfo)
    {
        moneyBox = (long)mapInfo.moneyBox;
        moneyBox += (long)((Time.time - updateTime) * mapInfo.producePerSecond);
        curGoldText.text = GameUtils.GetCurrencyString(moneyBox);
        if (moneyBox > 100)
        {
            reapBtn.interactable = true;
        }
        else
        {
            reapBtn.interactable = false;
        }
        
    }
    private void OnUpdateMapInfoHandle(BaseEvent e)
    {
        updateMapInfo();
    }
    private void OnAddMinerHandle(MinesData mines)
    {
        buyMinerPanel.gameObject.SetActive(true);
        for(int i = 0;i<buyMinerItems.Length;i++)
        {
            buyMinerItems[i].SetData(mines);
        }
    }

    public void OnCloseAddMinerPanel()
    {
        buyMinerPanel.gameObject.SetActive(false);
    }


    public new void OnClickClose()
    {
        GameMainManager.instance.uiManager.ChangeState(new MiningMapState());
    }

    public void OnClickGetGoldBtn()
    {
        GameMainManager.instance.netManager.ReapMine((ret,res) =>
        {
            if(res.isOK)
            {
                updateTime = Time.time;
                goldEfect.SetActive(false);
                goldEfect.SetActive(true);
                UpdateMoneyBox(res.data.mapInfo);
            }
        });
    }
}
