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
    private float goldProgress;
    private UserData user;

    private void Awake()
    {
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_MAPINFO, OnUpdateMapInfoHandle);
        user = GameMainManager.instance.model.userData;
    }
    private void Start()
    {
        
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
            UpdateMoneyBox(user.mapInfo);
        }
        largeGold.anchoredPosition = Vector2.Lerp(largeGold.anchoredPosition, new Vector2(0, goldProgress * 100),1f * Time.deltaTime) ;
        goldSlider.value = Vector2.Lerp(new Vector2(goldSlider.value,0),new Vector2(goldProgress,0),1f * Time.deltaTime).x ;
    }

    protected override void StartShowWindow(object[] data)
    {

        updateMapInfo();
        largeGold.anchoredPosition = new Vector2(0, goldProgress * 100);
        goldSlider.value = goldProgress;
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
        if (user.mapInfo != null)
        {
            updateTime = Time.time;
            MapInfoData mapinfo = user.mapInfo;
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
            scrollView.SetSelected(user.islandId>1? user.islandId-2:0);
            long produce = 0;
            foreach (MinesData md in mapinfo.mines)
            {
                if (md.miner != 0 && md.miner - 1 < md.produces.Length)
                {
                    produce += md.produces[md.miner - 1];
                }

            }

            goldEveryDayText.text = string.Format("{0}/天", GameUtils.GetCurrencyString(produce));
            //curGoldText.text = GameUtils.GetCurrencyString((long)mapinfo.moneyBox);
            allGoldText.text = GameUtils.GetCurrencyString(mapinfo.limit);
            UpdateMoneyBox(mapinfo);

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
        goldProgress = (float)moneyBox / mapInfo.limit;
        
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
