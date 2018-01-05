using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloaderManager {



    private static List<string> needPreloaderAssetBundles = new List<string>{
            "/fonts&materials/fzlantyk_cu_sdf",
            "/fonts&materials/fzlantyk_cu_sdf_2",
            "/fonts&materials/fzlantyk_cu_sdf_3",
            "/fonts&materials/fzlantyk_cu_sdf_4",

            "/fonts&materials/fzlantyk_cu_sdf_brownshadow",
            "/fonts&materials/fzlantyk_cu_sdf_shadow",
            "/fonts&materials/fzlantyk_cu_sdf_shadow&redoutline",
            "/fonts&materials/fzlantyk_cu_sdf_whiteoutline",

            "/spriteatlas/uipublicwindow",
            "/spriteatlas/uiwheelwindow",
            "/spriteatlas/uisidebarwindow",
            "/spriteatlas/uitopbarwindow",
        };

    private Dictionary<string, AssetBundle> preloadAssetBundles;

    public PreloaderManager()
    {

        preloadAssetBundles = new Dictionary<string, AssetBundle>();
    }

    public void StartPreloader(MonoBehaviour mono, System.Action onComplate)
    {
        mono.StartCoroutine(Preloader(onComplate));
    }

    public bool Contains(string path)
    {
        return preloadAssetBundles.ContainsKey(path);
    }

    public AssetBundle GetPreloaderAssetBundle(string path)
    {
        if(preloadAssetBundles.ContainsKey(path))
        {
            return preloadAssetBundles[path];
        }
        return null;
    }

    public void RemovePreloaderAssetBundle(string path)
    {
        if (preloadAssetBundles.ContainsKey(path))
        {
            preloadAssetBundles[path].Unload(false);
            preloadAssetBundles.Remove(path);
        }
    }


    private IEnumerator Preloader(System.Action onComplate)
    {
        for (int i =0;i< needPreloaderAssetBundles.Count;i++)
        {
            EventDispatcher.instance.DispatchEvent(new LoadingEvent("Preloader", i / (float)needPreloaderAssetBundles.Count));
            string fileName = needPreloaderAssetBundles[i];
            string path = FilePathTools.downLoadSavePath + fileName.ToLower();
            if(preloadAssetBundles.ContainsKey(path))
            {
                continue;
            }
            Debug.Log("预加载文件："+path);
            AssetBundleCreateRequest createRequest = AssetBundle.LoadFromFileAsync(path);
            yield return createRequest;
            if (createRequest.isDone)
            {
                if(!preloadAssetBundles.ContainsKey(path))
                {
                    preloadAssetBundles.Add(path, createRequest.assetBundle);
                }
                
            }
            else
            {
                Debug.Log("预加载依赖资源出错");
            }
            
        }
        EventDispatcher.instance.DispatchEvent(new LoadingEvent("Preloader", 1));
        onComplate();
    }
}
