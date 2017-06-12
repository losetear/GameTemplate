using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

public static class AssetBundleTag
{
    public const string Dongwu = "Dongwu";
    public const string Hero = "Hero";
    public const string ChangeClothing = "ChangeClothing";
    public const string Test = "Test";
}
public class AssetBundleName : ScriptableObject
{
    private const string VariantTag = "lijv";

    public static void MarkTestAssetBundle()
    {
        ClearAllMark();
        MarkAssetBundleByTag(AssetBundleTag.Test);
    }

    //[MenuItem("打包/标记资源/标记动物资源")]
    public static void MarkDongwuAssetBundle()
    {
        ClearAllMark();
        MarkAssetBundleByTag(AssetBundleTag.Dongwu);
    }

    //[MenuItem("打包/标记资源/MarkSanguo")]
    public static void MarkHeroAssetBundle()
    {
        ClearAllMark();
        MarkAssetBundleByTag(AssetBundleTag.Hero);
    }

    //[MenuItem("打包/标记资源/ChangeClothing")]
    public static void MarkChangeClothingAssetBundle()
    {
        ClearAllMark();
        MarkAssetBundleByTag(AssetBundleTag.ChangeClothing);
    }

    [MenuItem("打包/清除所有标记")]
    static void ClearAllMark()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/AssetBundles", "*.*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string assetPath = GetAssetPath(file);

            AssetImporter ai = AssetImporter.GetAtPath(assetPath);
            if (!ai || string.IsNullOrEmpty(ai.assetBundleName)) continue;
            ai.assetBundleName = "";
        }
    }

    static void MarkAssetBundleByTag(string tag)
    {
        var files = Directory.GetFiles(Application.dataPath + "/AssetBundles/" + tag, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".prefab") || s.EndsWith(".mp3") || s.EndsWith(".wav"));
        foreach (var file in files)
        { 
            string assetPath = GetAssetPath(file);

            AssetImporter ai = AssetImporter.GetAtPath(assetPath);
            string assetBundleName = GetBundleName(assetPath);
            if (!assetBundleName.Contains(tag)) continue;
            ai.assetBundleName = assetBundleName;
            ai.assetBundleVariant = VariantTag;
        }
    }

    static string GetBundleName(string path)
    {
        path = path.Replace("Assets/AssetBundles/", "");
        int endIndex = path.LastIndexOf("/", StringComparison.Ordinal);
        var assetBundleName = path.Substring(0, endIndex);
        return assetBundleName;
    }

    static string GetAssetPath(string filePath)
    {
        string curFile = filePath.Replace("\\", "/");
        int startIndex = curFile.IndexOf("Assets", StringComparison.Ordinal);
        string path = curFile.Substring(startIndex, curFile.Length - startIndex);
        return path;
    }
}
