using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloaderManager {

    private static PreloaderManager _instance;
    public static PreloaderManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new PreloaderManager();
            }
            return _instance;
        }
    }

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
        };

    private Dictionary<string, AssetBundle> preloadAssetBundles;

    public PreloaderManager()
    {
        if(PreloaderManager._instance==null)
        {
            PreloaderManager._instance = this;
        }
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



    private IEnumerator Preloader(System.Action onComplate)
    {
        foreach (string fileName in needPreloaderAssetBundles)
        {
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

        onComplate();
    }
}
