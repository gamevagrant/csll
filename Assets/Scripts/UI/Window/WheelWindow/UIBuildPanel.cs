using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using DG.Tweening;

public class UIBuildPanel : MonoBehaviour {

    public Text cityName;
    public GameObject buildingAnimation;
    public IslandFactory islandFactory;

    public GameObject upgradePanel;
    public GetMoneyBoxAnimation box;

    [SerializeField]
    private GameObject buildBtn;
    [SerializeField]
    private GameObject switchBtn;
    [SerializeField]
    private GameObject panel;

    private SpriteAtlas spriteAtlas;

    private Vector2 buildBtnOriginalValue;//roll点按钮位置原始值 下同
    private Vector2 switchOriginalValue;
    private Vector2 panelLocalOriginalValue;
    private int islandID;
    private BuildComplateEvent buildComplateData;

    private void Awake()
    {
        EventDispatcher.instance.AddEventListener(EventEnum.BUILD_COMPLATE, OnBuildComplateHandle);

        buildBtnOriginalValue = (buildBtn.transform as RectTransform).anchoredPosition;
        switchOriginalValue = (switchBtn.transform as RectTransform).anchoredPosition;
        panelLocalOriginalValue = (panel.transform as RectTransform).anchoredPosition;

        (buildBtn.transform as RectTransform).anchoredPosition = new Vector2((buildBtn.transform as RectTransform).anchoredPosition.x, -300);
        (switchBtn.transform as RectTransform).anchoredPosition = new Vector2(-200, (switchBtn.transform as RectTransform).anchoredPosition.y);

        panel.transform.localScale = new Vector3(0.5f,0.5f,1);
        (panel.transform as RectTransform).anchoredPosition = new Vector2(200,-150);

        buildBtn.SetActive(false);
        switchBtn.SetActive(false);
        buildingAnimation.SetActive(false);

        upgradePanel.SetActive(false);
        box.gameObject.SetActive(false);
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.BUILD_COMPLATE, OnBuildComplateHandle);
    }

    public void setData(int islandID,BuildingData[] data,MapInfoData mapInfo)
    {
        this.islandID = islandID;
        islandFactory.UpdateCityData(islandID, data);
        cityName.text = mapInfo.islandNames[islandID-1];

    }

    /// <summary>
    /// 切换到显示转盘界面时
    /// </summary>
    public void enterToWheelPanelState()
    {
        RectTransform buildBtnTF = buildBtn.transform as RectTransform;
        DOTween.To(() => buildBtnTF.anchoredPosition, p => buildBtnTF.anchoredPosition = p, new Vector2(buildBtnTF.anchoredPosition.x, -300), 1).SetEase(Ease.OutQuint); ;

        RectTransform switchBtnTF = switchBtn.transform as RectTransform;
        DOTween.To(() => switchBtnTF.anchoredPosition, p => switchBtnTF.anchoredPosition = p, new Vector2(-200, switchBtnTF.anchoredPosition.y), 1);

        RectTransform buildPanelTF = panel.transform as RectTransform;
        DOTween.To(() => buildPanelTF.anchoredPosition, x => buildPanelTF.anchoredPosition = x, new Vector2(200, -150), 1);
        DOTween.To(() => buildPanelTF.localScale, x => buildPanelTF.localScale = x, new Vector3(0.5f,0.5f,1), 1).onComplete = () => {

            buildBtn.SetActive(false);
            switchBtn.SetActive(false);
        };
    }

    /// <summary>
    /// 切换到显示建造界面时
    /// </summary>
    public void enterToBuildPanelState()
    {
        GameMainManager.instance.uiManager.DisableOperation();
        buildBtn.SetActive(true);
        switchBtn.SetActive(true);

        RectTransform buildBtnTF = buildBtn.transform as RectTransform;
        DOTween.To(() => buildBtnTF.anchoredPosition, p => buildBtnTF.anchoredPosition = p, buildBtnOriginalValue, 1).SetEase(Ease.InQuint); ;
        //DOTween.To(() => buildBtnTF.localScale, x => buildBtnTF.localScale = x, new Vector3(1.5f, 1.5f, 1), 1);

        RectTransform switchBtnTF = switchBtn.transform as RectTransform;
        DOTween.To(() => switchBtnTF.anchoredPosition, p => switchBtnTF.anchoredPosition = p, switchOriginalValue, 1);

        RectTransform buildPanelTF = panel.transform as RectTransform;
        DOTween.To(() => buildPanelTF.anchoredPosition, x => buildPanelTF.anchoredPosition = x,panelLocalOriginalValue, 1).SetEase(Ease.InQuint);
        DOTween.To(() => buildPanelTF.localScale, x => buildPanelTF.localScale = x, Vector3.one, 1).onComplete = () => {

            GameMainManager.instance.uiManager.EnableOperation();
        };
    }

    /// <summary>
    /// 窗口关闭时调用
    /// </summary>
    public void ClosePanel(System.Action onComplate)
    {
        RectTransform buildBtnTF = buildBtn.transform as RectTransform;
        RectTransform switchBtnTF = switchBtn.transform as RectTransform;
        RectTransform buildPanelTF = panel.transform as RectTransform;

        Sequence sq = DOTween.Sequence();
        sq.Append(DOTween.To(() => buildBtnTF.anchoredPosition, p => buildBtnTF.anchoredPosition = p, new Vector2(buildBtnTF.anchoredPosition.x, -300), 1).SetEase(Ease.OutExpo));
        sq.Insert(0.5f,DOTween.To(() => switchBtnTF.anchoredPosition, p => switchBtnTF.anchoredPosition = p, new Vector2(-200, switchBtnTF.anchoredPosition.y), 1).SetEase(Ease.OutCubic));
        sq.Insert(0.5f, DOTween.To(() => buildPanelTF.anchoredPosition, x => buildPanelTF.anchoredPosition = x, new Vector2(500, -150), 1).SetEase(Ease.OutBack));
        sq.onComplete += () => {
            buildBtn.SetActive(false);
            switchBtn.SetActive(false);
            panel.SetActive(false);
            onComplate();
        };
        

    }

    /// <summary>
    /// 窗口打开时的初始化
    /// </summary>
    public void OpenPanel(System.Action onComplate)
    {
        GameMainManager.instance.uiManager.DisableOperation();
        buildBtn.SetActive(false);
        switchBtn.SetActive(false);
        panel.SetActive(true);

        RectTransform buildBtnTF = buildBtn.transform as RectTransform;
        RectTransform switchBtnTF = switchBtn.transform as RectTransform;
        RectTransform buildPanelTF = panel.transform as RectTransform;

        buildBtnTF.anchoredPosition = new Vector2(buildBtnTF.anchoredPosition.x, -300);
        switchBtnTF.anchoredPosition = new Vector2(-200, switchBtnTF.anchoredPosition.y);
        buildPanelTF.anchoredPosition = new Vector2(400, -150);
        buildPanelTF.localScale = new Vector3(0.5f, 0.5f, 1);

        DOTween.To(() => buildPanelTF.anchoredPosition, x => buildPanelTF.anchoredPosition = x, new Vector2(200, -150), 1).SetEase(Ease.OutCubic).onComplete+=()=> {
            GameMainManager.instance.uiManager.EnableOperation();
            onComplate();
        };

    }

    public void onClickBuildBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIBuildingWindow);
    }

    public void OnClickNextIsland()
    {
        UpgradeIsland();
       
    }


    private void OnBuildComplateHandle(BaseEvent e)
    {
        BuildComplateEvent evt = e as BuildComplateEvent;
        buildComplateData = evt;
        if (!evt.isUpgrade)
        {
            StartCoroutine(showBuildAnimation(evt.buildIndex, evt.level));
        }else
        {
            StartCoroutine(showBuildAnimation(evt.buildIndex, GameMainManager.instance.model.userData.buildings[evt.buildIndex].level+1,()=> 
            {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_level_up);
                GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UISideBarWindow);
                GameMainManager.instance.uiManager.CloseWindow(UISettings.UIWindowID.UITopBarWindow);
                switchBtn.SetActive(false);
                upgradePanel.SetActive(true);
            }));
           
            buildBtn.SetActive(false);

        }
       
    }

    private void UpgradeIsland()
    {
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_island_change);
        UserData ud = GameMainManager.instance.model.userData;
        upgradePanel.SetActive(false);
        Sequence sq = DOTween.Sequence();
        sq.Append((panel.transform as RectTransform).DOAnchorPos(new Vector2(-800, 0), 1f).SetEase(Ease.InBack));
        sq.AppendCallback(() =>
        {
            islandFactory.UpdateCityData(ud.islandId, ud.buildings);
            (panel.transform as RectTransform).anchoredPosition = new Vector2(800, 0);
        });
        sq.Insert(1.2f,(panel.transform as RectTransform).DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBack));
        sq.AppendInterval(1);
        sq.AppendCallback(() =>
        {
            box.gameObject.SetActive(true);
            box.SetData(buildComplateData.upgradeEnergyReward > 0, buildComplateData.upgradeMoneyReward > 0);
            string content = string.Format("哇哦 恭喜您又建成一个岛屿，送你奖励继续加油哦！能量：{0} 金币：{1}", buildComplateData.upgradeEnergyReward.ToString(), buildComplateData.upgradeMoneyReward.ToString());
            GameMainManager.instance.uiManager.OpenModalBoxWindow(content, "领取", () =>
            {
                upgradePanel.SetActive(false);
                box.gameObject.SetActive(false);
                buildBtn.SetActive(true);

                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UISideBarWindow);
                GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UITopBarWindow);
                switchBtn.SetActive(true);
            });
        });
    }

    private IEnumerator showBuildAnimation(int index,int level,System.Action onComplate = null)
    {
        yield return new WaitForSeconds(0.5f);
        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.building_upgrade);
        buildingAnimation.transform.position = islandFactory.getBuildTransform(index).position;
        islandFactory.HideBuild(index);
        buildingAnimation.SetActive(true);
        yield return new WaitForSeconds(1);
        buildingAnimation.SetActive(false);
        BuildingData bd = new BuildingData();
        bd.level = level;
        bd.status = 0;
        islandFactory.UpdateBuildingData(islandID, index, bd);
        islandFactory.ShowBuild(index);
        if(onComplate != null)
        {
            yield return new WaitForSeconds(1);
            onComplate();
        }
    }
}
