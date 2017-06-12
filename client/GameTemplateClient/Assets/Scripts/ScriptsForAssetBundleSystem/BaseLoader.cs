using System;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR	
using UnityEditor;
#endif

public class BaseLoader : MonoBehaviour {

    //const string kAssetBundlesPath = "/Assets/StreamingAssets/AssetBundles/";

	// Use this for initialization.
	IEnumerator Start ()
	{
		yield return StartCoroutine(Initialize());
	}

	// Initialize the downloading url and AssetBundleManifest object.
	protected IEnumerator Initialize(string assetbundleType = "")
	{
		// Don't destroy the game object as we base on it to run the loading script.
		DontDestroyOnLoad(gameObject);
		
#if UNITY_EDITOR
		Debug.Log ("We are " + (AssetBundleManager.SimulateAssetBundleInEditor ? "in Editor simulation mode" : "in normal mode") );
#endif

		string platformFolderForAssetBundles = 
#if UNITY_EDITOR
			GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			GetPlatformFolderForAssetBundles(Application.platform);
#endif

        assetbundleType = string.IsNullOrEmpty(assetbundleType) ? "" : (assetbundleType + "/");
        string AssetBundlesPath = GetAssetBundles();
		// Set base downloading url.
		string relativePath = GetRelativePath();
        AssetBundleManager.BaseDownloadingURL = relativePath + AssetBundlesPath + assetbundleType + platformFolderForAssetBundles + "/";
#if dev
        Debug.Log("load " + assetbundleType + " downUrl:" + AssetBundleManager.BaseDownloadingURL);
#endif
        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
		var request = AssetBundleManager.Initialize(platformFolderForAssetBundles);
		if (request != null)
			yield return StartCoroutine(request);
	}

    private string GetAssetBundles()
    {
#if UNITY_EDITOR
        return "/Assets/StreamingAssets/AssetBundles/";
#else
        return "/AssetBundles/";
#endif
    }

    public string GetRelativePath()
    {
        if (Application.isEditor)
            return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/"); // Use the build output folder directly.

        if (Application.isWebPlayer)
            return System.IO.Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/") + "/StreamingAssets";
        else if (Application.isMobilePlatform || Application.isConsolePlatform)
            return "file://" + Application.persistentDataPath;
        else // For standalone player.
            return "file://" + Application.streamingAssetsPath;
    }

#if UNITY_EDITOR
	public static string GetPlatformFolderForAssetBundles(BuildTarget target)
	{
		switch(target)
		{
		case BuildTarget.Android:
			return "Android";
        case BuildTarget.iOS:
        case BuildTarget.StandaloneOSXIntel:
        case BuildTarget.StandaloneOSXIntel64:
        case BuildTarget.StandaloneOSXUniversal:
			return "iOS";
//		case BuildTarget.WebPlayer:
//			return "WebPlayer";
//		case BuildTarget.StandaloneWindows:
//		case BuildTarget.StandaloneWindows64:
//			return "Windows";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
		default:
            Debug.LogError("error GetPlatform BuildTarget:" + target);
			return null;
		}
	}
#endif

	public static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
	{
		switch(platform)
		{
		case RuntimePlatform.Android:
			return "Android";
        case RuntimePlatform.IPhonePlayer:
        case RuntimePlatform.OSXPlayer:
        case RuntimePlatform.OSXEditor:
			return "iOS";
//		case RuntimePlatform.WindowsWebPlayer:
//		case RuntimePlatform.OSXWebPlayer:
//			return "WebPlayer";
//		case RuntimePlatform.WindowsPlayer:
//			return "Windows";
			// Add more build platform for your own.
			// If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
        default:
            Debug.LogError("error GetPlatform RuntimePlatform:" + platform);
			return null;
		}
	}

    protected IEnumerator Load(string assetBundleName, string assetName, Vector3 orgPos = default(Vector3), Action<GameObject> callback = null)
	{
		Debug.Log("Start to load " + assetName + " at frame " + Time.frameCount);

		// Load asset from assetBundle.
		AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(GameObject) );
		if (request == null)
			yield break;
		yield return StartCoroutine(request);

		// Get the asset.
		GameObject prefab = request.GetAsset<GameObject> ();
        if (prefab == null)
        {
            Debug.LogError(assetName + "isn't loaded successfully at frame " + Time.frameCount);
        }
        else
        {
            Debug.Log(assetName + "is loaded successfully at frame " + Time.frameCount);
            GameObject go = Instantiate(prefab, orgPos, Quaternion.identity) as GameObject;
            if (callback != null)
                callback(go);
        }
	}

	protected IEnumerator LoadLevel (string assetBundleName, string levelName, bool isAdditive)
	{
		Debug.Log("Start to load scene " + levelName + " at frame " + Time.frameCount);

		// Load level from assetBundle.
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(assetBundleName, levelName, isAdditive);
		if (request == null)
			yield break;
		yield return StartCoroutine(request);

		// This log will only be output when loading level additively.
		Debug.Log("Finish loading scene " + levelName + " at frame " + Time.frameCount);
	}

    protected IEnumerator LoadOther<T>(string assetBundleName, string assetName, Action<T> callback = null)
     where T : UnityEngine.Object
    {
        return LoadOther(assetBundleName, assetName, Vector3.zero, callback);
    }
    protected IEnumerator LoadOther<T>(string assetBundleName, string assetName, Vector3 oriPos, Action<T> callback = null)
     where T : UnityEngine.Object
    {
#if dev
        Debug.Log("Start to load " + assetName + " at frame " + Time.frameCount);
#endif
        assetBundleName = assetBundleName.ToLower();

        // Load asset from assetBundle.
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(T));
        if (request == null)
            yield break;
        yield return StartCoroutine(request);

#if dev
        Debug.Log("finish request " + assetName + " at frame " + Time.frameCount);
#endif
        // Get the asset.
        T prefab = request.GetAsset<T>();
        if (prefab == null)
        {
            Debug.LogError(assetName + "isn't loaded successfully at frame " + Time.frameCount);
        }
        else
        {
            if (callback != null)
                callback(Instantiate(prefab, oriPos, Quaternion.identity));
        }
    }

    protected IEnumerator LoadSound<T>(string assetBundleName, string assetName, Action<AudioClip> callback = null)
     where T : UnityEngine.Object
    {
        //Debug.Log("Start to load " + assetName + " at frame " + Time.frameCount);
        assetBundleName = assetBundleName.ToLower();

        // Load asset from assetBundle.
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(AudioClip));
        if (request == null)
            yield break;
        yield return StartCoroutine(request);

        // Get the asset.
        AudioClip audio = request.GetAsset<AudioClip>();
        if (audio == null)
        {
            Debug.LogError(assetName + "isn't loaded successfully at frame " + Time.frameCount);
        }
        else
        {
            if (callback != null)
                callback(audio);
        }
    }

	// Update is called once per frame
	protected void Update () {
	}
}
