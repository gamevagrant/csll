using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using DG.Tweening;

public class UIBuildPanel : MonoBehaviour {

    public Text cityName;
    public Image[] buildItem;
    public GameObject buildingAnimation;

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
        string path = FilePathTools.getSpriteAtlasPath("City_" + islandID.ToString());
        AssetBundleLoadManager.Instance.LoadAsset<SpriteAtlas>(path, (sa)=>{
            spriteAtlas = sa;
            string isLandName = "island_city";
            Sprite isLandSprite = spriteAtlas.GetSprite(isLandName);
            setCityItemSprite(0,isLandSprite);

            buildItem[0].sprite = isLandSprite;

            for (int i = 0;i<data.Length;i++)
            {
                BuildingData bd = data[i];
                if(i+1<buildItem.Length)
                {

                    updateCitySprite(spriteAtlas,i+1,bd.level,bd.status);
                }
               

            }
        });
        
    }



    public void hidePanel()
    {
        RectTransform buildBtnTF = buildBtn.transform as RectTransform;
        //DOTween.To(() => buildBtnTF.offsetMin, min => buildBtnTF.offsetMin = min, new Vector2(buildBtnTF.offsetMin.x ,- 500), 1
        DOTween.To(() => buildBtnTF.anchoredPosition, p => buildBtnTF.anchoredPosition = p, new Vector2(buildBtnTF.anchoredPosition.x, -300), 1).SetEase(Ease.OutQuint); ;
        //DOTween.To(() => buildBtnTF.localScale, x => buildBtnTF.localScale = x, new Vector3(1.5f, 1.5f, 1), 1);

        RectTransform switchBtnTF = switchBtn.transform as RectTransform;
        //DOTween.To(() => switchBtnTF.offsetMin, min => switchBtnTF.offsetMin = min, new Vector2(-300, switchBtnTF.offsetMin.y), 1);
        DOTween.To(() => switchBtnTF.anchoredPosition, p => switchBtnTF.anchoredPosition = p, new Vector2(-200, switchBtnTF.anchoredPosition.y), 1);

        RectTransform buildPanelTF = panel.transform as RectTransform;
        DOTween.To(() => buildPanelTF.anchoredPosition, x => buildPanelTF.anchoredPosition = x, new Vector2(200, -150), 1);
        DOTween.To(() => buildPanelTF.localScale, x => buildPanelTF.localScale = x, new Vector3(0.5f,0.5f,1), 1).onComplete = () => {


        };
    }

    public void showPanel()
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

    public void onClickBuildBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIBuildingWindow);
    }

    private void updateCitySprite(SpriteAtlas sa ,int index,int level,int status = 0)
    {
        string name = string.Format("city_{0}_{1}_{2}", index.ToString(), level.ToString(), status.ToString());
        Sprite sp = spriteAtlas.GetSprite(name);
        setCityItemSprite(index, sp);
    }

    private void setCityItemSprite(int index, Sprite sprite)
    {
        if (sprite == null)
        {
            buildItem[index].enabled = false;
        }
        else
        {
            buildItem[index].enabled = true;
            buildItem[index].sprite = sprite;
        }
    }

    private void OnBuildComplateHandle(BaseEvent e)
    {
        Debug.Log("get");
        BuildComplateEvent evt = e as BuildComplateEvent;

        StartCoroutine(showBuildAnimation(evt.buildIndex,evt.level));
    }


    private IEnumerator showBuildAnimation(int index,int level)
    {
        buildingAnimation.transform.position = buildItem[index].transform.GetChild(0).position;
        buildItem[index].gameObject.SetActive(false);
        buildingAnimation.SetActive(true);
        yield return new WaitForSeconds(3);
        buildingAnimation.SetActive(false);
        updateCitySprite(spriteAtlas, index, level,0);
        buildItem[index].gameObject.SetActive(true);
    }
}
