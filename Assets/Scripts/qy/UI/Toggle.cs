using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace QY.UI
{
    public class Toggle : UnityEngine.UI.Selectable, IPointerClickHandler
    {
        public UnityEngine.UI.Toggle.ToggleEvent onValueChanged;

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
                _isOn = value;

                if (toggleGroup != null)
                {
                    toggleGroup.NotifyToggleOn(this);
                }else
                {
                    SetSelected(_isOn);
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
            if (isSelectedInStart)
            {
                isOn = true;
               
            }
        }

        internal void SetSelected(bool isSelected)
        {
            if(activate != null)
            {
                foreach (GameObject go in activate)
                {
                    if(go != null)
                    {
                        go.SetActive(isSelected);
                    }
                    
                }
               
            }

            if(deactivate!=null)
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
            isOn = true;
            int count = onValueChanged.GetPersistentEventCount();
            if (count > 0)
            {
                for(int i = 0;i<count;i++)
                {
                    (onValueChanged.GetPersistentTarget(i) as GameObject).SendMessage(onValueChanged.GetPersistentMethodName(i), true);
                }
               
            }
        }

    }
}


