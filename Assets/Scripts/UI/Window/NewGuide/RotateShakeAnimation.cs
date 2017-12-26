using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShakeAnimation : MonoBehaviour {

    Quaternion q = Quaternion.identity;
    float f;
    // Update is called once per frame
    void Update () {
        f = Mathf.Cos(Time.time * 5) ;
        q.eulerAngles = new Vector3(0,0,f * 10+5);
        transform.rotation = q;
        transform.localScale = new Vector3((-f + 1) * 0.1f + 0.9f, (-f + 1) * 0.1f + 0.9f, 0);
	}
}
