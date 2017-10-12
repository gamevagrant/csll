using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMessage {

    public int errcode;   // 错误代码
    public string errmsg;    // 错误信息

    public bool res
    {
        get
        {
            return string.IsNullOrEmpty(errmsg);
        }
    }
}
