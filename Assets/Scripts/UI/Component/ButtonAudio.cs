using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour {

    Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
        if(btn != null)
        {
            btn.onClick.AddListener(() => {
                GameMainManager.instance.audioManager.PlaySound(AudioNameEnum.button_click);
            });
        }else
        {
            Debug.LogError("没有找到Button组件:"+name);
        }
    }
    // Use this for initialization
    void Start () {
		
	}

    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }
}
