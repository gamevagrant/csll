using UnityEngine;
using System.Collections;

public class GameSetting {


    //------------------全平台一致的配置--------------------------
    public static string serverPath = "http://api.nutsgamer.com";
    public static string serverPathDevelop = "https://api.dev.nutsgamer.com";
    public static string websocketPath = "wss://api.nutsgamer.com/ws/conn";
    public static string websocketPathDevelop = "wss://api.dev.nutsgamer.com/ws/conn";

    public static bool isRelease = false;//是否时发布版本 是使用正式服务器还是测试服务器
    //-------------------平台区分的配置-------------------------
#if UNITY_EDITOR
    public static readonly bool isUseAssetBundle =  false;//true：使用assetbundle包中的资源,资源有变化需要重新打包，false：使用编辑器里的资源，资源更改随时生效
    public static readonly bool isUseLocalAssetBundle = true;//是否使用本地資源 使用本地资源会去streamAsset文件夹下加载，不使用会从网络加载
    public static readonly bool isDebug = true;//使用测试服务器 显示测试UI，帧率
#else
	public static readonly bool isUseAssetBundle = true;//这条勿动，发布平台下永远是true
    public static readonly bool isUseLocalAssetBundle = true;//是否使用本地資源 使用本地资源会去streamAsset文件夹下加载，不使用会从网络加载
	public static readonly bool isDebug = true ;//使用测试服务器 显示测试UI，帧率等
#endif


}
