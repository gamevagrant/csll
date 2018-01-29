using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace QY.UI
{
    public class Button :Interactable,IPointerClickHandler
    {
        [System.Serializable]
        public class ButtonClickEvent : UnityEvent { };

        [SerializeField]
        internal ButtonClickEvent onClick;


        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!IsInteractable())
            {
                return;
            }

            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
            if (isInteractive)
            {
                Interacted();
                onClick.Invoke();

            }
        }




    }



}

