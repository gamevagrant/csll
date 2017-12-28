using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using System;
using LitJson;

/// <summary>
/// 检测是否有可更新版本
/// 版本号形式 A.B.C    A：大版本更新 B：小版本更新 C：修复更新
/// </summary>
public class UpdateVersion {

    /// <summary>
    /// 更新完成 bool 是否更新成功，如果时true 可以继续后面的逻辑，如果为false需要去下载最新安装包
    /// </summary>
    public event Action<bool> onComplate;
    private string lookupUrl;
    private string localVersion;

    public UpdateVersion(Action<bool> onComplate)
    {
        lookupUrl = string.Format("{0}?id={1}", GameSetting.updateLookupUrl, GameSetting.appID);
        localVersion = Application.version;
        this.onComplate = onComplate;
    }

    public void StartUpdate()
    {
#if UNITY_EDITOR
        UpdateAssets();//跳过版本更新
#else
        CheckOutVerson();
#endif
    }

    private void CheckOutVerson()
    {
        HttpProxy.SendGetRequest<LookupMessage>(lookupUrl, (ret, res) =>
        {
            if(res.results.Length>0)
            {
                LookupData lookupData = res.results[0];
                if (lookupData != null && NeedUpdate(localVersion, lookupData.version))
                {
                    if (onComplate != null)
                    {
                        onComplate(false);
                    }
                    string url = lookupData.trackViewUrl;
#if UNITY_IPHONE
                url = url.Replace("https", "itms-apps");    
#endif
                    Application.OpenURL(url);
                }
                else
                {
                    UpdateAssets();
                }
            }else
            {
                UpdateAssets();
            }
           
        });
    }

    private void UpdateAssets()
    {
        if (GameSetting.isUseAssetBundle)
        {
            UpdateAssets updateAsset = new UpdateAssets();
            updateAsset.onComplate += () => {
                if (onComplate != null)
                {
                    onComplate(true);
                }
            };
            updateAsset.StartUpdate();
        }
        else
        {
            if (onComplate != null)
            {
                onComplate(true);
            }
        }
    }

    private bool NeedUpdate(string localVer,string netVer)
    {
        Debug.Log(string.Format("本地版本：{0}|商店版本:{1}",localVer,netVer));
        if(!string.IsNullOrEmpty(localVer) && !string.IsNullOrEmpty(netVer))
        {
            string[] localVerData = localVer.Split('.');
            string[] netVerData = netVer.Split('.');
            if (localVerData.Length == netVerData.Length && localVerData.Length == 3)
            {
                for(int i =0;i<3;i++)
                {
                    int localInt;
                    int netInt;
                    if(int.TryParse(localVerData[i],out localInt) && int.TryParse(netVerData[i],out netInt))
                    {
                        if(localInt<netInt)
                        {
                            Debug.Log("需要更新App");
                            return true;
                        }
                    }
                }
                Debug.Log("不需要更新App");
                return false;
            }
        }

        Debug.Log("需要更新App");
        return true;
    }

    public class LookupMessage:NetMessage
    {
        public int resultCount;
        public LookupData[] results;
    }
    public class LookupData
    {
        public string minimumOsVersion ;         //App所支持的最低iOS系统  
        public string fileSizeBytes ;                 //应用的大小  
        public string releaseDate;                 //发布时间  
        public string trackCensoredName ;           //审查名称        
        public string trackContentRating;          //评级   
        public int trackId;                       //应用程序ID  
        public string trackName ;                   //应用程序名称  
        public string rackViewUrl;                //应用程序介绍网址  
        public string version;                //版本号  
        public string trackViewUrl;                //应用程序介绍网址（用户升级跳转URL）  
    }
}
