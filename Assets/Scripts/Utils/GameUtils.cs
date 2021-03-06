﻿using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class GameUtils 
{
	public static void setLayer(GameObject go, int layer)
	{
		go.layer = layer;

		Transform t = go.transform;

		for (int i = 0, imax = t.childCount; i < imax; ++i)
		{
			Transform child = t.GetChild(i);
			setLayer(child.gameObject, layer);
		}
	}

	public static GameObject createGameObject(GameObject parent, string name = "new GameObject")
	{
		GameObject go = new GameObject(name);
		if (parent != null)
		{
			Transform t = go.transform;
			t.SetParent(parent.transform);
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			go.layer = parent.layer;
		}
		return go;
	}

	public static GameObject createGameObject(GameObject parent, Object prefab)
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		
		if (go != null && parent != null)
		{
			go.name = prefab.name;
			Transform t = go.transform;
			t.SetParent(parent.transform);
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			go.layer = parent.layer;
		}
		return go;
	}

	public static string getMD5(string msg)
	{
		StringBuilder sb = new StringBuilder();

		using (MD5 md5 = MD5.Create())
		{
			byte[] buffer = Encoding.UTF8.GetBytes(msg);
			byte[] newB = md5.ComputeHash(buffer);

			foreach (byte item in newB)
			{
				sb.Append(item.ToString("x2"));
			}
		}

		return sb.ToString();
	}

    /// <summary>  
    /// 将c# DateTime时间格式转换为Unix时间戳格式  
    /// </summary>  
    /// <param name="time">时间</param>  
    /// <returns>long</returns>  
    public static long DateTimeToTimestamp(System.DateTime time)
    {
        System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        long t = (long)(time - startTime).TotalSeconds;   //除10000调整为13位      
        return t;
    }

    /// <summary>
    /// unix时间戳转DateTime
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static System.DateTime TimestampToDateTime(long timestamp)
    {

        System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区

        System.DateTime dt = startTime.AddSeconds(timestamp);

        return dt;

    }

    /// <summary>
    /// 获取数字的货币表现形式的字符串
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    public static string GetCurrencyString(long money)
    {
        return string.Format("{0:N0}", money);
    }

    /// <summary>
    /// 获取钱币的短字符串以K为最小单位
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    public static string GetShortMoneyStr(long money)
    {
        string[] sign = new string[] { "K", "M", "B", "T" };
        string str = GetCurrencyString(money);
        string[] list = str.Split(',');
        if(list.Length<2)
        {

        }else if (list.Length - 1 <= sign.Length)
        {
            str = list[0] + sign[list.Length - 2];
        }else
        {
            float tag = money / Mathf.Pow(1000, sign.Length );
            str = GetCurrencyString((long)tag) + sign[sign.Length - 1];
        }
        return str;
    }
}
