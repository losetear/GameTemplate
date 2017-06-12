using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using XLua;
using Object = UnityEngine.Object;

namespace GameTemplate
{
    public enum AssetBundleType
    {
        Dongwu,
        Hero,
        Lua,
        UI,
        Test
    }

    [LuaCallCSharp]
    public class ResourcesManager : BaseLoader
    {
        private const string ActiveVariants = "ar";
        private bool _isSwithingRes;

        #region 热更新资源

        #region 事件广播
        public void Awake()
        {
            Messenger.AddListener<string, Action>(XEvent.SwitchResDependence, SwitchRes);
        }

        private void SwitchRes(string abType, Action callback)
        {
            if (!IgnoreInitRes(abType))
                StartCoroutine(InitRes(abType, callback));
            else
            {
                Messenger.Broadcast(XEvent.FinishLoadAssetBundle, abType);
                if (callback != null)
                    callback();
            }
        }
        #endregion

        public void Start()
        {
            CacheManager.InitAssetBundleCache();
        }

        public void VerifyAssetbundleVersionFile(string abType, Action<bool> callback)
        {
            io.download.VerifyAssetbundleVersionFile(abType, callback);
        }

        public void LoadAssetbundle(string abType, Action callback = null)
        {
            Debug.Log(string.Format("加载{0}资源 {1}", abType, Time.time));

            if (_isSwithingRes)
            {
                Debug.Log("正在切换资源依赖");
                return;
            }

            if (CacheManager.VerifyAssetBundleUpdateTime(abType))
            {
                SwitchRes(abType, callback);
            }
            else
            {
                io.download.DownLoadAssetbundle(abType, callback);
            }
        }

        //初始化资源 切换android文件 获得不同的依赖
        IEnumerator InitRes(string assetbundleType, Action callback)
        {
            _isSwithingRes = true;

            yield return StartCoroutine(Initialize(assetbundleType));

            //强制清除缓存
            Caching.CleanCache();

            AssetBundleManager.Variants = new[] { ActiveVariants };

            Debug.Log(assetbundleType + "资源初始化完成！" + Time.time);

            Messenger.Broadcast(XEvent.FinishLoadAssetBundle, assetbundleType);

            _isSwithingRes = false;

            if (callback != null)
                callback();
        }

        //因为LUA和UI资源不需要依赖 通过LoadFromFile直接访问资源 所以不需要切换资源
        private bool IgnoreInitRes(string abType)
        {
            return abType == AssetBundleType.UI.ToString() || abType == AssetBundleType.Lua.ToString();
        }

        #endregion

        #region 加载资源接口
        public IEnumerator LoadScenes(string bundleName, string levelName, bool isAdditive)
        {
            return LoadLevel("scenes/" + bundleName + "." + ActiveVariants, levelName, isAdditive);
        }

        public void UnLoadLevel(string bundleName)
        {
            string realName = "scenes/" + bundleName + "." + ActiveVariants;
            AssetBundleManager.UnloadAssetBundle(realName.ToLower());
        }

        Vector3 ModelBornPos = new Vector3(99999,99999,99999);
        public void LoadModel(string bundleName, string assetName, int id, Action<int, GameObject> callback = null)
        {
            StartCoroutine(LoadOther<GameObject>(bundleName, assetName,ModelBornPos, (go) =>
            {
                GameUtil.RetShader(go);
                callback(id, go);
            }));
        }

        public void LoadSound(string bundleName, string assetName, Action<AudioClip> callback = null)
        {
            StartCoroutine(LoadSound<AudioClip>(bundleName, assetName, (audio) =>
                {
                    callback(audio);
                }));
        }

        public void LoadCommonRes<T>(string bundleName, string assetName, Action<T> callback = null)
            where T : Object
        {
            StartCoroutine(LoadOther(bundleName, assetName, callback));
        }

        public string LoadLuaFile(string bundleName, string assetName)
        {
            bundleName = bundleName.ToLower();
            AssetBundle ab;
            if (CacheManager.RecordLuaAssetBundle.ContainsKey(bundleName))
                ab = CacheManager.RecordLuaAssetBundle[bundleName];
            else
            {
#if UNITY_EDITOR
                ab = AssetBundle.LoadFromFile(io.download.PersistentResPath + AssetBundleType.Lua + "/" + GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget) + "/" + bundleName);
#else
                ab = AssetBundle.LoadFromFile(io.download.PersistentResPath + AssetBundleType.Lua + "/" + GetPlatformFolderForAssetBundles(Application.platform) + "/" + bundleName);
#endif
                CacheManager.RecordLuaAssetBundle.Add(bundleName, ab);
            }
            if (ab == null)
            {
                Debug.LogError("AssetBundle null:"+bundleName);
                return "";
            }

