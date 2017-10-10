using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class ApplationUtils 
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
}
