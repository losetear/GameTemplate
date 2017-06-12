using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ar.platform;
using CSObjectWrapEditor;
using UnityEditor;
using UnityEngine;

public class BuildTool : ScriptableObject
{
    #region Common

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
                    Directory.Delete(d,true);
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
            {
                if (File.Exists(destName))
                {
                    File.Delete(destName);
                }
                if (!fsi.FullName.Contains(".meta"))
                    File.Copy(fsi.FullName, destName);
            }
            else
            {
                Directory.CreateDirectory(destName);
                CopyDirectory(fsi.FullName, destName);
            }
        }
    }
    #endregion

    #region OtherSet
    [MenuItem("打包/设置/重置编辑器Android开发环境")]
    public static void SetAndroidEditorEnv()
    {
        //LocalUI
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "LocalUI;" + Define_Natcam_Android);
        PlayerPrefs.DeleteAll();
        AssetDatabase.Refresh();
    }
    [MenuItem("打包/设置/重置编辑器IOS开发环境")]
    public static void SetIOSEditorEnv()
    {
        //LocalUI
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "LocalUI;" + Define_Natcam_Ios);
        PlayerPrefs.DeleteAll();
        AssetDatabase.Refresh();
    }
    #endregion

    #region IOS

    [MenuItem("打包/Ios_内网_测试")]
    public static void BuildIosClientLan()
    {
        PlayerSettings.productName = "卡萌测试";
        //打包
        BuildIosClientCommon("Test","devlan", "dev");
        SetIOSEditorEnv();
    }

    [MenuItem("打包/Ios_内网_调试")]
    public static void BuildIosClientLanDebug()
    {
        PlayerSettings.productName = "卡萌调试";
        //打包
        BuildIosClientCommon("Debug", "devlan", "dev", BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.AllowDebugging);
        SetIOSEditorEnv();
    }

    public static void BuildIosClientCommon(string xcodeName, string version, string scriptDefine, BuildOptions options = BuildOptions.None)
    {
        //转移不要的resource资源
        HideOtherResources();
        DeleteFolder(Application.dataPath + "/StreamingAssets");
        //lua
        Generator.ClearAll();
        Generator.GenAll();
        //打包
        BuildXcode(xcodeName,version, scriptDefine, options);
        //清理
        Generator.ClearAll();
        ShowOtherResources();
    }
    private const string XcodeName = "kameng_{0}_{1}_xcode";
    private static void BuildXcode(string xcodeName,string versionName, string scriptDefine = "", BuildOptions options = BuildOptions.None)
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.iOS.appleDeveloperTeamID = "X9KE6YR3UT";
        PlayerSettings.stripEngineCode = false;
        //设置autographics
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.iOS, true);
        //设置编译符
        PlayerSettings.bundleIdentifier = BundleIdentifier + "." + versionName;
        PlayerSettings.bundleVersion = VersionString;
        PlayerSettings.iOS.buildNumber = VersionString;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, Define_Natcam_Ios + scriptDefine);

        //设置密码
