using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDungeonCard : BaseItemView {

    public GameObject selectBtn;
    public GameObject headGo;
    public Image card;
    public RawImage head;

    public Sprite[] cardsSprite;

    public DungeonCardData data;

    // Use this for initialization
    void Start () {
		
	}
	
	public override void SetData(object data)
    {
        this.data = data as DungeonCardData;
        selectBtn.SetActive(data == null);
        headGo.SetActive(data!=null);

        if(data!=null)
        {
            if(cardsSprite!=null && cardsSprite.Length>0)
            {
                card.sprite = cardsSprite[this.data.num < cardsSprite.Length ? this.data.num : 0];
            }
            
            AssetLoadManager.Instance.LoadAsset<Texture2D>(this.data.head_img, (tex) =>
            {
                head.texture = tex;
            }, true);
        } else
        {
            card.sprite = cardsSprite[0];
        }
    }


}
