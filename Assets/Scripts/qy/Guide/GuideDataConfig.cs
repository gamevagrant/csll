using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QY.Guide
{
    public class GuideDataConfig
    {
        public GuideData[] data;

        private int progress;
        private int index;

        public GuideData SetProgress(int progress)
        {
            this.progress = progress;
            for(int i = 0;i< data.Length;i++)
            {
                if(data[i].progress == progress)
                {
                    index = i;
                    return data[index];

                }
            }
            return null;
        }

        public GuideData next
        {
            get
            {
                index++;
                if(index< data.Length)
                {
                    return data[index];
                }else
                {
                    return null;
                }
               
            }
        }
    }

    public class GuideData
    {
        public enum ActionType
        {
            click,
            tips,
            open,
        }

        /// <summary>
        /// 可交互物体的ID 点击这个物体后会触发引导结束当前点击引导，进行下一个引导
        /// </summary>
        public string interact_id;
        /// <summary>
        /// 此值不为空时，当 当前状态为此值时才会触发执行此次引导
        /// </summary>
        public string state;
        /// <summary>
        /// 服务器保存的交互进度，每次和服务器通信的进度都会让progress增加
        /// </summary>
        public int progress;
        /// <summary>
        /// 类型 点击指引 弹出对话框 滑动指引等
        /// </summary>
        public string type;
        /// <summary>
        /// 要显示的文字内容
        /// </summary>
        public string content;
        /// <summary>
        /// 和上一个指引之间的延迟
        /// </summary>
        public double delay;
        /// <summary>
        /// 是否执行过
        /// </summary>
        public bool isProcess = false;

        public ActionType actionType
        {
            get
            {
                return (ActionType)System.Enum.Parse(typeof(ActionType), type);
            }
        }

    }
}

