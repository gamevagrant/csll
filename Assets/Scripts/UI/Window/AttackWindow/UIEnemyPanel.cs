using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyPanel : MonoBehaviour {

    public DynamicScrollView scrollView;
	// Use this for initialization
	void Start () {

    }
	
    public void setData(List<SelectEnemyData> datas)
    {
        scrollView.setDatas(datas);
    }
}
