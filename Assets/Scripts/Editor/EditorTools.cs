﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using LitJson;
using System;
using System.Text.RegularExpressions;


public class EditorTools  {

    [MenuItem("Tools/通过json数据裁切图集")]
    static void SetTextureMultipleSpriteEditor()
    {
        if (Selection.activeObject && Selection.activeObject is Texture)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.maxTextureSize = 4096;
            importer.SaveAndReimport();

            int height = (Selection.activeObject as Texture).height;

            string jsonPath = path.Replace(Path.GetExtension(path), ".json");
            object jsonObj = AssetDatabase.LoadMainAssetAtPath(jsonPath);
            if (jsonObj != null)
            {
                string json = jsonObj.ToString();
                JsonData jsonData = JsonMapper.ToObject(json);

                IEnumerator<string> enumrator = jsonData["mc"].Keys.GetEnumerator();
                enumrator.MoveNext();
                string keyName = enumrator.Current;
                JsonData[] frames = JsonMapper.ToObject<JsonData[]>(jsonData["mc"][keyName]["frames"].ToJson()) ;
                

                List<SpriteMetaData> list = new List<SpriteMetaData>();
                for (int i =0;i<frames.Length;i++)
                {
                    JsonData jd = jsonData["res"][frames[i]["res"].ToString()];
                    int x = int.Parse(jd["x"].ToString());
                    int y = int.Parse(jd["y"].ToString());
                    int w = int.Parse(jd["w"].ToString());
                    int h = int.Parse(jd["h"].ToString());
                    SpriteMetaData md = new SpriteMetaData()
                    {
                        name = "Q"+i.ToString(),
                        rect = new Rect(x, height - y - h, w, h),
                    };

                    list.Add(md);
                }

               
                importer.maxTextureSize = 2048;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritesheet = list.ToArray();
                //这里有个unity的bug，不改变importer本身的参数，只改变spritesheet的时候unity认为这个资源没有变化没办法正常保存
                //这里先改下别的参数再改回来让其能正常保存
                importer.mipmapEnabled = !importer.mipmapEnabled;
                importer.SaveAndReimport();
                importer.mipmapEnabled = !importer.mipmapEnabled;
                importer.SaveAndReimport();


            }
            else
            {
                Debug.LogAssertion("没有找到与图片命名一致的json数据" + jsonPath);
            }

        }
        else
        {
            Debug.LogAssertion("只能对图片进行操作");
        }
    }



   // [MenuItem("Tools/设置面板raycastTarget = false")]
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

    [MenuItem("Tools/保存资源")]
    public static void SaveAssets()
    {
        AssetDatabase.SaveAssets();
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
                go.transform.localPosition = Vector3.zero;
                (go.transform as RectTransform).anchoredPosition = Vector2.zero;
                Selection.activeGameObject = go;
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
                go.transform.localPosition = Vector3.zero;
                (go.transform as RectTransform).anchoredPosition = Vector2.zero;
                Navigation nav = new Navigation();
                nav.mode = Navigation.Mode.None;
                go.GetComponent<QY.UI.Button>().navigation = nav;
                Selection.activeGameObject = go;
            }
        }
    }


}
