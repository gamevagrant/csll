using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

/// <summary>
/// 本地和网络资源的加载器，不能用以加载AssetBundle资源，要加载AssetBundle请使用AssetBundleLoadManager。
/// </summary>
public class AssetLoadManager:MonoBehaviour
{

	private Dictionary<string, object> cache = new Dictionary<string, object>();
 	private Queue<Action> queue = new Queue<Action>();
	private bool isLoading = false;

	private static AssetLoadManager _instance;

	public static AssetLoadManager Instance
	{
		get
		{
			return _instance;
		}

	}

	public void Awake()
	{
		_instance = this;
	}

    int aa = 0;
	public void Update()
	{
		if (queue.Count>0 && !isLoading)
		{
			Action act = queue.Dequeue();
			if (act != null)
			{
                aa++;
				act();
			}
		}
	}

	/// <summary>
	/// 加载一个本地资源（file）
	/// </summary>
	/// <param name="path"></param>
	public void LoadAsset<T>(string url,Action<T> callback ,bool isCache = true)
    {
		string path = url;

		path = FilePathTools.normalizePath(path);
        queue.Enqueue(() =>
        {
            StartCoroutine(loadAsync<T>(path, callback, isCache));
        });

    }



	private IEnumerator loadAsync<T>(string url,Action<T> callback,bool isCache)
	{
        string path = url;

        if (isCache)
        {
            object obj;
            if (callback != null && cache.TryGetValue(url, out obj) && obj != null)
            {
                callback((T)obj);
                yield break;
            }

            string localPath = CacheManager.instance.GetLocalPath(url);
            if (!string.IsNullOrEmpty(localPath))
            {
                path = localPath;
            }

        }

        isLoading = true;

        
		//Debug.Log("==开始使用WWW下载==:" + path);

		path = FilePathTools.normalizePath(path);
		WWW www = new WWW(path);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			object res;
            Type type =typeof(T);
            if(type == typeof(Texture2D))
            {
                Texture2D tex = new Texture2D(4, 4);
                www.LoadImageIntoTexture(tex);
                res = tex;
            }else if (type == typeof(AssetBundle))
            {
                res = www.assetBundle;
            }
            else if (type == typeof(string))
            {
                res = www.text;
            }
            else
            {
                res = www.bytes;
            }

            if (isCache)
            {
                if (!cache.ContainsKey(url))
                {
                    cache.Add(url, res);
                }
                CacheManager.instance.AddCache(url, www.bytes);
            }

            www.Dispose();
            isLoading = false;
            callback((T)res);
			
		}
		else
		{
            Debug.Log(url);
			Debug.Log(www.error);
            www.Dispose();
            isLoading = false;
        }
		
	}


}
