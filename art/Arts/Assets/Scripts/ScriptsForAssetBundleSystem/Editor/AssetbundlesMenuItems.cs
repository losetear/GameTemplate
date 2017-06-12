using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetbundlesMenuItems
{
	const string kSimulateAssetBundlesMenu = "AssetBundles/Simulate AssetBundles";

	//[MenuItem(kSimulateAssetBundlesMenu)]
	public static void ToggleSimulateAssetBundle ()
	{
		AssetBundleManager.SimulateAssetBundleInEditor = !AssetBundleManager.SimulateAssetBundleInEditor;
	}

	//[MenuItem(kSimulateAssetBundlesMenu, true)]
	public static bool ToggleSimulateAssetBundleValidate ()
	{
		Menu.SetChecked(kSimulateAssetBundlesMenu, AssetBundleManager.SimulateAssetBundleInEditor);
		return true;
	}
	
	//[MenuItem ("AssetBundles/Build AssetBundles")]
	static public void BuildAssetBundles ()
	{
		BuildScript.BuildAssetBundles();
		//刷新编辑器  
		AssetDatabase.Refresh();  
	}

	//[MenuItem ("AssetBundles/ReBuild AssetBundles")]
	static void DeleteOldAssets ()
	{
		BuildScript.DeleteOldAsset();
		BuildScript.BuildAssetBundles();
		//刷新编辑器  
		AssetDatabase.Refresh();  
	}

    //[MenuItem("AssetBundles/ClearData")]
    static void ClearData()
    {
        Caching.CleanCache();
    }
}
