using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.U2D;

public class IslandFactory : MonoBehaviour {

    private Image[] citySprites;
    private const int SpriteCount = 6;
    private int[] sort = { 0, 1, 5, 4, 3, 2 };
    private Vector2[] positions = {
        new Vector2 (0,-66),
        new Vector2(65,-37),
        new Vector2(170,-147),
        new Vector2(-180,-15),
        new Vector2(-73,-28),
        new Vector2(158,-17),
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
       // string str = "[{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":5,\"status\":0,\"isShield\":true},{\"level\":4,\"status\":0,\"isShield\":true},{\"level\":3,\"status\":0,\"isShield\":true}]";
       // BuildingData[] data = LitJson.JsonMapper.ToObject<BuildingData[]>(str);
       // UpdateCityData(1,data);
    }
	
    private void init()
    {
        citySprites = new Image[SpriteCount];
        for(int i = 0;i< SpriteCount; i++)
        {
            int index = sort[i];
            GameObject go = GameUtils.createGameObject(gameObject, index.ToString());
            go.AddComponent<RectTransform>();
            citySprites[index] = go.AddComponent<Image>();
            GameObject goTag = GameUtils.createGameObject(go, "tag");
            goTag.AddComponent<RectTransform>().anchoredPosition = new Vector3(positions[index].x, positions[index].y, 0);

        }
    }

    public void UpdateCityData(int islandID, BuildingData[] data)
    {
        if(islandID > 3)
        {
            islandID = 3;
        }
        if(islandID == this.islandID)
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

    public void UpdateBuildingData(int islandID,int index,BuildingData data)
    {
        if (islandID == this.islandID)
        {
            UpdateCitySprite(spriteAtlas, index, data);
        }
        else
        {
            string path = FilePathTools.getSpriteAtlasPath("City_" + islandID.ToString());
            AssetBundleLoadManager.Instance.LoadAsset<SpriteAtlas>(path, (sa) => {

                this.islandID = islandID;
                spriteAtlas = sa;

                UpdateCitySprite(sa,index, data);
            });
        }
    }

    public void HideBuild(int index)
    {
        citySprites[index].enabled = false;
    }

    public void ShowBuild(int index)
    {
        citySprites[index].enabled = true;
    }

    public RectTransform getBuildTransform(int index)
    {
        return citySprites[index].transform.GetChild(0) as RectTransform;
    }

    private void UpdateAllSprite(SpriteAtlas sa, BuildingData[] data)
    {
        
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
    }

    private void SetCityItemSprite(int index, Sprite sprite)
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
        }
    }
}
