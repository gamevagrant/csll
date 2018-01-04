using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Object = UnityEngine.Object;

/// <summary>
/// 用以加载本地AssetBundle加载器，只能用于加载AssetBundle。
/// 当GameSetting.isUseAssetBundle==false时，会使用AssetDatabase.LoadAssetAtPath从项目路径直接加载资源
/// 这样在修改资源后不用重新打包就能看到资源的变化
/// </summary>
public class AssetBundleLoadManager : MonoBehaviour {

    private const int RECOVERY_TIME = 180;//这个时间后没有被请求就自动销毁
    private Dictionary<string, CacheObject> dicCacheObject = new Dictionary<string, CacheObject>();
    private Queue<Action> queue = new Queue<Action>();
    private bool isLoading = false;
    private AssetBundleManifest manifest;
 

    private static AssetBundleLoadManager _instance;

    public static AssetBundleLoadManager Instance
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
        if (queue.Count > 0 && !isLoading)
        {
            Action act = queue.Dequeue();
            if (act != null)
            {
                act();
            }
        }
    }


    private void tryClearCache()
    {
        List<string> list = new List<string>(dicCacheObject.Keys);

        foreach (string key in list)
        {
            CacheObject co = dicCacheObject[key];
            if (Time.time - co.time > RECOVERY_TIME)
            {
                dicCacheObject.Remove(key);
            }

        }

    }

    /// <summary>
    /// 加载一个本地资源（file）
    /// </summary>
    /// <param name="path"></param>
    public void LoadAsset<T>(string url, Action<T> callback) where T : Object
    {
        string path = url;
        path = FilePathTools.normalizePath(path);

        CacheObject co;
        if (callback != null && dicCacheObject.TryGetValue(path, out co) && co!=null)
        {
            callback(co.obj as T);
            co.time = Time.time;
        }else
        {
            queue.Enqueue(() =>
            {
                StartCoroutine(loadAsync<T>(path, callback));
            });
        }
    }

    private IEnumerator loadAsync<T>(string path, Action<T> callback) where T : Object
    {
        CacheObject co;
        if (callback != null && dicCacheObject.TryGetValue(path, out co) && co != null)
        {
            callback(co.obj as T);
            co.time = Time.time;
            yield break;
        }

        isLoading = true;

        path = FilePathTools.normalizePath(path);
        Debug.Log("============start loadAsync:AssetBundleLoader.loadAsync" + path);
        string assetBundleName = FilePathTools.getAssetBundleNameWithPath(path);

        bool isUseAssetBundle = GameSetting.isUseAssetBundle;
        if (!isUseAssetBundle)
        {
#if UNITY_EDITOR
			path = FilePathTools.getRelativePath(path);
			Object Obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T));


			if (Obj == null)
			{
				Debug.LogError ("Asset not found at path:" + path);
			}
            callback((T)Obj);
#endif
        }
        else
        {
            //打的ab包都资源名称和文件名都是小写的
            //path = Path.GetDirectoryName(path)+"/"+Path.GetFileNameWithoutExtension(path).ToLower();
            AssetBundleRequest assetRequest;
            AssetBundleCreateRequest createRequest;
            //1加载Manifest文件
            if (manifest == null)
            {
                string manifestPath = FilePathTools.manifestPath;
                Debug.Log("---start load Manifest " + manifestPath);
                createRequest = AssetBundle.LoadFromFileAsync( manifestPath);
                yield return createRequest;
                if (createRequest.isDone)
                {
                    AssetBundle manifestAB = createRequest.assetBundle;
                    yield return assetRequest = manifestAB.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
                    manifest = assetRequest.asset as AssetBundleManifest;
                    manifestAB.Unload(false);

                }
                else
                {
                    Debug.Log("---Manifest加载出错");
                }

            }


            //2获取文件依赖列表
            string[] dependencies = manifest.GetAllDependencies(assetBundleName);

            //3加载依赖资源
            Dictionary<string, AssetBundle> dependencyAssetBundles = new Dictionary<string, AssetBundle>();
            //Debug.Log("---开始加载依赖资源:" + dependencies.Length.ToString());
            foreach (string fileName in dependencies)
            {
                string dependencyPath = FilePathTools.root + "/" + fileName;
                if(!GameMainManager.instance.preloader.Contains(dependencyPath))
                {
                    Debug.Log("---开始加载依赖资源:" + dependencyPath);

                    createRequest = AssetBundle.LoadFromFileAsync(dependencyPath);
                    yield return createRequest;
                    if (createRequest.isDone)
                    {
                        dependencyAssetBundles.Add(dependencyPath, createRequest.assetBundle);
                       
                    }
                    else
                    {
                        Debug.Log("加载依赖资源出错");
                    }
                }
               

            }
            //4加载目标资源
            Object obj = null;
            //Debug.Log("---开始加载目标资源:" + path);
            //www = new WWW(prefix + path);
            createRequest = AssetBundle.LoadFromFileAsync(path);
            yield return createRequest;
            if (createRequest.isDone)
            {
                AssetBundle assetBundle = createRequest.assetBundle;
                yield return assetRequest = assetBundle.LoadAssetAsync(Path.GetFileNameWithoutExtension(path), typeof(T));
                obj = assetRequest.asset;

                addCache(path,obj);
                //5释放目标资源
                //Debug.Log("---释放目标资源:" + path);
                assetBundle.Unload(false);
                assetBundle = null;
            }
            else
            {
                Debug.Log("加载目标资源出错 ");
            }


         
            if (dependencyAssetBundles != null)
            {
                //6释放依赖资源
                foreach (string key in dependencyAssetBundles.Keys)
                {
                    //Debug.Log("---释放依赖资源:" + key);
                    AssetBundle dependencyAB = dependencyAssetBundles[key];
                    dependencyAB.Unload(false);
                }
            }

            tryClearCache();
            callback((T)obj);
        }
        //Debug.Log("---end loadAsync:AssetBundleLoader.loadAsync" + path);
        yield return null;
        isLoading = false;
    }

    private void addCache(string path ,Object obj)
    {
        if (!dicCacheObject.ContainsKey(path))
        {
            dicCacheObject.Add(path, new CacheObject(obj, Time.time));
        }
    }

    private class CacheObject
    {

        public Object obj;
        public float time;

        public CacheObject(Object obj, float time)
        {
            this.obj = obj;
            this.time = time;

        }
    };

}
