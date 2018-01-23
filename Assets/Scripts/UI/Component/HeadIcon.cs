using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeadIcon : MonoBehaviour {
    public TextMeshProUGUI nicNameLabel;
    public RawImage head;
    public TextMeshProUGUI starCountLabel;
    public Image bottomFrame;
    public Sprite[] sprites;

    private string headUrl;

    public void setData(string nicname,string headUrl,int starCount,bool isVip)
    {
        nicNameLabel.text = nicname;
        if(starCountLabel != null)
            starCountLabel.text = starCount.ToString();
        bottomFrame.sprite = isVip ? sprites[1] : sprites[0];

        if(this.headUrl!= headUrl)
        {
            this.headUrl = headUrl;
            head.texture = null;
            AssetLoadManager.Instance.LoadAsset<Texture2D>(headUrl, (tex) =>
            {
                head.texture = tex;
            });
        }
       
    }

}
