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
    /// 检测完成可以继续
    /// </summary>
    public event Action onComplate;
    /// <summary>
    /// 需要更新
    /// </summary>
    public event Action<string> onNeedUpdate;
    private string lookupUrl;
    private string localVersion;

    public UpdateVersion(Action onComplate,Action<string> onNeedUpdate)
    {
        lookupUrl = string.Format("{0}?id={1}", GameSetting.updateLookupUrl, GameSetting.appID);
        localVersion = Application.version;
        this.onComplate = onComplate;
        this.onNeedUpdate = onNeedUpdate;
    }

    public void StartUpdate()
    {
        CheckOutVerson();
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

                    string url = lookupData.trackViewUrl;
#if UNITY_IPHONE
                url = url.Replace("https", "itms-apps");    
#endif
                   if(onNeedUpdate!=null)
                   {
                        onNeedUpdate(url);
                   }
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
                    onComplate();
                }
            };
            updateAsset.StartUpdate();
        }
        else
        {
            if (onComplate != null)
            {
                onComplate();
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
            }
        }

        Debug.Log("不需要更新App");
        return false;
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
