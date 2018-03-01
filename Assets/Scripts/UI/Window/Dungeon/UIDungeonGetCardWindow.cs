using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDungeonGetCardWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            if (_windowData == null)
            {
                _windowData = new UIWindowData();
                _windowData.id = UISettings.UIWindowID.UIDungeonGetCardWindow;
                _windowData.type = UISettings.UIWindowType.PopUp;
                _windowData.colliderMode = UISettings.UIWindowColliderMode.Normal;
                _windowData.showMode = UISettings.UIWindowShowMode.DoNothing;
                _windowData.navMode = UISettings.UIWindowNavigationMode.IgnoreNavigation;
            }
            return _windowData;
        }
    }

    public Image backLight;
    public Image bigSmall;
    public Sprite[] lightSprites;
    public Sprite[] bigSmallSprites;

    protected override void StartShowWindow(object[] data)
    {
        AudioManager.instance.PlaySound(AudioNameEnum.dungeon_extractShow);
        int type = (int)data[0];
        if(type == 0)
        {
            backLight.sprite = lightSprites[0];
            bigSmall.sprite = bigSmallSprites[0];
        }
        else
        {
            backLight.sprite = lightSprites[1];
            bigSmall.sprite = bigSmallSprites[1];
        }
    }

}
