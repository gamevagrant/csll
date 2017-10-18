﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class UIBuildingWindow : UIWindowBase {

    BuildingWindowItem[] items;
    SpriteAtlas spriteAtlas;
    int islandID;
    UserData userData;

    public override UIWindowData windowData
    {
        get
        {
            if(_windowData==null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIBuildingWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    private void Awake()
    {
        items = GetComponentsInChildren<BuildingWindowItem>();
        foreach (BuildingWindowItem item in items)
        {
            item.onBuild += (index) => {
                GameMainManager.instance.netManager.Build(userData.uid, userData.islandId, index - 1, (ret, data) => {
                    if (ret && data.isOK)
                    {
                        BuildComplateEvent evt = new BuildComplateEvent();
                        evt.buildIndex = index;
                        evt.level = data.data.buildings[index - 1].level;
                        EventDispatcher.instance.DispatchEvent(evt);
                        Debug.Log("DispatchEvent");
                        GameMainManager.instance.uiManager.CloseWindow(windowData.id);
                        
                    }
                });
            };
        }
    }

    private void Start()
    {
        
    }

    protected override void startShowWindow()
    {
        userData = GameMainManager.instance.model.userData;
        if (spriteAtlas!=null && userData.islandId == islandID)
        {
            updateData(spriteAtlas);
        }
        else
        {
            islandID = userData.islandId;
            string name = "CityThumbnail_" + islandID.ToString();
            string path = FilePathTools.getSpriteAtlasPath(name);
            AssetBundleLoadManager.Instance.LoadAsset<SpriteAtlas>(path,(sa)=> {

                spriteAtlas = sa;
                updateData(sa);
            });
        }
    }

    private void updateData(SpriteAtlas sa )
    {
        foreach(BuildingWindowItem item in items)
        {
            item.setData(userData.buildings, sa,userData.buildingCost,userData.buildingRepairCost);
           
        }
    }


}