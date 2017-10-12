using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
///assetbundle 加载器 在编辑器模式下可以切换成直接加载资源
///  需要关注下 lock 的运行情况,是否有死锁的问题
/// </summary>
public class AssetBundleLoader {
	
	private const int RECOVERY_TIME = 180;//这个时间后没有被请求就自动销毁
	
	private static Dictionary<string,string> lockDic = new Dictionary<string, string>();
	private static Dictionary<string,CacheObject> dicCacheObject = new Dictionary<string, CacheObject> ();
	static AssetBundleManifest manifest;

	public Object Obj;
	public AssetBundle assetBundle; 
	public AsyncOperation asyncOperation;



	Dictionary<string,AssetBundle> dependencyAssetBundles;
	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	string prefix = "file:///";
	#elif UNITY_IPHONE
	string prefix = "file://";
	#else
	string prefix = "";
	#endif

	private static void tryClearCache()
	{
		List<string> list = new List<string> (dicCacheObject.Keys);
		
		foreach(string key in list)
		{
			CacheObject co = dicCacheObject[key];
			if(Time.time - co.time>RECOVERY_TIME)
			{
				dicCacheObject.Remove(key);
			}
			
		}

	}
	

	public IEnumerator loadAsync<T>(string path)where T : Object 
	{
		path = FilePathTools.normalizePath(path);
		Debug.Log ("AssetBundleLoader.loadAsync----------------------:开始异步加载资源："+path);
		string assetBundleName = FilePathTools.getAssetBundleNameWithPath (path);

		bool isUseAssetBundle = GameSetting.isUseAssetBundle;
		if(!isUseAssetBundle)
		{
			#if UNITY_EDITOR
			path = FilePathTools.getRelativePath(path);
			Obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T));


