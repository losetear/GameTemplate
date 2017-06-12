using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildTools : ScriptableObject
{
    [MenuItem("打包/打包测试资源")]
    public static void BuildTestAssetBundles()
    {
        DeleteFolder(Application.dataPath + "/StreamingAssets");
        AssetBundleName.MarkTestAssetBundle();
        BuildScript.BuildAssetBundles();

        string toolPath = Application.dataPath + "/../../../tools/TemporaryTools";
        string srcPath = Application.dataPath + "/StreamingAssets/AssetBundles/";
        string destPath = Application.dataPath + "/../../../Server/AssetbundleRes/AssetbundleRes/" + AssetBundleTag.Test;
        DeleteFolder(destPath);
        CopyDirectory(srcPath, destPath);
        CopyDirectory(toolPath, destPath);//将MD5工具也一并拷贝到制定目录
        AssetDatabase.Refresh();

        Process ps = new Process { StartInfo = { FileName = destPath + "/FileMd5Gen.exe" } };
        ps.Start();
    }


//    [MenuItem("打包/打包动物资源")]
//    public static void BuildDongwuAssetBundles()
//    {
//        DeleteFolder(Application.dataPath + "/StreamingAssets");
//        AssetBundleName.MarkDongwuAssetBundle();
//        BuildScript.BuildAssetBundles();
//
//        string toolPath = Application.dataPath + "/../../../tool/TemporaryTools";
//        string srcPath = Application.dataPath + "/StreamingAssets/AssetBundles/";
//        string destPath = Application.dataPath + "/../../../Server/AssetbundleRes/AssetbundleRes/" + AssetBundleTag.Dongwu;
//        DeleteFolder(destPath);
//        CopyDirectory(srcPath, destPath);
//        CopyDirectory(toolPath, destPath);//将MD5工具也一并拷贝到制定目录
//        AssetDatabase.Refresh();
//
//        Process ps = new Process { StartInfo = { FileName = destPath + "/FileMd5Gen.exe" } };
//        ps.Start();
//    }
//
//    [MenuItem("打包/打包Hero资源")]
//    public static void BuildHeroAssetBundles()
//    {
//        DeleteFolder(Application.dataPath + "/StreamingAssets");
//        AssetBundleName.MarkHeroAssetBundle();
//        BuildScript.BuildAssetBundles();
//
//        string toolPath = Application.dataPath + "/../../../tool/TemporaryTools";
//        string srcPath = Application.dataPath + "/StreamingAssets/AssetBundles/";
//        string destPath = Application.dataPath + "/../../../Server/AssetbundleRes/AssetbundleRes/" + AssetBundleTag.Hero;
//        DeleteFolder(destPath);
//        CopyDirectory(srcPath, destPath);
//        CopyDirectory(toolPath, destPath);//将MD5工具也一并拷贝到制定目录
//        AssetDatabase.Refresh();
//
//        Process ps = new Process { StartInfo = { FileName = destPath + "/FileMd5Gen.exe" } };
//        ps.Start();
//    }
//
//    [MenuItem("打包/打包换装资源")]
//    public static void BuildChangeClothingAssetBundles()
//    {
//        DeleteFolder(Application.dataPath + "/StreamingAssets");
//        AssetBundleName.MarkChangeClothingAssetBundle();
//        BuildScript.BuildAssetBundles();
//
//        string toolPath = Application.dataPath + "/../../../tool/TemporaryTools";
//        string srcPath = Application.dataPath + "/StreamingAssets/AssetBundles/";
//        string destPath = Application.dataPath + "/../../../Server/AssetbundleRes/AssetbundleRes/" + AssetBundleTag.ChangeClothing;
//        DeleteFolder(destPath);
//        CopyDirectory(srcPath, destPath);
//        CopyDirectory(toolPath, destPath);//将MD5工具也一并拷贝到制定目录
//        AssetDatabase.Refresh();
//
//        Process ps = new Process { StartInfo = { FileName = destPath + "/FileMd5Gen.exe" } };
//        ps.Start();
//    }

    static void DeleteFolder(string dir)
    {
        if (!Directory.Exists(dir))
            return;
        foreach (string d in Directory.GetFileSystemEntries(dir))
        {
            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(d);
            }
            else
            {
                DirectoryInfo d1 = new DirectoryInfo(d);
                if (d1.GetFiles().Length != 0)
                {
                    DeleteFolder(d1.FullName);////递归删除子文件夹
                }
                if (Directory.Exists(d))
                    Directory.Delete(d);
            }
        }
        if (Directory.Exists(dir))
            Directory.Delete(dir, true);
    }

    public static void CopyDirectory(string sourcePath, string destinationPath)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        Directory.CreateDirectory(destinationPath);
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);
            if (fsi is FileInfo)
                File.Copy(fsi.FullName, destName);
            else
            {
                Directory.CreateDirectory(destName);
                CopyDirectory(fsi.FullName, destName);
            }
        }
    }
}
