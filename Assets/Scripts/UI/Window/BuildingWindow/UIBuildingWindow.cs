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
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
            }
            return _windowData;
        }
    }


    private void Start()
    {
        items = GetComponentsInChildren<BuildingWindowItem>();
        foreach (BuildingWindowItem item in items)
        {
            item.onBuild += (index) => {
                GameMainManager.instance.uiManager.CloseWindow(windowData.id);
                GameMainManager.instance.netManager.Build(userData.islandId, index - 1, (ret, data) => {
                    if (ret && data.isOK)
                    {
                        BuildComplateEvent evt = new BuildComplateEvent();
                        evt.buildIndex = index;
                        evt.level = data.data.buildings[index - 1].level;
                        evt.status = data.data.buildings[index - 1].status;
                        evt.isRepair = userData.buildings[index - 1].status == 1;
                        evt.islandID = data.data.islandId;
                        evt.isUpgrade = data.data.playUpgradeAnimation;
                        evt.upgradeEnergyReward = data.data.upgradeEnergyAfterReward - data.data.energy;
                        evt.upgradeMoneyReward = data.data.upgradeMoneyAfterReward - data.data.money;
                        EventDispatcher.instance.DispatchEvent(evt);


                    }

                });


            };
        }
    }

    protected override void StartShowWindow(object[] data)
    {
        userData = GameMainManager.instance.model.userData;
        if (spriteAtlas!=null && userData.islandId == islandID)
        {
            updateData(spriteAtlas);
        }
        else
        {
            islandID = userData.islandId;
            if (islandID > GameMainManager.instance.configManager.islandConfig.islandNames.Length)
            {
                islandID = islandID % (GameMainManager.instance.configManager.islandConfig.islandNames.Length + 1) + 1;
            }
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