			if (Obj == null)
			{
				Debug.LogError ("Asset not found at path:" + path);
			}
			yield break;
			#endif
		}else
		{
			//打的ab包都资源名称和文件名都是小写的
			//path = Path.GetDirectoryName(path)+"/"+Path.GetFileNameWithoutExtension(path).ToLower();
			CacheObject co;
			if(dicCacheObject.TryGetValue(path,out co))
			{
				Obj = co.obj;
				co.time = Time.time;
			}else
			{
				WWW www ;
				AssetBundleRequest assetRequest;
				//1加载Manifest文件
				if(manifest == null)
				{
					string manifestPath = FilePathTools.manifestPath;
					while(lockDic.ContainsKey(manifestPath))
					{
						Debug.Log("正在等待上一个相同资源加载完毕"+manifestPath);
						yield return null;
					}
					lockDic.Add(manifestPath,"AssetBundleManifest");
					www = new WWW(prefix+manifestPath);
					yield return www;
					if (string.IsNullOrEmpty (www.error)) 
					{
						AssetBundle manifestAB = www.assetBundle;
						yield return assetRequest = manifestAB.LoadAssetAsync<AssetBundleManifest> ("AssetBundleManifest");
						manifest = assetRequest.asset as AssetBundleManifest;
						manifestAB.Unload (false);
						
					}else
					{
						Debug.Log("===================Manifest加载出错"+www.error);
					}
					lockDic.Remove(manifestPath);
				}


				//2获取文件依赖列表
				string[] dependencies = manifest.GetAllDependencies(assetBundleName);
				
				//3加载依赖资源
				dependencyAssetBundles = new Dictionary<string, AssetBundle>();
				Debug.Log ("AssetBundleLoader.loadAsync----------------------:开始加载依赖资源："+dependencies.Length.ToString());
				foreach(string fileName in dependencies)
				{
					string dependencyPath = FilePathTools.root + "/" + fileName;
					Debug.Log ("AssetBundleLoader.loadAsync----------------------:开始加载依赖资源："+dependencyPath);
					while(lockDic.ContainsKey(dependencyPath))
					{
						Debug.Log("正在等待上一个相同的依赖资源加载完毕" + dependencyPath);
						yield return null;
					}
					lockDic.Add(dependencyPath,fileName);
					
					www = new WWW(prefix + dependencyPath);
					yield return www;
					if(string.IsNullOrEmpty(www.error))
					{
						dependencyAssetBundles.Add(dependencyPath,www.assetBundle);
					}else
					{
						Debug.Log(www.error);
					}
					
				}
				//4加载目标资源
				while(lockDic.ContainsKey(path))
				{
					Debug.Log("正在等待上一个相同资源加载完毕" + path);
					yield return null;
				}
				lockDic.Add(path,assetBundleName);

				www = new WWW(prefix + path);
				yield return www;
				if(string.IsNullOrEmpty(www.error))
				{
					assetBundle = www.assetBundle;
					yield return assetRequest = assetBundle.LoadAssetAsync(Path.GetFileNameWithoutExtension(path),typeof(T));

					Obj = assetRequest.asset;
					dicCacheObject.Add(path,new CacheObject(Obj,Time.time));
					//5释放目标资源
					assetBundle.Unload(false);
					assetBundle = null;
				}else
				{
					Debug.Log(www.error);
				}
				lockDic.Remove(path);

				if(dependencyAssetBundles!=null)
				{
					//6释放依赖资源
					foreach(string key in dependencyAssetBundles.Keys)
					{
						AssetBundle dependencyAB = dependencyAssetBundles[key];
						dependencyAB.Unload(false);
						lockDic.Remove(key);
					}
				}
				
				

				tryClearCache();
			}
		}
	}
	
	public IEnumerator loadSceneAsync(string path,string sceneName)
	{
		asyncOperation = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
		yield return asyncOperation;

		/*
		path = FilePathTools.normalizePath(path);
		string assetBundleName = FilePathTools.getAssetBundleNameWithPath(path);

		Debug.Log ("AssetBundleLoader.loadSceneAsync----------------------:开始异步加载场景："+path);
		bool isUseAssetBundle = GameSetting.isUseAssetBundle;
		if(!isUseAssetBundle)
		{
			#if UNITY_EDITOR
			asyncOperation = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
			yield return asyncOperation;
			yield break;
			#endif
		}else
		{
			WWW www ;
			AssetBundleRequest assetRequest;
			//1加载Manifest文件
			if(manifest == null)
			{
				string manifestPath = FilePathTools.manifestPath;
				while(lockDic.ContainsKey(manifestPath))
				{
					Debug.Log("正在等待上一个相同资源加载完毕"+manifestPath);
					yield return null;
				}
				lockDic.Add(manifestPath,"AssetBundleManifest");
				www = new WWW(prefix+manifestPath);
				yield return www;
				if (string.IsNullOrEmpty (www.error)) 
				{
					AssetBundle manifestAB = www.assetBundle;
					yield return assetRequest = manifestAB.LoadAssetAsync<AssetBundleManifest> ("AssetBundleManifest");
					manifest = assetRequest.asset as AssetBundleManifest;
					manifestAB.Unload (false);

				}else
				{
					Debug.Log("===================Manifest加载出错"+www.error);
				}
				lockDic.Remove(manifestPath);
			}

			//2获取文件依赖列表
			string[] dependencies = manifest.GetAllDependencies(assetBundleName);
			Debug.Log ("AssetBundleLoader.loadSceneAsync----------------------:开始加载依赖资源："+dependencies.Length.ToString());
			//3加载依赖资源
			dependencyAssetBundles = new Dictionary<string, AssetBundle>();
			foreach(string fileName in dependencies)
			{
				string dependencyPath = FilePathTools.root + "/" + fileName;
				Debug.Log ("AssetBundleLoader.loadSceneAsync----------------------:开始加载依赖资源："+dependencyPath);
				while(lockDic.ContainsKey(dependencyPath))
				{
					Debug.Log("正在等待上一个相同的依赖资源加载完毕" + dependencyPath);
					yield return null;
				}
				lockDic.Add(dependencyPath,fileName);
				
				www = new WWW(prefix + dependencyPath);
				yield return www;
				if(string.IsNullOrEmpty(www.error))
				{
					dependencyAssetBundles.Add(dependencyPath,www.assetBundle);
				}else
				{
					Debug.Log("===================依赖资源加载出错"+www.error);
				}
				
			}
			Debug.Log ("AssetBundleLoader.loadSceneAsync----------------------:开始释放Manifest文件");
			//4加载目标资源
			while(lockDic.ContainsKey(path))
			{
				Debug.Log("正在等待上一个相同资源加载完毕" + path);
				yield return null;
			}
			lockDic.Add(path,sceneName);

			www = new WWW(prefix + path);
			yield return www;
			if(string.IsNullOrEmpty(www.error))
			{
				assetBundle = www.assetBundle;

				asyncOperation = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
				yield return asyncOperation;
				Debug.Log ("AssetBundleLoader.loadSceneAsync----------------------:开始释放目标资源：");
				//5释放目标资源
				assetBundle.Unload(false);
				assetBundle = null;

			}else
			{
				Debug.Log("===================目标资源加载出错"+www.error);
			}
			lockDic.Remove(path);
			if(dependencyAssetBundles!=null)
			{
				//6释放依赖资源
				foreach(string key in dependencyAssetBundles.Keys)
				{
					Debug.Log ("AssetBundleLoader.loadSceneAsync----------------------:开始释放目依赖资源源：");
					AssetBundle dependencyAB = dependencyAssetBundles[key];
					dependencyAB.Unload(false);
					lockDic.Remove(key);
				}
			}
			
			

		}*/
		
	}

	IEnumerator loadManifest()
	{
		//1加载Manifest文件
		WWW www ;
		AssetBundleRequest assetRequest;
		string manifestPath = FilePathTools.manifestPath;
		while(lockDic.ContainsKey(manifestPath))
		{
			Debug.Log("正在等待上一个相同资源加载完毕"+manifestPath);
			yield return null;
		}
		lockDic.Add(manifestPath,"AssetBundleManifest");
		www = new WWW(prefix+manifestPath);
		yield return www;
		if (string.IsNullOrEmpty (www.error)) 
		{
			AssetBundle manifestAB = www.assetBundle;
			yield return assetRequest = manifestAB.LoadAssetAsync<AssetBundleManifest> ("AssetBundleManifest");
			manifest = assetRequest.asset as AssetBundleManifest;
			manifestAB.Unload (false);
		}
		lockDic.Remove(manifestPath);
	}
	

	public void clear()
	{
		if(assetBundle!=null)
		{
			assetBundle.Unload(false);
			assetBundle = null;
		}
		
		Obj = null;
	}
	
	private class CacheObject
	{
		
		public Object obj;
		public float time;

		public CacheObject(Object obj,float time)
		{
			this.obj = obj;
			this.time = time;
			
		}
	};
}
