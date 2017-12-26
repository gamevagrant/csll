using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using QY.UI;

public class GestureTrigger : Interactable, IDragHandler,IBeginDragHandler,IEndDragHandler{
    [System.Serializable]
    public class GestureEvent : UnityEvent { };

    public GestureEvent touchLeft;
    public GestureEvent touchRight;
    public GestureEvent touchUp;
    public GestureEvent touchDown;

    private Vector2 start;
    private Vector2 end;

    public void OnBeginDrag(PointerEventData eventData)
    {
        start = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(isInteractive)
        {
            end = eventData.position;

            Process();
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }

    private void Process()
    {
        Vector2 move = end - start;
        if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
        {
            if (move.x > 0)
            {
                touchRight.Invoke();

            }
            else
            {
                touchLeft.Invoke();
            }
        }
        else
        {
            if (move.y > 0)
            {
                touchUp.Invoke();
            }
            else
            {
                touchDown.Invoke();
            }
        }
    }

}
