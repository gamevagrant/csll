using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UINewUserGuiderWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UINewUserGuiderWindow;
                _windowData.type = UISettings.UIWindowType.Fixed;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.colliderType = UISettings.UIWindowColliderType.SemiTransparent;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public TextMeshProUGUI text;
    public GameObject bubble;
    public QY.UI.Toggle toggle;

    private float timeTag;
    private const float TIME_SPACING = 24;//气泡关闭后到下次打开时的时间
    private const float SHOW_TIME = 6;//气泡显示时间
    private int tipsIndex = 0;
    private bool isShowBubble = true;

    private void Update()
    {
        if(isShowBubble && Time.time> timeTag+ TIME_SPACING + SHOW_TIME)
        {
            timeTag = Time.time;
            StartCoroutine(ShowBubble(SHOW_TIME));
        }
    }

    protected override void StartShowWindow(object[] data)
    {
        toggle.isOn = false;
        timeTag = Time.time;
        StartCoroutine(ShowBubble(SHOW_TIME));
    }

    private IEnumerator ShowBubble(float delay)
    {
        if(GameMainManager.instance.configManager.guiderTipsConfig!=null && GameMainManager.instance.configManager.guiderTipsConfig.data!=null)
        {
            tipsIndex = Random.Range(0, GameMainManager.instance.configManager.guiderTipsConfig.data.Length);
            text.text = GameMainManager.instance.configManager.guiderTipsConfig.data[tipsIndex];
        }
        bubble.SetActive(true);
        yield return new WaitForSeconds(delay);
        bubble.SetActive(false);
    }

    public void OnClickCloseBubble()
    {
        bubble.SetActive(false);
        timeTag = Time.time;
        isShowBubble = !toggle.isOn;
    }

    public void OnClickGuiderBtn()
    {
        GameMainManager.instance.uiManager.OpenWindow(UISettings.UIWindowID.UIEmptyEnergyGuideWindow,new ShowEmptyGuideWindowData(ShowEmptyGuideWindowData.PanelType.EmptyEnergy));
    }
    
}
