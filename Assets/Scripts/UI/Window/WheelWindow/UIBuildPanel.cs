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
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.BUILD_COMPLATE, OnBuildComplateHandle);
    }

    public void setData(int islandID,BuildingData[] data)
    {
        this.islandID = islandID;
        islandFactory.UpdateCityData(islandID, data);


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
        DOTween.To(() => buildPanelTF.anchoredPosition, x => buildPanelTF.anchoredPosition = x,panelLocalOriginalValue, 1);
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
        sq.Insert(0.5f, DOTween.To(() => buildPanelTF.anchoredPosition, x => buildPanelTF.anchoredPosition = x, new Vector2(500, -150), 1).SetEase(Ease.OutCubic));
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



    private void OnBuildComplateHandle(BaseEvent e)
    {
        Debug.Log("get");
        BuildComplateEvent evt = e as BuildComplateEvent;

        StartCoroutine(showBuildAnimation(evt.buildIndex,evt.level));
    }


    private IEnumerator showBuildAnimation(int index,int level)
    {

        buildingAnimation.transform.position = islandFactory.getBuildTransform(index).position;
        islandFactory.HideBuild(index);
        buildingAnimation.SetActive(true);
        yield return new WaitForSeconds(3);
        buildingAnimation.SetActive(false);
        BuildingData bd = new BuildingData();
        bd.level = level;
        bd.status = 0;
        islandFactory.UpdateBuildingData(islandID, index, bd);
        islandFactory.ShowBuild(index);
    }
}
