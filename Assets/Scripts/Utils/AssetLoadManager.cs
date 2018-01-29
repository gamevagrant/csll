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
    private const int RECOVERY_TIME = 120;//这个时间后没有被请求就自动销毁
    private Dictionary<string, CacheObject> cache = new Dictionary<string, CacheObject>();
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

	public void Update()
	{
		if (queue.Count>0 && !isLoading)
		{
			Action act = queue.Dequeue();
			if (act != null)
			{
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
        //为了保证后请求的图片就在后面刷新，避免列表复用时下载没结束被新的缓存图片覆盖，加载结束又被之前的图片覆盖掉，这里即使有缓存图片也要加入队列排队
        queue.Enqueue(() =>
        {
            TryClearCache();
            StartCoroutine(loadAsync<T>(path, callback, isCache));
        });

    }



	private IEnumerator loadAsync<T>(string url,Action<T> callback,bool isCache)
	{
        string path = url;

        if (isCache)
        {
            CacheObject obj;
            if (callback != null && cache.TryGetValue(url, out obj) && obj != null)
            {
                callback((T)obj.obj);
                obj.time = Time.time;
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
                    AddCache(url,res);
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

    private void AddCache(string path, object obj)
    {
        if (!cache.ContainsKey(path))
        {
            cache.Add(path, new CacheObject(obj, Time.time));
        }
    }
    private void TryClearCache()
    {
        List<string> list = new List<string>(cache.Keys);

        foreach (string key in list)
        {
            CacheObject co = cache[key];
            if (Time.time - co.time > RECOVERY_TIME)
            {
                cache.Remove(key);
            }

        }

    }
    private class CacheObject
    {

        public object obj;
        public float time;

        public CacheObject(object obj, float time)
        {
            this.obj = obj;
            this.time = time;

        }
    }
}
