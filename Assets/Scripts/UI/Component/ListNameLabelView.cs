using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ListNameLabelView : BaseItemView {

    public TextMeshProUGUI text;
    public override void SetData(object data)
    {
        text.text = data.ToString();
    }
}
