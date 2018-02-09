using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIDungeonRewardIcon : Selectable, IPointerDownHandler,IPointerUpHandler,IDragHandler,IDropHandler
{
    public Action<int,bool> onPress;

    [SerializeField]
    private int index;

    public void OnDrag(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if(onPress!=null)
        {
            onPress(index,true);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (onPress != null)
        {
            onPress(index, false);
        }
    }


}
