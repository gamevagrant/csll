using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class EditorTools  {

	[@MenuItem("Tools/清除用户数据")]
	public static void buildAllAsset()
	{
		PlayerPrefs.DeleteAll ();
	}

    [MenuItem("Tools/设置面板raycastTarget = false")]
    public static void SetRayTask()
    {
        Graphic[] graphics = Selection.activeTransform.GetComponentsInChildren<Graphic>();
        foreach (Graphic g in graphics)
        {
            Selectable selectable = g.transform.GetComponent<Selectable>();
            if(selectable == null)
            {
                g.raycastTarget = false;
                Debug.Log(g.gameObject.transform);
            }
            
        }
        Debug.Log(Selection.activeGameObject.name);
    }

    [MenuItem("Tools/清理player")]
    public static void ClearPlayer()
    {
        PlayerPrefs.DeleteAll();
    }
}
