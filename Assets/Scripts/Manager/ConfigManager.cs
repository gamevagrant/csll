
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using LitJson;
using System;
using QY.Guide;
using Object = UnityEngine.Object;

public class ConfigManager {

    public IslandConfig islandConfig;
    public GuideDataConfig guideDataConfig;

    private Dictionary<string, System.Type> configDic = new Dictionary<string, System.Type>
    {
        { "islandConfig" ,typeof(IslandConfig)},
        { "guideDataConfig" ,typeof(GuideDataConfig)},
    };
    private int num;
    public void LoadConfig(System.Action onCompalte)
    {
        num = 0;
        string[] keyList = new string[configDic.Keys.Count];
        configDic.Keys.CopyTo(keyList, 0);
        foreach (string key in keyList)
        {
            AssetBundleLoadManager.Instance.LoadAsset<Object>(FilePathTools.getConfigPath(key), (data) =>
            {
                string k = keyList[num];
                Type type = configDic[k];
                string json = data.ToString();

                MethodInfo mi = GetGenericMethod(typeof(JsonMapper), "ToObject", BindingFlags.Public | BindingFlags.Static, typeof(string));
                MethodInfo miConstructed = mi.MakeGenericMethod(type);
                object obj = miConstructed.Invoke(null, new object[]{ json });

                this.GetType().GetField(key).SetValue(this, obj);

                num++;
                if(num>= keyList.Length)
                {
                    onCompalte();
                }
            });
        }
        
    }

    public static MethodInfo GetGenericMethod(Type targetType, string name, BindingFlags flags, params Type[] parameterTypes)
    {
        var methods = targetType.GetMethods(flags).Where(m => m.Name == name && m.IsGenericMethod);
        foreach (MethodInfo method in methods)
        {
            bool founded = true;
            var parameters = method.GetParameters();
            if (parameters.Length != parameterTypes.Length)
                continue;

            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != parameterTypes[i])
                {
                    founded = false;
                    break;
                }
                   
            }
            if(founded)
            {
                return method;
            }
           
        }
        return null;
    }
}
