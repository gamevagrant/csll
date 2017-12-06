using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRecallItem : BaseItemView
{

    public Toggle toggle;
    public HeadIcon headIcon;
    private RecallFriendData data;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(OnToggleChangeValue);
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    public override void SetData(object data)
    {
        this.data = data as RecallFriendData;
        toggle.isOn = this.data.isSelected;
        headIcon.setData(this.data.data.name, this.data.data.head_img, 0, this.data.data.isVip);

    }

    private void OnToggleChangeValue(bool isOn)
    {
        data.isSelected = isOn;
    }
}

public class RecallFriendData
{
    public ShareData.RecallableFriendData data;
    public bool isSelected;
}
