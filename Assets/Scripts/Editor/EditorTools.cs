using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class EditorTools  {


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
 

    [MenuItem("GameObject/UI/Image")]
    static void CreatImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Image", typeof(Image));
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale = Vector3.one;
                (go.transform as RectTransform).anchoredPosition = Vector2.zero;
            }
        }
    }

    [MenuItem("GameObject/UI/CustomButton")]
    static void CreatCustomButton()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Button", typeof(Image),typeof(QY.UI.Button));
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale = Vector3.one;
                (go.transform as RectTransform).anchoredPosition = Vector2.zero;
            }
        }
    }
}
