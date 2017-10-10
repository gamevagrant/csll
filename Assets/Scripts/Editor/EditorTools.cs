using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorTools  {

	[@MenuItem("Tools/清除用户数据")]
	public static void buildAllAsset()
	{
		PlayerPrefs.DeleteAll ();
	}
}
