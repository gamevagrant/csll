using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace QY.UI
{
    public class Button :Interactable,IPointerClickHandler,IPointerDownHandler
    {
        [System.Serializable]
        public class ButtonClickEvent : UnityEvent { };

        public ButtonClickEvent onClick;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if(!IsInteractable())
            {
                return;
            }
            
            base.OnPointerDown(eventData);
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
            if (isInteractive)
            {
                Interacted();
                onClick.Invoke();
                
            }
        }
    }



}

