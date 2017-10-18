﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

/// <summary>
/// 本地和网络资源的加载器，不能用以加载AssetBundle资源，要加载AssetBundle请使用AssetBundleLoadManager。
/// </summary>
public class AssetLoadManager:MonoBehaviour
{

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	string prefix = "file:///";
#elif UNITY_IPHONE
	string prefix = "";
#else
	string prefix = "file://";
#endif

	private Dictionary<string, Object> cache = new Dictionary<string, Object>();
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
	public void LoadAsset<T>(string url,Action<T> callback ,bool isCache = true) where T:Object
	{
		string path = url;
		if (path.IndexOf(@"http://") == -1)
		{
			if (path.IndexOf(@"file://") == -1)
			{
				path = prefix + path;

			}
		}

		path = FilePathTools.normalizePath(path);
		Object res;
		if (callback != null && cache.TryGetValue(path, out res) && res != null)
		{
			callback(res as T);
		}
		else
		{
			queue.Enqueue(() =>
			{
				StartCoroutine(loadAsync<T>(path, callback,isCache));
			});
		}
	
	}

	private IEnumerator loadAsync<T>(string url,Action<T> callback ,bool iscache)where T:Object
	{
		isLoading = true;
		//string path = CacheManager.instance.getLocalPath(url);
        string path="";

        if (!string.IsNullOrEmpty(path))
		{
			path = prefix + path;
		}
		else
		{
			path = url;
		}
		Debug.Log("================" + path);

		path = FilePathTools.normalizePath(path);
		WWW www = new WWW(path);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			Object res;
			if (typeof(T) == typeof(Texture2D))
			{
				Texture2D tex = new Texture2D(4, 4);
				tex.LoadImage(www.bytes);
				res = tex;
			}
			else if (typeof(T) == typeof(AudioClip))
			{
				res = www.GetAudioClip();
			}
			else
			{
				res = www.assetBundle;
			}
			if (iscache && !cache.ContainsKey(url))
			{
				cache.Add(url, res);
			}
				
			//CacheManager.instance.addCache(url, www.bytes);
			callback((T)res);
			
		}
		else
		{
			Debug.Log(www.error);
		}
		www.Dispose();
		isLoading = false;
	}


}
