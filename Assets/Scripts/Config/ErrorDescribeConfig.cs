using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorDescribeConfig {

    public Dictionary<string, string> data;

    public string GetDescribe(string errorCode,string defaultDes="")
    {
        if(data.ContainsKey(errorCode))
        {
            return data[errorCode];
        }
        return defaultDes;
    }
}
