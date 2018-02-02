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
    [System.Serializable]
    public class ButtonDown : UnityEvent { };
    public LongPressSelectEvent onHoldOn;
    public ButtonDown onButtonDown;
    [SerializeField]
    private Sprite[] sprites;//0:弹起状态 1：长按状态 2按下状态

    private const float HOLD_ON_TIME = 1f;
    private float downTag = 0;
    private bool _isHoldOn = false;//前一次点击是否时长按
    private bool isHoldOn
    {
        get
        {
            return _isHoldOn;
        }
        set
        {
            _isHoldOn = value;
            image.sprite = sprites[_isHoldOn ? 1:0];
            SpriteState ss = new SpriteState()
            {
                pressedSprite = sprites[_isHoldOn ? 1:2]
            };
            this.spriteState = ss;
        }
    }

    private void Update()
    {
        if (Application.isPlaying && !GameMainManager.instance.model.userData.isTutorialing && downTag > 0 && Time.time - downTag > HOLD_ON_TIME)
        {
            downTag = 0;
            isHoldOn = true;
            onHoldOn.Invoke(true);
           

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

        downTag = Time.time;

        if (onButtonDown != null)
            onButtonDown.Invoke();
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
