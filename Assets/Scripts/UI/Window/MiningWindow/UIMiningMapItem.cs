using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiningMapItem : MonoBehaviour {

    public GameObject lockImage;
    public GameObject unlockImage;
    public GameObject headImage;
    public RawImage head;
    public int islandID;

    public bool SetData(int currIslandID)
    {
        if(currIslandID == islandID)
        {
            unlockImage.SetActive(true);
            headImage.SetActive(true);
            lockImage.SetActive(false);
            AssetLoadManager.Instance.LoadAsset<Texture2D>(GameMainManager.instance.model.userData.headImg, (tex) =>
            {
                head.texture = tex;
            });
            return true;
        }else if(currIslandID>islandID)
        {
            unlockImage.SetActive(true);
            headImage.SetActive(false);
            lockImage.SetActive(false);
        }
        else
        {
            unlockImage.SetActive(false);
            headImage.SetActive(false);
            lockImage.SetActive(true);
        }
        return false;
    }
}
