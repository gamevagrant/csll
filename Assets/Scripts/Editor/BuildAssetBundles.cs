using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
/// <summary>
///资源打包类  全自动打包 使用assetBundleLoader 加载,使用filePathTools获取地址
/// </summary>
public class BuildAssetBundles {

	#if UNITY_ANDROID
	static BuildTarget buildTarget = BuildTarget.Android;
	#elif UNITY_IPHONE
	static BuildTarget buildTarget = BuildTarget.iOS;
	#else
	static BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
	#endif

	static string buildRootPath = FilePathTools.assetsRootPath;//需要打包的文件夹的根目录
	static string exportPath = FilePathTools.exportRoot;//assetbundle输出路径

	[@MenuItem("Build/提取依赖资源并打包")]
	public static void buildAllAsset()
	{
		AssetBundleBuild[] buildMap = getBuildFileList (buildRootPath);
		FilePathTools.createFolder (exportPath);
		BuildPipeline.BuildAssetBundles (exportPath,buildMap,BuildAssetBundleOptions.DeterministicAssetBundle,buildTarget);
	}
    [@MenuItem("Build/直接打包所有资源")]
    public static void TestBuildAllAsset()
    {
        AssetBundleBuild[] buildMap = GetBuildFileListNew(buildRootPath);
        FilePathTools.createFolder(exportPath);
        BuildPipeline.BuildAssetBundles(exportPath,buildMap, BuildAssetBundleOptions.DeterministicAssetBundle, buildTarget);
    }
    
    static AssetBundleBuild[] GetBuildFileListNew(string buildRoot)
    {
        //获取所有固定打包的文件
        FileInfo[] files = FilePathTools.getFiles(buildRoot);
        //List<string> fixedPaths = new List<string>();
        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        for (int i = 0; i < files.Length; i++)
        {
            //获取相对于asset目录的相对路径
            string path = FilePathTools.getRelativePath(files[i].FullName);

            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = getAssetBundleNameWithPath(path);
            build.assetNames = new string[1] { path };
            buildMap.Add(build);
        }


        return buildMap.ToArray();
    }

    //获取所有需要打包的文件
    static AssetBundleBuild[] getBuildFileList(string buildRoot)
	{
		//获取所有固定打包的文件
		FileInfo[] files = FilePathTools.getFiles (buildRoot);
		List<string> fixedPaths = new List<string>();//固定打包资源（必定被打包的资源）
		for(int i = 0;i<files.Length;i++)
		{
			//获取相对于asset目录的相对路径
			fixedPaths.Add( FilePathTools.getRelativePath(files[i].FullName));
		}
		//找出固定打包的文件以及其所有依赖文件 排重
		string[] allDependencies = AssetDatabase.GetDependencies (fixedPaths.ToArray(),true);//所有的依赖关系，包括自己并已排重

		//寻找固定打包文件和依赖文件的第一层依赖关系并进行依赖计数
		Dictionary<string,int> dependenciesCount = new Dictionary<string, int> ();//依赖计数用字典
		foreach(string path in allDependencies)
		{
            if (Path.GetExtension(path) == ".spriteatlas")
                continue;

			string[] dependencie = AssetDatabase.GetDependencies (path,false);
			foreach(string p in dependencie)
			{
				if(!dependenciesCount.ContainsKey(p))
				{
					dependenciesCount.Add(p,1);
				}else
				{
					dependenciesCount[p]++;
				}
			}
		}
		//将计数器大于1的资源提取出来，剔除掉固定打包的资源
		List<AssetBundleBuild> buildMap = new List<AssetBundleBuild> ();
		List<string> dependenciesPaths = new List<string> ();//被引用次数大于1的依赖资源 需要进行打包
		foreach(string key in dependenciesCount.Keys)
		{
			int count = dependenciesCount[key];
			if(count>1 && !isFixedBuildAsset(key))
			{
				dependenciesPaths.Add(key);

			}
		}
		//合并固定打包资源和依赖打包资源
		List<string> allBuildPaths = new List<string> (fixedPaths);
		allBuildPaths.AddRange (dependenciesPaths);

		foreach(string path in allBuildPaths)
		{
			//去掉脚本资源
			if(Path.GetExtension(path)== ".cs")
			{
				continue;
			}

			AssetBundleBuild build = new AssetBundleBuild();
			build.assetBundleName = getAssetBundleNameWithPath(path);
			build.assetNames = new string[1]{path};
			buildMap.Add(build);
			Debug.Log(build.assetBundleName+" | " + build.assetNames[0]);
		}
		
		return buildMap.ToArray();
	}
	
	/// <summary>
	/// 通过相对地址获取assetbundle的名字
	/// </summary>
	/// <returns>The asset bundle name with path.</returns>
	/// <param name="path">相对地址.</param>
	static string getAssetBundleNameWithPath(string path)
	{
		string p = Path.GetDirectoryName (path)+ "/" + Path.GetFileNameWithoutExtension (path);
		//判断是依赖资源还是固定资源
		if(!isFixedBuildAsset(p))
		{
			p = FilePathTools.replaceFirst(p,"Assets","Dependencie");
			//p = p.Replace ("Assets","Dependencie");
		}else
		{
			p = FilePathTools.replaceFirst(p,FilePathTools.getRelativePath(buildRootPath) + "/","");
			//p = p.Replace (buildRoot + "/","");
		}
		return p;
	}

	/// <summary>
	/// 判断是不是固定打包资源 
	/// </summary>
	/// <returns><c>true</c>, if fixed build asset was ised, <c>false</c> otherwise.</returns>
	/// <param name="path">相对地址.</param>
	static bool isFixedBuildAsset(string path)
	{
		if (path.IndexOf (FilePathTools.getRelativePath(buildRootPath)) == -1) 
		{
			return false;
		}
		return true;
	}



}