            TextAsset txt = ab.LoadAsset<TextAsset>(assetName);
            if (txt == null)
            {
                Debug.LogError("assetName null:" + assetName);
                return "";
            }
            string script;
            if (CacheManager.EncryptRecord.ContainsKey(bundleName + "/" + assetName))
                script = CacheManager.EncryptRecord[bundleName + "/" + assetName];
            else
            {
                script = GameUtil.DesDecrypt(txt.text);
                CacheManager.EncryptRecord.Add(bundleName + "/" + assetName, script);
            }
            //Debug.Log(bundleName + "/" + assetName);
            return script;
        }

        public void LoadUIAssetbundle(string bundleName, string assetName, Action<GameObject> cb = null)
        {
            AssetBundle ab;
            if (CacheManager.RecordUIAssetBundle.ContainsKey(bundleName))
                ab = CacheManager.RecordUIAssetBundle[bundleName];
            else
            {
#if UNITY_EDITOR
                ab = AssetBundle.LoadFromFile(io.download.PersistentResPath + AssetBundleType.UI + "/" + GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget) + "/" + bundleName);
#else
                ab = AssetBundle.LoadFromFile(io.download.PersistentResPath + AssetBundleType.UI + "/" + GetPlatformFolderForAssetBundles(Application.platform) + "/" + bundleName);
#endif
                CacheManager.RecordUIAssetBundle.Add(bundleName, ab);
            }
            GameObject go = Instantiate(ab.LoadAsset<GameObject>(assetName));
            if (cb != null)
                cb(go);
        }

        public Sprite LoadSprite(string assetName)
        {
#if UNITY_EDITOR
            var path = string.Format("Assets/AssetBundles/UI/UITexture/{0}.png", assetName);
            Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            return sp;
#else
            AssetBundle ab;
            const string bundleName = "ui.lijv";
            if (CacheManager.RecordUIAssetBundle.ContainsKey(bundleName))
                ab = CacheManager.RecordUIAssetBundle[bundleName];
            else
            {
#if UNITY_EDITOR
                ab = AssetBundle.LoadFromFile(io.download.PersistentResPath + AssetBundleType.UI + "/" + GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget) + "/" + bundleName);
#else
                ab = AssetBundle.LoadFromFile(io.download.PersistentResPath + AssetBundleType.UI + "/" + GetPlatformFolderForAssetBundles(Application.platform) + "/" + bundleName);
#endif
                CacheManager.RecordUIAssetBundle.Add(bundleName, ab);
            }
            string[] temp = assetName.Split('/');
            assetName = temp[temp.Length - 1];
            Sprite sp = ab.LoadAsset<Sprite>(assetName);
            return sp;
#endif
        }

        public Texture LoadTexture(string assetName)
        {
#if UNITY_EDITOR
            var path = string.Format("Assets/AssetBundles/UI/UITexture/{0}.png", assetName);
            Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
            return texture;
#else
            AssetBundle ab;
            const string bundleName = "ui.lijv";
            if (CacheManager.RecordUIAssetBundle.ContainsKey(bundleName))
                ab = CacheManager.RecordUIAssetBundle[bundleName];
            else
            {
#if UNITY_EDITOR
                ab = AssetBundle.LoadFromFile(io.download.PersistentResPath + AssetBundleType.UI + "/" + GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget) + "/" + bundleName);
#else
                ab = AssetBundle.LoadFromFile(io.download.PersistentResPath + AssetBundleType.UI + "/" + GetPlatformFolderForAssetBundles(Application.platform) + "/" + bundleName);
#endif
                CacheManager.RecordUIAssetBundle.Add(bundleName, ab);
            }
            string[] temp = assetName.Split('/');
            assetName = temp[temp.Length - 1];
            Texture texture = ab.LoadAsset<Texture>(assetName);
            return texture;
#endif
        }
        #endregion
    }
}
