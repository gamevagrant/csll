﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDot : MonoBehaviour {

    //不要更改枚举顺序
    public enum RedDotType
    {
        NULL,
        EveryEnergy,
        EveryLogin,
        EveryTask,
        Map,
        Building,
        Invite,
        Recall,
        Message,
        Friend,
        LeftDatail,
        InviteAndRecall,
    }

    [SerializeField]
    private Image  redDot;
    [SerializeField]
    private RedDotType type;

    public bool show
    {
        get
        {
            return redDot.enabled;
        }
        set
        {
            redDot.enabled = value;
        }
    }

    public void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        bool isShow = false;
        switch (type)
        {
            case RedDotType.LeftDatail:
                UserData user = GameMainManager.instance.model.userData;
                int v = user.mapTip + user.buildingTip + user.friendTip + user.mailTip + user.invitTip + user.recallTip;
                isShow = v > 0 ? true : false;
                break;
            case RedDotType.Map:
                isShow = GameMainManager.instance.model.userData.mapTip>0?true:false;
                break;
            case RedDotType.Building:
                isShow = GameMainManager.instance.model.userData.buildingTip > 0 ? true : false;
                break;
            case RedDotType.Friend:
                isShow = GameMainManager.instance.model.userData.friendTip > 0 ? true : false;
                break;
            case RedDotType.Message:
                isShow = GameMainManager.instance.model.userData.mailTip > 0 ? true : false;
                break;
            case RedDotType.Invite:
                isShow = GameMainManager.instance.model.userData.invitTip > 0 ? true : false;
                break;
            case RedDotType.Recall:
                isShow = GameMainManager.instance.model.userData.recallTip > 0 ? true : false;
                break;
            case RedDotType.InviteAndRecall:
                isShow = GameMainManager.instance.model.userData.invitTip + GameMainManager.instance.model.userData.recallTip > 0 ? true : false;
                break;
        }
        show = isShow;
    }
}
