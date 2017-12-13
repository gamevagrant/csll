using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;

public class NetMessage {

    public int errcode;   // 错误代码
    public string errmsg;    // 错误信息

    public bool isOK
    {
        get
        {
            return errcode==0;
        }
    }


}
