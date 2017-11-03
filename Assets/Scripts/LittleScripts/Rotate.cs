using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    /// <summary>
    /// 度/每秒
    /// </summary>
    public float speed = 60;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1), speed * Time.deltaTime);
    }
}
