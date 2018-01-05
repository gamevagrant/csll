using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingAnimation : MonoBehaviour {

    public float speed = 1;
    public Vector2 sizeStrength = new Vector2(0.2f,0.2f);
    public Vector2 posStrengh = Vector2.zero;


    private float sin;
    RectTransform rect;
    Vector2 size;
    Vector2 pos;
    // Use this for initialization
    void Start () {
        rect = transform as RectTransform;
        size = rect.sizeDelta;
        pos = rect.anchoredPosition;
    }
	
	// Update is called once per frame
	void Update () {
        sin = Mathf.Sin(Time.time * speed);
       
        if (sizeStrength.x > 0 || sizeStrength.y>0)
        {
            rect.sizeDelta = new Vector2(size.x * (1 + sizeStrength.x * sin), size.y * (1 + sizeStrength.y * sin));
        }
        if(posStrengh.x > 0 || posStrengh.y > 0)
        {
            rect.anchoredPosition = new Vector2(pos.x + posStrengh.x * sin, pos.y + posStrengh.y * sin);
        }
       
	}
}
