using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.EventSystems;
using System;

public class BuildingWindowItem : MonoBehaviour,IPointerClickHandler {

    public Text price;
    public Image repair;
    public Image upgraded;
    public Action<int> onBuild;

    private Image icon;

    public int index;//建筑类型索引 从1开始
    public int level;//此格子显示的建筑等级

    private BuildState state = BuildState.Null;

    private enum BuildState
    {
        Null,
        cantBuild,//不能建造
        canBuild,//可以建造
        upgraded,//已经升级过的
        damage,//损坏的
    }

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(state == BuildState.canBuild)
        {
            if(onBuild!=null)
            {
                onBuild(index);
            }
           
        }
    }

    public void setData(BuildingData[] bds,SpriteAtlas sa,int[][] buildingCost,int[][] repairCost)
    {
        if(index <= bds.Length)
        {
            BuildingData bd = bds[index - 1];
            string name = string.Format("thumb{0}_{1}_{2}@2x", level > bd.level + 1 ? "_gray" : "", index.ToString(), level.ToString());
            Sprite sp = sa.GetSprite(name);
            if (sp != null)
            {
                icon.sprite = sp;
                icon.SetNativeSize();
            }
            if(bd.status != 0)
            {
                price.text = repairCost[index-1][level-1].ToString();
            }else
            {
                price.text = buildingCost[index - 1][level - 1].ToString();
            }

            if (bd.status != 0)
            {
                changeState(BuildState.damage);
            }
            else if (level == bd.level + 1)
            {
                changeState(BuildState.canBuild);
            }
            else if (level > bd.level)
            {
                changeState(BuildState.cantBuild);
            }
            else
            {
                changeState(BuildState.upgraded);
            }
        }
       
    }

    private void changeState(BuildState state)
    {
        switch (state)
        {
            case BuildState.cantBuild:
                price.color = Color.black;
                price.gameObject.SetActive(true);
                repair.gameObject.SetActive(false);
                upgraded.gameObject.SetActive(false);
                this.state = BuildState.cantBuild;
                break;
            case BuildState.canBuild:
                price.color = Color.yellow;
                price.gameObject.SetActive(true);
                repair.gameObject.SetActive(false);
                upgraded.gameObject.SetActive(false);
                this.state = BuildState.canBuild;
                break;
            case BuildState.upgraded:
                price.gameObject.SetActive(false);
                repair.gameObject.SetActive(false);
                upgraded.gameObject.SetActive(true);
                this.state = BuildState.upgraded;
                break;
            case BuildState.damage:
                price.color = Color.yellow;
                price.gameObject.SetActive(true);
                repair.gameObject.SetActive(true);
                upgraded.gameObject.SetActive(false);
                this.state = BuildState.damage;
                break;
        }

    }


}
