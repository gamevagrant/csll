using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QY.UI
{
    public class ToggleGroup : MonoBehaviour
    {
        public bool allowSwitchOff = false;

        private Dictionary<Toggle,string> dic = new Dictionary<Toggle, string>();


        public void RegisterToggle(Toggle toggle)
        {
            if(!dic.ContainsKey(toggle))
            {
                dic.Add(toggle,"");
            }
        }

        public void UnregisterToggle(Toggle toggle)
        {
            if (dic.ContainsKey(toggle))
            {
                dic.Remove(toggle);
            }
        }

        public void NotifyToggleOn(Toggle toggle)
        {
            foreach(Toggle t in dic.Keys)
            {
                t.SetSelected(t == toggle);
            }
        }

        public void SetAllTogglesOff()
        {
            if(allowSwitchOff)
            {
                foreach (Toggle t in dic.Keys)
                {
                    t.SetSelected(false);
                }
            }
            
        }
    }
}

