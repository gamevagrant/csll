using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGetStealAnimation : MonoBehaviour {

    public GameObject tips;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        tips.SetActive(GameMainManager.instance.model.userData.isTutorialing);
    }

}
