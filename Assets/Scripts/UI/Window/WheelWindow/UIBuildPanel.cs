using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using DG.Tweening;
using TMPro;
public class UIBuildPanel : MonoBehaviour {

    public TextMeshProUGUI cityName;
    public GameObject buildingAnimation;
    public GameObject star;
    public IslandFactory islandFactory;

    public GameObject upgradePanel;
    public GetMoneyBoxAnimation box;
    public GestureTrigger gestureTrigger;

    [SerializeField]
    private RectTransform buildBtn;
    [SerializeField]
    private RectTransform switchBtn;
    [SerializeField]
    private RectTransform panel;
    [SerializeField]
    private RectTransform[] repairBtns;

    private SpriteAtlas spriteAtlas;

    private Vector2 buildBtnOriginalValue;//roll点按钮位置原始值 下同
    private Vector2 switchOriginalValue;
    private Vector2 panelLocalOriginalValue;
    //private int islandID;
    private UserData user;
    private BuildComplateEvent buildComplateData;

    private System.Action<bool> onUpgrading;

    private void Awake()
    {
        EventDispatcher.instance.AddEventListener(EventEnum.BUILD_COMPLATE, OnBuildComplateHandle);
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_BUILDING, OnUpdateBuilding);

        buildBtnOriginalValue = buildBtn.anchoredPosition;
        switchOriginalValue = switchBtn.anchoredPosition;
        panelLocalOriginalValue = panel.anchoredPosition;

        buildBtn.anchoredPosition = new Vector2(buildBtn.anchoredPosition.x, -300);
        switchBtn.anchoredPosition = new Vector2(-200, switchBtn.anchoredPosition.y);
        panel.anchoredPosition = new Vector2(400, -150);
        panel.localScale = new Vector3(0.5f, 0.5f, 1);



