using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UpdateAssets {

    public event System.Action onComplate;

    private string loadRootPath;//远程资源地址
    private string saveRootPath;//本地存储地址
    private string verMainName = "/Version_Main.txt";
    private AssetBundleManifest oldManifest;
    private AssetBundleManifest newManifest;
    

    public UpdateAssets()
    {
        this.loadRootPath = FilePathTools.downLoadRootPath;
        this.saveRootPath = FilePathTools.downLoadSavePath;
    }

    public void StartUpdate()
    {
        Debug.Log("==开始更新==");
        Debug.Log("下载根目录：" + loadRootPath);
        Debug.Log("保存根目录：" + saveRootPath);

        AssetBundle manifestAB;
        if (File.Exists(FilePathTools.manifestPath))
        {
            manifestAB = AssetBundle.LoadFromFile(FilePathTools.manifestPath);
            oldManifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            manifestAB.Unload(false);
        }else
        {
            Debug.Log("-本地没有版本文件-"+ FilePathTools.manifestPath);
        }
       
        //GameMainManager.instance.mono.StartCoroutine(loadMainVerson());
        //下载主版本文件
        string path = FilePathTools.GetDownLoadMainVersonPath(verMainName);
        AssetLoadManager.Instance.LoadAsset<byte[]>(path, (bytes) => {

            Debug.Log("-下载远程版本文件成功-" + path);

            writeFile(saveRootPath +"/"+Path.GetFileName(saveRootPath), bytes);

            manifestAB = AssetBundle.LoadFromMemory(bytes);
            newManifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            manifestAB.Unload(false);
            UpdateFiles();
        });
    }


    private void UpdateFiles()
    {
        List<string> updateFileNames = GetUpdateFileName();
        int allCount = updateFileNames.Count;
        int count = 0;
        Debug.Log("-更新资源-:"+allCount);
        if (allCount == 0 && onComplate != null)
        {
            Debug.Log("-没有需要更新的资源-");
            EventDispatcher.instance.DispatchEvent(new LoadingEvent("UpdateAssets", 1));
            onComplate();
        }

        foreach (string fileName in updateFileNames)
        {
            string path = loadRootPath + "/" + fileName;
            AssetLoadManager.Instance.LoadAsset<byte[]>(path, (bytes) => {
                count++;
                writeFile(saveRootPath + "/" + fileName, bytes);
                Debug.Log("写入" + fileName + "成功:" + saveRootPath + "/" + fileName);
                EventDispatcher.instance.DispatchEvent(new LoadingEvent("UpdateAssets", (float)count / allCount, fileName));
                if (count >= allCount && onComplate != null)
                {
                    onComplate();
                }
            });
        }

    }

    /// <summary>
    /// 获取需要更新的文件
    /// </summary>
    /// <returns></returns>
    private List<string> GetUpdateFileName()
    {
        if(oldManifest == null )
        {
            if(newManifest != null)
            {
                return new List<string>(newManifest.GetAllAssetBundles());
            }
            else
            {
                return new List<string>();
            }
            
        }

        List<string> updateFileNames = new List<string>();
        int newHashCode = newManifest.GetHashCode();
        int oldHashCode = oldManifest.GetHashCode();
        if(newHashCode == oldHashCode)
        {
            updateFileNames = new List<string>();
        }else
        {
            string[] newAssets = newManifest.GetAllAssetBundles();
            string[] oldAssets = oldManifest.GetAllAssetBundles();
            Dictionary<string, Hash128> oldHashs = new Dictionary<string, Hash128>();
            foreach(string name in oldAssets)
            {
                oldHashs.Add(name,oldManifest.GetAssetBundleHash(name));
            }
            foreach(string name in newAssets)
            {
                if(oldHashs.ContainsKey(name))
                {
                    Hash128 newHash = newManifest.GetAssetBundleHash(name);
                    if(newHash!=oldHashs[name])
                    {
                        updateFileNames.Add(name);
                    }
                }else
                {
                    updateFileNames.Add(name);
                }
            }
        }
        return updateFileNames;
    }

    //写文件，会覆盖同名文件
    static void writeFile(string path, byte[] data)
    {
        FileInfo fi = new FileInfo(path);
        DirectoryInfo dir = fi.Directory;
        if (!dir.Exists)
        {
            dir.Create();
        }

        FileStream fs = fi.Create();
        fs.Write(data, 0, data.Length);
        //清空缓冲区、关闭流
        fs.Flush();
        fs.Close();
    }
}
