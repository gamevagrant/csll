using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace QY.UI
{
    public class Toggle : Interactable, IPointerClickHandler
    {
        [System.Serializable]
        public class ToggleEvent : UnityEvent<bool> { };

        public ToggleEvent onValueChanged ;
        
        public bool isSelectedInStart = false;

        public GameObject[] activate;//选中时需要开启的
        public GameObject[] deactivate;//没有选中时需要关闭的

        public ToggleGroup toggleGroup;
        private bool _isOn;
        public bool isOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                if (toggleGroup != null)
                {
                    if(value)
                    {
                        toggleGroup.NotifyToggleOn(this);
                    }else
                    {
                        toggleGroup.SetAllTogglesOff();
                    }
                    
                }else
                {
                    SetSelected(value);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
           
            if (toggleGroup != null)
            {
                toggleGroup.RegisterToggle(this);
            }
        }

        

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (toggleGroup != null)
            {
                toggleGroup.UnregisterToggle(this);
            }
        }

        protected override void Start()
        {
            base.Start();
            _isOn = isSelectedInStart;
            SetState(isSelectedInStart);
        }

        internal void SetSelected(bool isSelected)
        {
            if(isSelected == _isOn)
            {
                return;
            }
            _isOn = isSelected;
            SetState(isSelected);
            StartCoroutine(ChangeValue(isSelected));
        }

        IEnumerator ChangeValue(bool isSelected)
        {
            yield return null;
            onValueChanged.Invoke(isSelected);
        }

        private void SetState(bool isSelected)
        {
            if(!Application.isPlaying)
            {
                return;
            }
            if (activate != null)
            {
                foreach (GameObject go in activate)
                {
                    if (go != null)
                    {
                        go.SetActive(isSelected);
                    }

                }

            }

            if (deactivate != null)
            {
                foreach (GameObject go in deactivate)
                {
                    if (go != null)
                    {
                        go.SetActive(!isSelected);
                    }

                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsInteractable())
            {
                return;
            }
            GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
            if(isInteractive)
            {
                Interacted();
                isOn = !isOn;
            }
           
            
        }

    }
}


