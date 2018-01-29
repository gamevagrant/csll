using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNamesConfig {

    public string[] firstNames;
    public string[] lastNames;

    public string GetRandomName()
    {
        string str = firstNames[Random.Range(0, firstNames.Length)];
        str += lastNames[Random.Range(0, lastNames.Length)];
        return str;
    }
}
