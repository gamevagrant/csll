using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FilePathTools : MonoBehaviour {



	public static string assetsRootPath = Application.dataPath + "/Export";//资源文件夹绝对路径

	public static string exportRoot = Application.streamingAssetsPath;//打包资源的输出文件夹

	public static string root
	{
		get
		{

			return exportRoot;
		}
	}

	/// <summary>
	/// 获取文件夹的所有文件，包括子文件夹 不包含.meta文件
	/// </summary>
	/// <returns>The files.</returns>
	/// <param name="path">Path.</param>
	public static FileInfo[] getFiles(string path)
	{
		DirectoryInfo folder = new DirectoryInfo(path);

		DirectoryInfo[] subFolders = folder.GetDirectories();
		List<FileInfo> filesList = new List<FileInfo>();
	
		foreach (DirectoryInfo subFolder in subFolders)
		{
			filesList.AddRange(getFiles(subFolder.FullName));
		}

		FileInfo[] files = folder.GetFiles();
		foreach (FileInfo file in files)
		{
			if (file.Extension != ".meta")
			{
				filesList.Add(file);
			}

		}
		return filesList.ToArray();
	}

	public static string[] getFilesPath(string path)
	{
		DirectoryInfo folder = new DirectoryInfo(path);
		DirectoryInfo[] subFolders = folder.GetDirectories();
		List<string> filesList = new List<string>();

		foreach (DirectoryInfo subFolder in subFolders)
		{
			filesList.AddRange(getFilesPath(subFolder.FullName));
		}

		FileInfo[] files = folder.GetFiles();
		foreach (FileInfo file in files)
		{
			if (file.Extension != ".meta")
			{
				filesList.Add(normalizePath(file.FullName));
			}

		}
		return filesList.ToArray();
	}

	//创建文件目录前的文件夹，保证创建文件的时候不会出现文件夹不存在的情况
	public static void createFolderByFilePath(string path)
	{
		FileInfo fi = new FileInfo(path);
		DirectoryInfo dir = fi.Directory;
		if (!dir.Exists)
		{
			dir.Create();
		}
	}

	public static void createFolder(string path)
	{
		DirectoryInfo dir = new DirectoryInfo(path);
		if (!dir.Exists)
		{
			dir.Create();
		}
	}

	//规范化路径名称 修正路径中的正反斜杠
	public static string normalizePath(string path)
	{
		return path.Replace(@"\", "/");
	}
	//将绝对路径转成工作空间内的相对路径
	public static string getRelativePath(string fullPath)
	{
		string path = normalizePath(fullPath);
		//path = path.Replace(Application.dataPath,"Assets");
		path = replaceFirst(path, Application.dataPath, "Assets");
		return path;
	}
	//将相对路径转成绝对路径
	public static string getAbsolutelyPath(string relativePath)
	{
		string path = normalizePath(relativePath);
		//path = Application.dataPath.Replace("Assets","") + path;
		path = replaceFirst(Application.dataPath, "Assets", "") + path;
		return path;
	}

	//替换掉第一个遇到的指定字符串
	public static string replaceFirst(string str, string oldValue, string newValue)
	{
		int i = str.IndexOf(oldValue);
		str = str.Remove(i, oldValue.Length);
		str = str.Insert(i, newValue);
		return str;
	}


}