        buildBtn.gameObject.SetActive(false);
        switchBtn.gameObject.SetActive(false);
        buildingAnimation.SetActive(false);
        star.SetActive(false);
        upgradePanel.SetActive(false);
        box.gameObject.SetActive(false);
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.BUILD_COMPLATE, OnBuildComplateHandle);
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_BUILDING, OnUpdateBuilding);
    }

    public void SetData(int islandID,BuildingData[] data,System.Action<bool> onUpgrading)
    {
        user = GameMainManager.instance.model.userData;
        //this.islandID = islandID;
        islandFactory.UpdateCityData(user.islandId, user.buildings);
        
        cityName.text = GameMainManager.instance.configManager.islandConfig.GetIslandName(user.islandId);

        this.onUpgrading = onUpgrading;

        UpdateRepairBtns(data);
    }


    /// <summary>
    /// 切换到显示转盘界面时
    /// </summary>
    public void EnterToWheelPanelState()
    {
        DOTween.To(() => buildBtn.anchoredPosition, p => buildBtn.anchoredPosition = p, new Vector2(buildBtn.anchoredPosition.x, -300), 1).SetEase(Ease.OutQuint);
        DOTween.To(() => switchBtn.anchoredPosition, p => switchBtn.anchoredPosition = p, new Vector2(-200, switchBtn.anchoredPosition.y), 1);
        DOTween.To(() => panel.anchoredPosition, x => panel.anchoredPosition = x, new Vector2(200, -150), 1);
        DOTween.To(() => panel.localScale, x => panel.localScale = x, new Vector3(0.5f,0.5f,1), 1).onComplete = () => {

            buildBtn.gameObject.SetActive(false);
            switchBtn.gameObject.SetActive(false);
        };
    }

    /// <summary>
    /// 切换到显示建造界面时
    /// </summary>
    public void EnterToBuildPanelState()
    {
        GameMainManager.instance.uiManager.DisableOperation();
        buildBtn.gameObject.SetActive(true);
        switchBtn.gameObject.SetActive(true);

        DOTween.To(() => buildBtn.anchoredPosition, p => buildBtn.anchoredPosition = p, buildBtnOriginalValue, 1).SetEase(Ease.InQuint);
        DOTween.To(() => switchBtn.anchoredPosition, p => switchBtn.anchoredPosition = p, switchOriginalValue, 1);
        DOTween.To(() => panel.anchoredPosition, x => panel.anchoredPosition = x,panelLocalOriginalValue, 1).SetEase(Ease.InQuint);
        DOTween.To(() => panel.localScale, x => panel.localScale = x, Vector3.one, 1).onComplete = () => {

            QY.Guide.GuideManager.instance.state = "building";
            GameMainManager.instance.uiManager.EnableOperation();
        };
    }

    public void EnterToHideState(System.Action onComplate)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(DOTween.To(() => buildBtn.anchoredPosition, p => buildBtn.anchoredPosition = p, new Vector2(buildBtn.anchoredPosition.x, -300), 1).SetEase(Ease.OutExpo));
        sq.Insert(0.5f, DOTween.To(() => switchBtn.anchoredPosition, p => switchBtn.anchoredPosition = p, new Vector2(-200, switchBtn.anchoredPosition.y), 1).SetEase(Ease.OutCubic));
        sq.Insert(0.5f, DOTween.To(() => panel.anchoredPosition, x => panel.anchoredPosition = x, new Vector2(500, -150), 1).SetEase(Ease.OutBack));
        sq.OnComplete(() =>
        {
            onComplate();
        });

    }

    public void ShowBuildState()
    {
        buildBtn.gameObject.SetActive(true);
        switchBtn.gameObject.SetActive(true);

        buildBtn.anchoredPosition = buildBtnOriginalValue;
        switchBtn.anchoredPosition = switchOriginalValue;
        panel.anchoredPosition = panelLocalOriginalValue;
        panel.localScale = Vector3.one;
    }

    public void ShowWheelState()
    {
        buildBtn.anchoredPosition = new Vector2(buildBtn.anchoredPosition.x, -300);
        switchBtn.anchoredPosition = new Vector2(-200, switchBtn.anchoredPosition.y);
        panel.anchoredPosition = new Vector2(200, -150);
        panel.localScale = new Vector3(0.5f, 0.5f, 1);

        buildBtn.gameObject.SetActive(false);
        switchBtn.gameObject.SetActive(false);
    }

    public void onClickBuildBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIBuildingWindow);
    }

    public void OnClickNextIsland()
    {
        UpgradeIsland();
       
    }

    public void OnClickRepairBtn(int index)
    {
        GameMainManager.instance.netManager.Build(GameMainManager.instance.model.userData.islandId, index - 1, (ret, data) => {
            if (ret && data.isOK)
            {

                BuildComplateEvent evt = new BuildComplateEvent();
                evt.buildIndex = index;
                evt.level = data.data.buildings[index - 1].level;
                evt.status = data.data.buildings[index - 1].status;
                evt.isRepair = true;
                evt.islandID = data.data.islandId;
                evt.isUpgrade = data.data.playUpgradeAnimation;
                evt.upgradeEnergyReward = data.data.upgradeEnergyAfterReward - data.data.energy;
                evt.upgradeMoneyReward = data.data.upgradeMoneyAfterReward - data.data.money;
                EventDispatcher.instance.DispatchEvent(evt);

               
            }

        });
    }

    private void UpdateRepairBtns(BuildingData[] data)
    {
        for (int i = 0; i < repairBtns.Length; i++)
        {
            if (i < data.Length && data[i].status == 1)
            {
                repairBtns[i].gameObject.SetActive(true);
            }
            else
            {
                repairBtns[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnBuildComplateHandle(BaseEvent e)
    {
        BuildComplateEvent evt = e as BuildComplateEvent;
        buildComplateData = evt;

        repairBtns[buildComplateData.buildIndex-1].gameObject.SetActive(buildComplateData.status==1);
        if (!evt.isUpgrade)
        {
            StartCoroutine(showBuildAnimation(evt.buildIndex, evt.level, buildComplateData.isRepair));
        }else
        {
            GameMainManager.instance.uiManager.DisableOperation();
            StartCoroutine(showBuildAnimation(evt.buildIndex, GameMainManager.instance.model.userData.buildings[evt.buildIndex-1].level+1,buildComplateData.isRepair,()=> 
            {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_level_up);
                GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UITopBarWindow);
                
                upgradePanel.SetActive(true);
                onUpgrading(true);
            }));
            switchBtn.gameObject.SetActive(false);
            buildBtn.gameObject.SetActive(false);

        }
       
    }

    private void OnUpdateBuilding(BaseEvent e)
    {
        islandFactory.UpdateCityData(user.islandId, user.buildings);
    }

    private void UpgradeIsland()
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_island_change);
        //UserData ud = GameMainManager.instance.model.userData;
        upgradePanel.SetActive(false);
        gestureTrigger.isIgnoreLock = true;

        Sequence sq = DOTween.Sequence();
        sq.Append((panel.transform as RectTransform).DOAnchorPos(new Vector2(-800, 0), 1f).SetEase(Ease.InBack));
        sq.AppendCallback(() =>
        {
            cityName.text = GameMainManager.instance.configManager.islandConfig.GetIslandName(user.islandId);
            islandFactory.UpdateCityData(user.islandId, user.buildings);
            (panel.transform as RectTransform).anchoredPosition = new Vector2(800, 0);
        });
        sq.Insert(1.2f,(panel.transform as RectTransform).DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBack));
        sq.AppendInterval(1);
        sq.AppendCallback(() =>
        {
            box.gameObject.SetActive(true);
            box.SetData(buildComplateData.upgradeEnergyReward > 0, buildComplateData.upgradeMoneyReward > 0);
            EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Energy, 0));
            EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money, 0));
            EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.star, 0));

            gestureTrigger.isIgnoreLock = false;
            GameMainManager.instance.uiManager.EnableOperation();

            string content = string.Format("哇哦 恭喜您又建成一个岛屿，送你奖励继续加油哦！能量：{0} 金币：{1}", buildComplateData.upgradeEnergyReward.ToString(),GameUtils.GetCurrencyString(buildComplateData.upgradeMoneyReward));
            Alert.ShowPopupBox(content, () =>
            {
                upgradePanel.SetActive(false);
                box.gameObject.SetActive(false);
                buildBtn.gameObject.SetActive(true);

                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIWheelWindow, 0);
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UITopBarWindow);
               
                switchBtn.gameObject.SetActive(true);
                onUpgrading(false);

               
            }, "领取");

        });
    }

    private IEnumerator showBuildAnimation(int index,int level,bool isRepair,System.Action onComplate = null)
    {
        //yield return new WaitForSeconds(0.5f);
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_upgrade);
        buildingAnimation.transform.position = islandFactory.GetBuildTransform(index).position;
        islandFactory.HideBuild(index);
        buildingAnimation.SetActive(true);
        yield return new WaitForSeconds(1);
        buildingAnimation.SetActive(false);
        BuildingData bd = new BuildingData();
        bd.level = level;
        bd.status = 0;
        islandFactory.UpdateBuildingData(index, bd);
        islandFactory.ShowBuild(index);
        EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.Money,0));
        if(!isRepair)
        {
            EventDispatcher.instance.DispatchEvent(new GetStarPosEvent((pos) => {
                star.transform.position = islandFactory.GetBuildTransform(index).position;
                star.SetActive(true);
                Sequence sq = DOTween.Sequence();
                sq.Append((star.transform as RectTransform).DOAnchorPos(new Vector2(0, 30), 0.3f).SetRelative().SetEase(Ease.OutCubic));
                sq.Append((star.transform as RectTransform).DOAnchorPos(new Vector2(0, -30), 0.7f).SetRelative().SetEase(Ease.OutBounce));
                sq.Append(star.transform.DOMove(pos, 1).SetEase(Ease.InCubic));
                sq.OnComplete(() => {
                    star.SetActive(false);
                    EventDispatcher.instance.DispatchEvent(new UpdateBaseDataEvent(UpdateBaseDataEvent.UpdateType.star, 0));
                });

            }));

        }


        if (onComplate != null)
        {
            yield return new WaitForSeconds(2);
            onComplate();
        }
    }
}
