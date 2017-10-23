using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadIcon : MonoBehaviour {
    public Text nicNameLabel;
    public RawImage head;
    public Text starCountLabel;
    public Image bottomFrame;
    public Sprite[] sprites;

    public void setData(string nicname,string headUrl,int starCount,bool isVip)
    {
        nicNameLabel.text = nicname;
        starCountLabel.text = starCount.ToString();
        bottomFrame.sprite = isVip ? sprites[1] : sprites[0];
        AssetLoadManager.Instance.LoadAsset<Texture2D>(headUrl, (tex) =>
        {
            head.texture = tex;
        });
    }

}
