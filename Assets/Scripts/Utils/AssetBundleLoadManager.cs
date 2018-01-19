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

    private const int RECOVERY_TIME = 120;//这个时间后没有被请求就自动销毁
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
                TryClearCache();
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
        Debug.Log("===AssetBundleLoader.loadAsync:" + path);
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
            AssetBundleRequest assetRequest;
            AssetBundleCreateRequest createRequest;
            //1加载Manifest文件
            if (manifest == null)
            {
                string manifestPath = FilePathTools.manifestPath;
                Debug.Log("start load Manifest:" + manifestPath);
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
                    Debug.Log("Manifest加载出错");
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
                    Debug.Log("开始加载依赖资源:" + dependencyPath);

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
            Debug.Log("---开始加载目标资源:" + path);
            createRequest = AssetBundle.LoadFromFileAsync(path);
            yield return createRequest;
            List<AssetBundle> abList = new List<AssetBundle>();
            if (createRequest.isDone)
            {
                AssetBundle assetBundle = createRequest.assetBundle;
                yield return assetRequest = assetBundle.LoadAssetAsync(Path.GetFileNameWithoutExtension(path), typeof(T));
                obj = assetRequest.asset;

                AddCache(path,obj);
                //5释放目标资源
                //Debug.Log("---释放目标资源:" + path);
                abList.Add(assetBundle);

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
                    abList.Add(dependencyAB);
                }
            }

            
            callback((T)obj);
            StartCoroutine(UnloadAssetbundle(abList));
        }
        //Debug.Log("---end loadAsync:AssetBundleLoader.loadAsync" + path);
        yield return null;
        isLoading = false;
    }

    public T Load<T>(string path) where T : Object
    {
        CacheObject co;
        if (dicCacheObject.TryGetValue(path, out co) && co != null)
        {

            co.time = Time.time;
            return (T)co.obj;
        }

        path = FilePathTools.normalizePath(path);
        Debug.Log("===AssetBundleLoader.Load:" + path);
        

        if (!GameSetting.isUseAssetBundle)
        {
#if UNITY_EDITOR
            path = FilePathTools.getRelativePath(path);
            Object Obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T));


            if (Obj == null)
            {
                Debug.LogError("Asset not found at path:" + path);
            }
            return (T)Obj;
#endif
        }
        else
        {
            //打的ab包都资源名称和文件名都是小写的

            //1加载Manifest文件
            if (manifest == null)
            {
                string manifestPath = FilePathTools.manifestPath;
                Debug.Log("start load Manifest:" + manifestPath);

                AssetBundle manifestAB = AssetBundle.LoadFromFile(manifestPath);
                manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                manifestAB.Unload(false);

            }


            //2获取文件依赖列表
            string assetBundleName = FilePathTools.getAssetBundleNameWithPath(path);
            string[] dependencies = manifest.GetAllDependencies(assetBundleName);

            //3加载依赖资源
            Dictionary<string, AssetBundle> dependencyAssetBundles = new Dictionary<string, AssetBundle>();
            //Debug.Log("---开始加载依赖资源:" + dependencies.Length.ToString());
            foreach (string fileName in dependencies)
            {
                string dependencyPath = FilePathTools.root + "/" + fileName;
                if (!GameMainManager.instance.preloader.Contains(dependencyPath))
                {
                    Debug.Log("开始加载依赖资源:" + dependencyPath);

                    dependencyAssetBundles.Add(dependencyPath, AssetBundle.LoadFromFile(dependencyPath));
                }


            }
            //4加载目标资源
            Object obj = null;
            Debug.Log("---开始加载目标资源:" + path);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            obj = assetBundle.LoadAsset<T>(Path.GetFileNameWithoutExtension(path)); ;

            AddCache(path, obj);
            //5释放目标资源
            //Debug.Log("---释放目标资源:" + path);
            List<AssetBundle> abList = new List<AssetBundle>();
            abList.Add(assetBundle);



            if (dependencyAssetBundles != null)
            {
                //6释放依赖资源
                foreach (string key in dependencyAssetBundles.Keys)
                {
                    //Debug.Log("---释放依赖资源:" + key);
                    AssetBundle dependencyAB = dependencyAssetBundles[key];
                    abList.Add(dependencyAB);
                }
            }
            StartCoroutine(UnloadAssetbundle(abList));

            return (T)obj;
        }
        return null;
    }

    private IEnumerator UnloadAssetbundle(List<AssetBundle> list)
    {
        //为了解决在ios上同步加载后直接释放造成加载出来的资源被回收的问题，需要隔一帧再释放
        yield return null;
        for(int i =0;i<list.Count;i++)
        {
            list[i].Unload(false);
        }
        
    }

    private void AddCache(string path ,Object obj)
    {
        if (!dicCacheObject.ContainsKey(path))
        {
            dicCacheObject.Add(path, new CacheObject(obj, Time.time));
        }
    }
    private void TryClearCache()
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
    private class CacheObject
    {

        public Object obj;
        public float time;

        public CacheObject(Object obj, float time)
        {
            this.obj = obj;
            this.time = time;

        }
    }

}
