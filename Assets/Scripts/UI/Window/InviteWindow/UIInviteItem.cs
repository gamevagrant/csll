using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIInviteItem : BaseItemView {

    public Toggle toggle;
    public TextMeshProUGUI nameText;
    private InviteItemData data;

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
        this.data = data as InviteItemData;
        toggle.isOn = this.data.isSelected;
        nameText.text = this.data.name;
    }

    private void OnToggleChangeValue(bool isOn)
    {
        data.isSelected = isOn;
    }

}

public class InviteItemData
{
    public string name;
    public string id;
    public bool isSelected;
}