using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.U2D;

public class IslandFactory : MonoBehaviour {

    private Image[] citySprites;
    private GameObject[] effects;
    private Image[] ruins;
    private const int SpriteCount = 6;
    private int[] sort = { 0, 1, 5, 4, 3, 2 };
    private Vector2[] positions = {
        new Vector2 (0,-66),
        new Vector2(65,-15),
        new Vector2(170,-147),
        new Vector2(-180,-15),
        new Vector2(-73,-52),
        new Vector2(160,-60),
    };

    private SpriteAtlas spriteAtlas;
    private int islandID;

    void Awake()
    {
        init();
    }
	// Use this for initialization
	void Start () {
        //假数据
        //string str = "[{\"level\":5,\"status\":1,\"isShield\":true},{\"level\":5,\"status\":1,\"isShield\":true},{\"level\":5,\"status\":1,\"isShield\":true},{\"level\":4,\"status\":1,\"isShield\":true},{\"level\":3,\"status\":1,\"isShield\":true}]";
        //BuildingData[] data = LitJson.JsonMapper.ToObject<BuildingData[]>(str);
        //UpdateCityData(1,data);
    }

    private void OnEnable()
    {
        if(Application.isPlaying)
        {
            foreach (GameObject go in effects)
            {
                if (go != null)
                {
                    go.SetActive(false);
                }
            }
        }
    }

    private void init()
    {
        citySprites = new Image[SpriteCount];

        effects = new GameObject[SpriteCount];
        ruins = new Image[SpriteCount];
        for (int i = 0;i< SpriteCount; i++)
        {
            int index = sort[i];
            GameObject go = GameUtils.createGameObject(gameObject, index.ToString());
            go.AddComponent<RectTransform>();
            citySprites[index] = go.AddComponent<Image>();
            citySprites[index].raycastTarget = false;
            GameObject goTag = GameUtils.createGameObject(go, "tag");
            goTag.AddComponent<RectTransform>().anchoredPosition = new Vector3(positions[index].x, positions[index].y, 0);

           
        }
    }

    public void UpdateCityData(int islandID, BuildingData[] data)
    {
        Debug.Log("island is "+islandID);
        if(islandID> GameMainManager.instance.configManager.islandConfig.islandNames.Length)
        {
            islandID = islandID % (GameMainManager.instance.configManager.islandConfig.islandNames.Length + 1) + 1;
        }
        
        if (islandID == this.islandID)
        {
            UpdateAllSprite(spriteAtlas, data);
        }
        else
        {
            string path = FilePathTools.getSpriteAtlasPath("City_" + islandID.ToString());
            AssetBundleLoadManager.Instance.LoadAsset<SpriteAtlas>(path, (sa) => {

                this.islandID = islandID;
                spriteAtlas = sa;

                UpdateAllSprite(sa,data);
            });
        }

    }

    public void UpdateBuildingData(int index,BuildingData data)
    {
        UpdateCitySprite(spriteAtlas, index, data);
    }

    public void HideBuild(int index)
    {
        citySprites[index].enabled = false;
    }

    public void ShowBuild(int index)
    {
        citySprites[index].enabled = true;
    }

    public RectTransform GetBuildTransform(int index)
    {
        return citySprites[index].transform.GetChild(0) as RectTransform;
    }

    private void UpdateAllSprite(SpriteAtlas sa, BuildingData[] data)
    {
        if(citySprites == null)
        {
            return;
        }
        
        string isLandName = "island_city";
        Sprite isLandSprite = sa.GetSprite(isLandName);
        SetCityItemSprite(0, isLandSprite);


        for (int i = 0; i < data.Length; i++)
        {
            BuildingData bd = data[i];
            if (i + 1 < citySprites.Length)
            {

                UpdateCitySprite(sa, i + 1, bd);
            }


        }
    }

    private void UpdateCitySprite(SpriteAtlas sa, int index, BuildingData bd)
    {

        string name = string.Format("city_{0}_{1}_{2}", index.ToString(), bd.level.ToString(), bd.status.ToString());
        Sprite sp = sa.GetSprite(name);
        SetCityItemSprite(index, sp);
        SetBuildingState(index, bd.status,sa.GetSprite("ruins"));
    }

    private void SetCityItemSprite(int index, Sprite sprite)
    {
        if (citySprites!= null && index < citySprites.Length)
        {
            if (sprite == null)
            {
                citySprites[index].enabled = false;
            }
            else
            {

                citySprites[index].enabled = true;
                citySprites[index].sprite = sprite;
                citySprites[index].SetNativeSize();

                if (index == 2)//交通工具
                {
                    if(GetComponent<BreathingAnimation>() == null)
                    {
                        BreathingAnimation breathing = citySprites[index].gameObject.AddComponent<BreathingAnimation>();
                        breathing.sizeStrength = Vector2.zero;
                        breathing.posStrengh = new Vector2(0, 8);
                        breathing.speed = 1f;
                    }
                   
                }
            }
        }
        
    }

    private void SetBuildingState(int index, int state, Sprite ruinsSprite)
    {
        if(index == 2 || index == 3)//排除动物和载具
        {
            return;
        }
        //状态值 0正常 1损坏 2摧毁
        if (effects != null && ruins != null && index < citySprites.Length)
        {
            if (effects[index] != null)
            {
                effects[index].SetActive(false);

            }
            if (ruins[index] != null)
            {
                ruins[index].gameObject.SetActive(false);
            }

            if (state == 1)
            {
                if(effects[index]!=null)
                {
                    effects[index].SetActive(true);

                }else
                {
                    string path = FilePathTools.getEffectPath("SmokeLight");
                    AssetBundleLoadManager.Instance.LoadAsset<GameObject>(path, (go) => {

                        GameObject effect = GameUtils.createGameObject(citySprites[index].transform.GetChild(0).gameObject, go);
                        Quaternion q = Quaternion.identity;
                        q.eulerAngles = new Vector3(-90, 0, 0);
                        effect.transform.localRotation = q;
                        effect.transform.localPosition = new Vector3(0, 50,-100);
                        effects[index] = effect;
                    });
                }
            }
            else if (state == 2)
            {
                if(ruins[index]!=null)
                {
                    ruins[index].gameObject.SetActive(true);
                }else
                {
                    GameObject ruinsGo = GameUtils.createGameObject(citySprites[index].transform.GetChild(0).gameObject, "ruins");
                    ruins[index] = ruinsGo.AddComponent<Image>();
                    ruins[index].sprite = ruinsSprite;
                    ruins[index].SetNativeSize();
                }
            }else
            {
                
            }
        }
            
        
    }
}