//        PlayerSettings.Android.keystoreName = Application.dataPath + "/Keys/kameng.keystore";
//        PlayerSettings.Android.keystorePass = "liju1714";
//        PlayerSettings.Android.keyaliasName = "kameng";
//        PlayerSettings.Android.keyaliasPass = "liju1714";
        string xcode = string.Format(XcodeName, xcodeName, VersionString);
        BuildPipeline.BuildPlayer(Levels, Application.dataPath + "/../../../Export/" + xcode,
                       BuildTarget.iOS, options);
    }
    #endregion

    #region Apk

    [MenuItem("打包/安卓客户端_正式版")]
    public static void BuildAndroidClientRelease()
    {
        PlayerSettings.productName = "卡萌";
        //打包
        BuildAndroidClientCommon("release", "release");
        SetAndroidEditorEnv();
    }

    [MenuItem("打包/安卓客户端_测试版_内网")]
    public static void BuildAndroidClientLan()
    {
        PlayerSettings.productName = "卡萌测试";
        //打包
        BuildAndroidClientCommon("devlan", "dev");
        SetAndroidEditorEnv();
    }

    [MenuItem("打包/安卓客户端_测试版_外网")]
    public static void BuildAndroidClientNet()
    {
        PlayerSettings.productName = "卡萌外网测试";
        //打包
        BuildAndroidClientCommon("devnet", "devnet");
        SetAndroidEditorEnv();
    }

    [MenuItem("打包/安卓客户端_测试版_内网_调试")]
    public static void BuildAndroidClientLanDev()
    {
        PlayerSettings.productName = "卡萌调试";
        //打包
        BuildAndroidClientCommon("devlan_debug", "dev;LuaDebug", BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging);
        SetAndroidEditorEnv();
    }
    public static void BuildAndroidClientCommon(string version,string scriptDefine, BuildOptions options = BuildOptions.None)
    {
        //转移不要的resource资源
        HideOtherResources();
        DeleteFolder(Application.dataPath + "/StreamingAssets");
        //lua
        Generator.ClearAll();
        Generator.GenAll();
        //打包
        BuildApk(version, scriptDefine,options);
        //清理
        Generator.ClearAll();
        ShowOtherResources();
    }

    static readonly String[] Levels = { "Assets/Scenes/main.unity" };
    private const String BundleIdentifier = "cn.matrixgame.kameng";
    private const String VersionString = "0.1.6";
    private const int VersionCode = 16;
    private const string Define_Natcam_Ios = "NATCAM_15;INATCAM_C;NATCAM_CORE;NATCAM_EXTENDED;NATCAM_PROFESSIONAL;";
    private const string Define_Natcam_Android = "NATCAM_15;NATCAM_CORE;NATCAM_EXTENDED;NATCAM_PROFESSIONAL;";
    private const string ApkName = "kameng_{0}_{1}.apk";
    private static void BuildApk(string versionName,string scriptDefine = "", BuildOptions options = BuildOptions.None)
    {
        //设置autographics
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, true);
        //设置编译符
        PlayerSettings.bundleIdentifier = BundleIdentifier + "." + versionName;
        PlayerSettings.bundleVersion = VersionString;
        PlayerSettings.Android.bundleVersionCode = VersionCode;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, Define_Natcam_Android + scriptDefine);

        //设置密码
        PlayerSettings.Android.keystoreName = Application.dataPath + "/Keys/kameng.keystore";
        PlayerSettings.Android.keystorePass = "liju1714";
        PlayerSettings.Android.keyaliasName = "kameng";
        PlayerSettings.Android.keyaliasPass = "liju1714";
        string apkname = string.Format(ApkName, versionName, VersionString);
        BuildPipeline.BuildPlayer(Levels, Application.dataPath + "/../../../Export/" + apkname,
                       BuildTarget.Android, options);
    }

    private static void HideOtherResources()
    {
        Directory.Move(Application.dataPath + "/MarkerLessARExample/Resources", Application.dataPath + "/MarkerLessARExample/Resources_Temp");
        Directory.Move(Application.dataPath + "/MarkerBasedARSample/Resources", Application.dataPath + "/MarkerBasedARSample/Resources_Temp");
        Directory.Move(Application.dataPath + "/OpenCVForUnity/Examples/Resources", Application.dataPath + "/OpenCVForUnity/Examples/Resources_Temp");
        AssetDatabase.Refresh();

    }
    private static void ShowOtherResources()
    {
        Directory.Move(Application.dataPath + "/MarkerLessARExample/Resources_Temp", Application.dataPath + "/MarkerLessARExample/Resources");
        Directory.Move(Application.dataPath + "/MarkerBasedARSample/Resources_Temp", Application.dataPath + "/MarkerBasedARSample/Resources");
        Directory.Move(Application.dataPath + "/OpenCVForUnity/Examples/Resources_Temp", Application.dataPath + "/OpenCVForUnity/Examples/Resources");
        AssetDatabase.Refresh();
    }

    #endregion

    #region Lua

    [MenuItem("打包/打包lua")]
    public static void BuildLuaAssetBundles()
    {
        ClearAllMark();
        DeleteFolder(Application.dataPath + "/StreamingAssets");

        //生成lua版本号
        GenLuaGameInfo();

        string luaPath = Application.dataPath + "/AssetBundles/ScriptsLua/";
        string tempPath = Application.dataPath + "/LuaTextFile/";
        GenTempLuaTxtFile(luaPath, tempPath);//生成TXT临时文件
        AssetDatabase.Refresh();
        EncryptLuaFile(tempPath);
        AssetDatabase.Refresh();

        MarkLuaFile();
        BuildScript.BuildAssetBundles();

        string toolPath = Application.dataPath + "/../../../tool/TemporaryTools";
        string srcPath = Application.dataPath + "/StreamingAssets/AssetBundles/";
        string destPath = Application.dataPath + "/../../../Server/AssetbundleRes/AssetbundleRes/Lua";
        DeleteFolder(destPath + "/" + GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget));
        CopyDirectory(srcPath, destPath);
        CopyDirectory(toolPath, destPath);//将MD5工具也一并拷贝到指定目录

        DeleteFolder(tempPath);
        AssetDatabase.Refresh();

        Process ps = new Process { StartInfo = { FileName = destPath + "/FileMd5Gen.exe" } };
        ps.Start();
    }

    const string GameInfoPath = "/AssetBundles/ScriptsLua/info/GameInfo.lua";
    private static void GenLuaGameInfo()
    {
        //full path
        string fullpath = Application.dataPath + GameInfoPath;
        //get info
        int buildNum = 0;
        if (File.Exists(fullpath))
        {
            string[] lines = System.IO.File.ReadAllLines(fullpath);

            foreach (string line in lines)
            {
                if (line.StartsWith("GameInfo.LuaVersion"))
                {
                    int index = line.IndexOf("=");
                    string numStr = line.Substring(index+1, line.Length - index-1);
                    buildNum = Convert.ToInt32(numStr);
                    break;
                }
            }
        }
        //set info
        buildNum++;
        string luaInfo = "--仅供自动生成版本号，请勿修改此文件\r\nGameInfo = {}\r\n";
        luaInfo += string.Format("GameInfo.LuaVersion = {0}\r\n", buildNum);
        luaInfo += string.Format("GameInfo.Version = \"{0}.{1}\"\r\n", VersionString, buildNum);
        luaInfo += "return GameInfo\r\n";
        //write file
        using (StreamWriter writer = new StreamWriter(fullpath, false))
        {
            {
                writer.Write(luaInfo);
            }
            writer.Close();
        }
    }

    static void MarkLuaFile()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/LuaTextFile/", "*.txt", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string curFile = file.Replace("\\", "/");
            int startIndex = curFile.IndexOf("Assets", StringComparison.Ordinal);
            string path = curFile.Substring(startIndex, curFile.Length - startIndex);

            AssetImporter ai = AssetImporter.GetAtPath(path);
			if (ai != null) {
				string assetBundleName = GetBundleName(path);
				ai.assetBundleName = assetBundleName;
				ai.assetBundleVariant = "ar";
			}
        }
    }

    public static void GenTempLuaTxtFile(string sourcePath, string destinationPath)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        Directory.CreateDirectory(destinationPath);
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);
            if (fsi is FileInfo)
            {
                if (!fsi.FullName.Contains(".meta"))
                    File.Copy(fsi.FullName, destName + ".txt");
            }
            else
            {
                Directory.CreateDirectory(destName);
                GenTempLuaTxtFile(fsi.FullName, destName);
            }
        }
    }

    private static void EncryptLuaFile(string path)
    {
        string[] files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            if (File.Exists(file))
            {
                var script = File.ReadAllText(file);
                script = GameUtil.DesEncrypt(script);
                File.WriteAllText(file, script);
            }
        }
    }

    static string GetBundleName(string path)
    {
        path = path.Replace("Assets/LuaTextFile/", "Lua/");
        int endIndex = path.LastIndexOf("/", StringComparison.Ordinal);
        var assetBundleName = path.Substring(0, endIndex);
        return assetBundleName;
    }
    #endregion

    #region 打包UI

    [MenuItem("打包/打包UI")]
    public static void BuildUIAssetBundles()
    {
        ClearAllMark();
        DeleteFolder(Application.dataPath + "/StreamingAssets");
        MarkUIFile();
        BuildScript.BuildAssetBundles();

        string toolPath = Application.dataPath + "/../../../tool/TemporaryTools";
        string srcPath = Application.dataPath + "/StreamingAssets/AssetBundles/";
        string destPath = Application.dataPath + "/../../../Server/AssetbundleRes/AssetbundleRes/UI";
        DeleteFolder(destPath + "/" + GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget));
        CopyDirectory(srcPath, destPath);
        CopyDirectory(toolPath, destPath);//将MD5工具也一并拷贝到指定目录

        Process ps = new Process { StartInfo = { FileName = destPath + "/FileMd5Gen.exe" } };
        ps.Start();

    }

    static void MarkUIFile()
    {
        var files =
            Directory.GetFiles(Application.dataPath + "/AssetBundles/UI/", "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".prefab") || s.EndsWith(".ttf") || s.EndsWith(".png"));
        foreach (var file in files)
        {
            string curFile = file.Replace("\\", "/");
            int startIndex = curFile.IndexOf("Assets", StringComparison.Ordinal);
            string path = curFile.Substring(startIndex, curFile.Length - startIndex);

            AssetImporter ai = AssetImporter.GetAtPath(path);
            string assetBundleName = "ui";//GetBundleName(path);
            ai.assetBundleName = assetBundleName;
            ai.assetBundleVariant = "ar";
        }
    }


    #endregion

    #region 清楚所有标记
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
    static string GetAssetPath(string filePath)
    {
        string curFile = filePath.Replace("\\", "/");
        int startIndex = curFile.IndexOf("Assets", StringComparison.Ordinal);
        string path = curFile.Substring(startIndex, curFile.Length - startIndex);
        return path;
    }
    #endregion

    #region 获得平台
    private static string GetPlatformFolderForAssetBundles(BuildTarget target)
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
            // Add more build targets for your own.
            // If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
            default:
                return null;
        }
    }
    #endregion

}
