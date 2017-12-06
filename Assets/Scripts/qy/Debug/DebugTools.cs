using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QY.Debug
{
    internal class DebugTools : ConsoleBase
    {
        private string logString="";
        private bool isShow = false;
        protected override void Awake()
        {
            base.Awake();
            Application.logMessageReceived += OnLogMessage;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.touchCount>0)
            {
                Vector2 pos = Input.GetTouch(0).position;
                if(pos.x<100 && pos.y< 100)
                {
                    isShow = true;
                }
            }
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Input.mousePosition;
                if (pos.x < 100 && pos.y < 100)
                {
                    isShow = true;
                }
            }
        }

        private void OnLogMessage(string condition, string stackTrace, LogType type)
        {
            var typePrefix = type != LogType.Log ? type + ": " : "";
            logString = System.DateTime.Now.ToLongTimeString() + "> " + typePrefix + condition + "\n" + logString;
            logString = logString.Substring(0, Mathf.Min(10000,logString.Length));
        }

        protected void OnGUI()
        {
            if(isShow)
            {
                GUILayout.BeginVertical();
                if (this.Button("Back"))
                {
                    isShow = false;
                }

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector2 scrollPosition = this.ScrollPosition;
                    scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
                    this.ScrollPosition = scrollPosition;
                }
#endif
                this.ScrollPosition = GUILayout.BeginScrollView(
                    this.ScrollPosition,
                    GUILayout.MinWidth(ConsoleBase.MainWindowFullWidth));

                GUILayout.TextArea(
                    logString,
                    this.TextStyle,
                    GUILayout.ExpandHeight(true),
                    GUILayout.MaxWidth(ConsoleBase.MainWindowWidth));

                GUILayout.EndScrollView();

                GUILayout.EndVertical();
            }
            
        }

    }
}

