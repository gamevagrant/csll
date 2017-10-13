using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIWheelPanel : MonoBehaviour {

    public Text[] names;//文字显示位置
    public Image[] images;//图片显示位置
    public Sprite[] sprites;//sprite集合
    public Image reflective;//反光
    public Transform wheel;

    private bool isWorking;
    
	// Use this for initialization
	void Awake ()
    {
        reflective.gameObject.SetActive(false);
        EventDispatcher.instance.addEventListener(EventEnum.UPDATE_USERDATA, onUpdateUserData);
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.removeEventListener(EventEnum.UPDATE_USERDATA, onUpdateUserData);
    }

    private void Start()
    {
        isWorking = false;
    }

    // Update is called once per frame
    void Update () {
		
	}


    public void setData(RollerItemData[] datas)
    {
        foreach(RollerItemData data in datas)
        {
            int index = data.index;
            Text t = names[index];
            Image pic = images[index];
            pic.enabled = true;
            if(data.type == "steal")
            {
                t.text = data.name;
                pic.sprite = sprites[0];
            }else if(data.type == "energy")
            {
                t.text = data.name;
                pic.sprite = sprites[1];
            }
            else if (data.type == "shield")
            {
                t.text = data.name;
                pic.sprite = sprites[2];
            }
            else if (data.type == "shoot")
            {
                t.text = data.name;
                pic.sprite = sprites[3];
            }
            else if (data.type == "coin")
            {
                t.text = data.name;
                pic.enabled = false;
            }
        }
    }

    public void startRotate()
    {
        if(!isWorking)
        {
            isWorking = true;

            NetManager.instance.roll(GameModel.userData.uid.ToString(), (ret, data) => {
                if(data.isOK)
                {
                    StartCoroutine(rotateWheel(data.data.rollerItem.index));
                }
               
            });

            

        }

    }

    private IEnumerator rotateWheel(int index)
    {
        reflective.gameObject.SetActive(true);
        wheel.DOLocalRotate(new Vector3(0, 0, 360 * 8 + 36 * index), 4, RotateMode.FastBeyond360).SetEase(Ease.OutQuart).OnComplete(() => {
            isWorking = false;
        });
        yield return new WaitForSeconds(1.5f);
        reflective.gameObject.SetActive(false);
    }

    private void onUpdateUserData(params object[] data)
    {
        UserData ud = data[0] as UserData;
        setData(ud.rollerItems);
    }
}
