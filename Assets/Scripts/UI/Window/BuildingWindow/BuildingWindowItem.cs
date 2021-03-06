﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class BuildingWindowItem : QY.UI.Interactable,IPointerClickHandler {

    public Image icon;
    public Image repair;
    public Image upgraded;
    public TextMeshProUGUI price;
    public Action<int> onBuild;

    public int index;//建筑类型索引 从1开始
    public int level;//此格子显示的建筑等级

    private BuildState state = BuildState.Null;
    private int needMoney;

    private enum BuildState
    {
        Null,
        cantBuild,//不能建造
        canBuild,//可以建造
        upgraded,//已经升级过的
        damage,//损坏的
    }

    protected override void Awake()
    {
        base.Awake();

        icon.gameObject.SetActive(false);
        repair.gameObject.SetActive(false);
        upgraded.gameObject.SetActive(false);
        price.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsInteractable())
        {
            return;
        }
        

        GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
        if (isInteractive)
        {
            if (GameMainManager.instance.model.userData.money < needMoney)
            {
                Alert.Show("金币不足！");
                return;
            }
            Interacted();
            if (state == BuildState.canBuild || state == BuildState.damage)
            {
                
                if (onBuild != null)
                {
                    onBuild(index);
                }

            }
        }
        
    }

    public void setData(BuildingData[] bds,SpriteAtlas sa,int[][] buildingCost,int[][] repairCost)
    {
        icon.gameObject.SetActive(true);
        if (index <= bds.Length)
        {
            BuildingData bd = bds[index - 1];
            string name = string.Format("thumb{0}_{1}_{2}@2x", level > bd.level + 1 ? "_gray" : "", index.ToString(), level.ToString());
            Sprite sp = sa.GetSprite(name);
            if (sp != null)
            {
                icon.sprite = sp;
                icon.SetNativeSize();
            }

            needMoney = 0;
            if(bd.status != 0)
            {
                needMoney = repairCost[index - 1][level - 1];
                
            }else
            {
                needMoney = buildingCost[index - 1][level - 1];
            }
            price.text = GameUtils.GetCurrencyString(needMoney);

            if (level == bd.level && bd.status != 0)
            {
                if (GameMainManager.instance.model.userData.money >= needMoney)
                {
                    price.color = Color.yellow;
                }
                else
                {
                    price.color = new Color(0.72f,0.49f,0,0.8f);
                }
                price.gameObject.SetActive(true);
                repair.gameObject.SetActive(true);
                upgraded.gameObject.SetActive(false);
                this.state = BuildState.damage;
            }
            else if (level == bd.level + 1)
            {
                if(GameMainManager.instance.model.userData.money >= needMoney)
                {
                    price.color = Color.yellow;
                }else
                {
                    price.color = new Color(0.72f, 0.49f, 0, 0.8f);
                }
               
                price.gameObject.SetActive(true);
                repair.gameObject.SetActive(false);
                upgraded.gameObject.SetActive(false);
                this.state = BuildState.canBuild;
            }
            else if (level > bd.level)
            {
                price.color = new Color(0, 0, 0, 0.4f);
                price.gameObject.SetActive(true);
                repair.gameObject.SetActive(false);
                upgraded.gameObject.SetActive(false);
                this.state = BuildState.cantBuild;
            }
            else
            {
                price.gameObject.SetActive(false);
                repair.gameObject.SetActive(false);
                upgraded.gameObject.SetActive(true);
                this.state = BuildState.upgraded;
            }
        }
       
    }




}
