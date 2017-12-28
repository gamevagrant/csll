using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using QY.UI;

namespace QY.Guide
{
    public class GuideManager :MonoBehaviour
    {
        private static GuideManager _instance;
        public static GuideManager instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("GuideManager");
                    _instance = go.AddComponent<GuideManager>();
                }
                return _instance;
            }
        }

        public bool enable
        {
            get
            {
                if(config == null || curData == null)
                {
                    return false;
                }
                return true;
            }
        }

        private GuideDataConfig config;
        private Dictionary<string, Interactable> interactables;

        private Action<GuideData, Interactable> onProcess;
        private Action<GuideData> onExecutedComplate;
        private GuideData curData;
        private string _state;
        public string state
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                Process();
            }
        }


        public void Start()
        {
            interactables = new Dictionary<string, Interactable>();

        }

        private void OnDestroy()
        {
            _instance = null;
        }
        public void Init(int progress,GuideDataConfig config, Action<GuideData, Interactable> onProcess,Action<GuideData> onExecutedComplate)
        {
            this.config = config;
            this.onProcess = onProcess;
            this.onExecutedComplate = onExecutedComplate;
            //取得新手引导的配置文件转化成可用数据结构
            //判断是否开启新手引导(curData为null时就没有开启新手引导)
            //设置currData
            curData = config.SetProgress(progress);
           
            if (curData!=null)
            {
                curData.delay = 0;
                UnityEngine.Debug.Log("启用新手引导:"+ curData.progress.ToString());
            }
        }

        public void RegisterInteractable(string id, Interactable obj)
        {
            if (!enable)
            {
                return;
            }
            UnityEngine.Debug.Log("注册组件:"+id);
            if (!interactables.ContainsKey(id))
            {
                interactables.Add(id,obj);
            }else
            {
                UnityEngine.Debug.Log("新手指导，同一控件不能多次注册");
            }
            //每次有新的注册时也尝试执行新手引导，避免指引需求在交互物体注册前进行的问题
            Process();
        }

        /// <summary>
        /// 执行了点击或者滑动等可交互控件的行为
        /// </summary>
        /// <param name="id"></param>
        public void Executed(Interactable interactable)
        {
            if(!enable)
            {
                return;
            }
            UnityEngine.Debug.Log("执行交互:" + interactable.id);
            if (curData.interact_id == interactable.id)
            {
                onExecutedComplate(curData);
                Interactable.isLock = false;
                interactable.isIgnoreLock = false;

                curData = config.next;
                Process();
            }
        }
        /// <summary>
        /// 尝试执行
        /// </summary>
        public void TryProcess()
        {
            //UnityEngine.Debug.Log("TryProcess");
            Process();
        }

        private void Process()
        {
            if (!enable || curData == null || curData.isProcess || (!string.IsNullOrEmpty(curData.state) && curData.state!=state))
            {
                return;
            }

            Interactable interactable;
            interactables.TryGetValue(curData.interact_id, out interactable);

            if (curData.actionType == GuideData.ActionType.click)
            {
                if (interactable != null && interactable.gameObject.activeInHierarchy)
                {
                    UnityEngine.Debug.Log("Process click");
                    curData.isProcess = true;
                    StartCoroutine(StartAction(curData, interactable));
                }
            }
            else
            {
                curData.isProcess = true;
                StartCoroutine(StartAction(curData, interactable));
            }
        }

        IEnumerator StartAction(GuideData guideData,Interactable interactable)
        {
            Interactable.isLock = true;
            yield return new WaitForSeconds((float)guideData.delay);
            
            if(interactable!=null)
                interactable.isIgnoreLock = true;
            onProcess(curData, interactable);
        }

    }
}


