using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWheelRollButton : QY.UI.Button
{

    [System.Serializable]
    public class LongPressSelectEvent : UnityEvent<bool> { };
    public LongPressSelectEvent onHoldOn;
    [SerializeField]
    private Sprite[] sprites;

    private const float HOLD_ON_TIME = 2;
    private float downTag = 0;
    private bool isHoldOn = false;//前一次点击是否时长按

    private void Update()
    {
        if (downTag > 0 && Time.time - downTag > HOLD_ON_TIME)
        {
            downTag = 0;
            if (isInteractive)
            {
                isHoldOn = true;
                onHoldOn.Invoke(true);
                image.sprite = sprites[1];
            }
           
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!IsInteractable())
        {
            return;
        }
        base.OnPointerDown(eventData);
        if (!IsInteractable())
        {
            return;
        }

        downTag = Time.time;
        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!IsInteractable())
        {
            return;
        }
        base.OnPointerUp(eventData);
        if (Time.time - downTag < HOLD_ON_TIME)
        {
            if(isHoldOn)
            {
                onHoldOn.Invoke(false);
                isHoldOn = false;
                image.sprite = sprites[0];
            }
            else
            {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
                if (isInteractive)
                {
                    Interacted();
                    onClick.Invoke();

                }
            }
        }

        downTag = 0;
    }
}
