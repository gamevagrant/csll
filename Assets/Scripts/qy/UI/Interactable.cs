using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QY.Guide;

namespace QY.UI
{
    public class Interactable : UnityEngine.UI.Selectable
    {
        private static int lockNum = 0;
        public static bool isLock
        {
            get
            {
                return lockNum > 0;
            }
            set
            {
                
                lockNum+=value?1:-1;
                //UnityEngine.Debug.Log("--Lock--" + value + " locNum:" + lockNum);
            }
        }
        private static Dictionary<string, string> ignoreList = new Dictionary<string, string>();

        public string id = "";

        /// <summary>
        /// 是否接受 全局锁定 新手引导的控制
        /// </summary>
        private bool enableController
        {
            get
            {
                return !string.IsNullOrEmpty(id);
            }
        }

        /// <summary>
        /// 是否应该响应交互操作 进行全局锁定后 并且忽略列表中没有自己的id 的时候就不能响应操作
        /// </summary>
        protected bool isInteractive
        {
            get
            {
                return !Interactable.isLock ||(lockNum<=1 && ignoreList.ContainsKey(id));

            }
        }

        /// <summary>
        /// 是否忽略全局锁定
        /// </summary>
        public bool isIgnoreLock
        {
            get
            {
                if (enableController)
                {
                    return ignoreList.ContainsKey(id);
                }
                else
                {
                    return false;
                }

            }
            set
            {
                if (enableController)
                {
                    if (value && !ignoreList.ContainsKey(id))
                    {
                        ignoreList.Add(id, id);
                    }
                    else if (!value && ignoreList.ContainsKey(id))
                    {
                        ignoreList.Remove(id);
                    }
                }
            }
        }

        protected override void OnEnable()
        {
            if(Application.isPlaying)
            {
                GuideManager.instance.TryProcess();
            }
           
        }

        protected override void Start()
        {
            base.Start();
            if(Application.isPlaying && enableController && GuideManager.instance.enable)
            {
                GuideManager.instance.RegisterInteractable(id,this);
            }
           
        }

        /// <summary>
        /// 发生交互行为
        /// </summary>
        protected void Interacted()
        {
            if (enableController && GuideManager.instance.enable && isInteractive)
            {
                GuideManager.instance.Executed(this);
            }
               
        }

    }
}

