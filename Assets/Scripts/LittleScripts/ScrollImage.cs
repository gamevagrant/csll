using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollImage : MonoBehaviour {

    public float speedX = 1;
    public float speedY = 0;
    Image render;

	// Use this for initialization
	void Awake () {
        render = GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {
		if(render != null)
        {
            render.material.mainTextureOffset += new Vector2(speedX*Time.deltaTime*0.2f,speedY*Time.deltaTime*0.2f);
        }
	}
}
