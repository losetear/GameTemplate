using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text;

public class Md5MenuItems
{
    const string kAssetBundlesPath = "/Assets/StreamingAssets/AssetBundles/";

    //[MenuItem("AssetBundles/GenMd5")]
    public static void GenMd5()
    {
        StringBuilder fileInfoes = new StringBuilder();
        string targetPath = GetPath();
        if (!Directory.Exists(targetPath))
        {
            Debug.LogError("没有需要生成md5的文件！");
            return;
        }

        string[] fileNames = Directory.GetFiles(targetPath, "*", SearchOption.AllDirectories);  
        foreach (string fileName in fileNames)
        {
            string fileNameShort = fileName.Substring(targetPath.Length);

            if (fileNameShort.Contains("version.txt") || fileNameShort.Contains(".meta"))
                continue;

            string md5 = GetMD5HashFromFile(fileName);

            fileInfoes.Append(fileNameShort.Replace("\\", "/")).Append(",").Append(md5).Append("\n");
        }
        saveFile(fileInfoes);
    }

    private static void saveFile(StringBuilder fileInfoes)
    {
        string targetPath = GetPath() + "version.txt";
        FileStream fs = new FileStream(targetPath, FileMode.Create);
        byte[] data = new UTF8Encoding().GetBytes(fileInfoes.ToString());
        fs.Write(data, 0, data.Length);
        fs.Flush();
        fs.Close();
    }

    public static string GetPath()
    {
        string relativePath = System.Environment.CurrentDirectory.Replace("\\", "/");
        string kAssetBundlesPath = "/Assets/StreamingAssets/AssetBundles/";
        string platformFolderForAssetBundles = GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
        return relativePath + kAssetBundlesPath + platformFolderForAssetBundles + "/";
    }

    public static string GetPlatformFolderForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.WebPlayer:
                return "WebPlayer";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "OSX";
            default:
                return null;
        }
    }

    private static string GetMD5HashFromFile(string fileName)
    {
        using (FileStream file = new FileStream(fileName, FileMode.Open))
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(file);

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("x2"));
            }
            return result.ToString();
        }
    }
}
