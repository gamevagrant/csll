using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingPanel : MonoBehaviour {

    public TMP_InputField inputField;
    public Slider slider;

    private void Awake()
    {
        EventDispatcher.instance.AddEventListener(EventEnum.LOADING_PROGRESS, OnLoadingProgress);
        EventDispatcher.instance.AddEventListener(EventEnum.UPDATE_ASSETS_COMPLATE, OnUpdateAssetComplate);

        inputField.gameObject.SetActive(false);
        slider.value = 0;
        
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventListener(EventEnum.LOADING_PROGRESS, OnLoadingProgress);
        EventDispatcher.instance.RemoveEventListener(EventEnum.UPDATE_ASSETS_COMPLATE, OnUpdateAssetComplate);
    }

    private void OnLoadingProgress(BaseEvent evt)
    {
        LoadingEvent e = evt as LoadingEvent;
        slider.value = e.progress;
        slider.gameObject.SetActive(true);
    }

    private void OnUpdateAssetComplate(BaseEvent evt)
    {
        string openID = PlayerPrefs.GetString("OpenID");
        if (!string.IsNullOrEmpty(openID))
        {
            inputField.text = openID;
        }

        inputField.gameObject.SetActive(true);
        slider.gameObject.SetActive(false);
    }

    public void OnClickLoginBtn()
    {
        if(!string.IsNullOrEmpty(inputField.text))
        {
            EventDispatcher.instance.DispatchEvent(new BaseEvent(EventEnum.LOGIN_START, inputField.text));
        }
    }
}
