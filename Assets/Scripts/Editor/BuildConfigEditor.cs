using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;
using System;
using System.Text.RegularExpressions;

public class BuildConfigEditor :Editor {

    [MenuItem("BuildConfig/生成随机名字配置")]
    static void BuildRandomNameConfig()
    {
        string path = "Assets/ArtResources/csv/randomNamesConfig.csv";
        string exportPath = "Assets/Export/Configs/randomNamesConfig.txt";
        string str = AssetDatabase.LoadMainAssetAtPath(path).ToString();

        Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
        string[] lineArray = Regex.Split(str, @"\r\n", RegexOptions.IgnoreCase);
        string[] keys = lineArray[0].Split(',');
        foreach (string key in keys)
        {
            data[key] = new List<string>();
        }
        for (int i = 1; i < lineArray.Length; i++)
        {
            if (lineArray[i] != "")
            {
                string[] strs = lineArray[i].Split(',');
                for (int j = 0; j < strs.Length; j++)
                {
                    data[keys[j]].Add(strs[j]);
                }

            }
        }

        string json = JsonMapper.ToJson(data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        var ss = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

        WriteFile(exportPath, ss);
    }

    [MenuItem("BuildConfig/生成错误描述配置")]
    public static void BuildErrorDescribeConfig()
    {
        string path = "Assets/ArtResources/csv/errorDescribeConfig.csv";
        string exportPath = "Assets/Export/Configs/errorDescribeConfig.txt";
        string str = AssetDatabase.LoadMainAssetAtPath(path).ToString();

        List<Dictionary<string, string>> data = ReadCSV(str);
        Dictionary<string, string> dic = new Dictionary<string, string>();
        foreach(Dictionary<string,string> item in data)
        {
            dic.Add(item["id"],item["describe"]);
        }

        string json = JsonMapper.ToJson(new Dictionary<string, object>() { { "data", dic } });
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        var ss = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        WriteFile(exportPath, ss);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(exportPath);
    }

    public static List<Dictionary<string, string>> ReadCSV(string csvData)
    {
        List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        string[] lineArray = Regex.Split(csvData, @"\r\n", RegexOptions.IgnoreCase);
        string[] keys = lineArray[0].Split(',');
        for (int i = 1; i < lineArray.Length; i++)
        {
            if (lineArray[i] != "")
            {
                Dictionary<string, string> line = new Dictionary<string, string>();
                string[] strs = lineArray[i].Split(',');
                for (int j = 0; j < strs.Length; j++)
                {
                    line.Add(keys[j], strs[j]);
                }
                data.Add(line);
            }
        }

        return data;
    }

    static void WriteFile(string path, string str)
    {
        FileInfo fi = new FileInfo(path);
        DirectoryInfo dir = fi.Directory;
        if (!dir.Exists)
        {
            dir.Create();
        }

        FileStream fs = new FileStream(path, FileMode.Create);//文本加入不覆盖
        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);//转码

        sw.WriteLine(str);
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
    }
}
