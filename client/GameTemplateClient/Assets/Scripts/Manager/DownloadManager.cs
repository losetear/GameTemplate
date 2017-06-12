using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameTemplate
{
    public struct ResInfoStruct
    {
        public int Id;
        public string Md5;
    }

    public class DownloadManager : BaseLoader
    {
        private const string VersionFile = "version.txt";
        private string _curAssetbundleType;
        private bool _isLoadingAssetBundle;

        private string _persistentResUrl;//沙盒路径url
        public string PersistentResPath;//沙盒路径path
        private string _serverAssetbundlePath;
        string _serverResUrl;

        private Dictionary<string, ResInfoStruct> _persistentResVersion;//用来存储沙盒文件夹下的version
        private Dictionary<string, ResInfoStruct> _serverResVersion;//用来存储服务器version
        private List<string> _needDownFiles;
        private bool _needUpdateLocalVersionFile;
        private WWW _www;
        private int _currentLoadFileNum;
        private int _currentLoadFileId;
        private int _versionNum;

        private Action _callback;

        #region assetbundle热更新

        public void Awake()
        {
            InitData();
        }

        public void Start()
        { }

        public void DownLoadAssetbundle(string abType, Action callback)
        {
            if (_isLoadingAssetBundle)
            {
                Debug.Log("请等待资源加载完成");
                return;
            }

            _callback = callback;

            _isLoadingAssetBundle = true;

            _curAssetbundleType = abType;

            _serverAssetbundlePath = UrlPacks.GetResUrl() + _curAssetbundleType + "/";

            ClearData();

            VerifyAssetbundleVersionFile(abType, needUpdate =>
            {
                DownLoadRes();
            });
        }

        public void VerifyAssetbundleVersionFile(string abType, Action<bool> callback)
        {
            ClearData();

            _serverAssetbundlePath = UrlPacks.GetResUrl() + abType + "/";

            //加载服务器的version 文件
            StartCoroutine(DownLoad(_serverAssetbundlePath + VersionFile, wwwServerVersionData =>
            {
                //记录服务器version文件MD5
                ParseVersionFile(wwwServerVersionData.text, _serverResVersion);

                //加载客户端的version文件
                StartCoroutine(DownLoad(_persistentResUrl + abType + "/" + VersionFile, wwwPersistentVersionData =>
                {
                    //记录沙盒路径下的version文件的MD5
                    ParseVersionFile(wwwPersistentVersionData.text, _persistentResVersion);

                    //计算出需要重新加载的资源
                    CompareVersion();

                    callback(_needUpdateLocalVersionFile);

                }));
            }));
        }

        //初始化数据
        private void InitData()
        {
            _persistentResVersion = new Dictionary<string, ResInfoStruct>();
            _serverResVersion = new Dictionary<string, ResInfoStruct>();
            _needDownFiles = new List<string>();
            _currentLoadFileNum = 0;
            _currentLoadFileId = 0;
        }

        //清理数据
        private void ClearData()
        {
            _persistentResVersion.Clear();
            _serverResVersion.Clear();
            _needDownFiles.Clear();
            _currentLoadFileNum = 0;
            _currentLoadFileId = 0;
        }

        //初始化路径
        public void InitRelativePath()
        {
            if (Application.isEditor)
            {
                _persistentResUrl = "file://" + Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/StreamingAssets/AssetBundles/";
                PersistentResPath = Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/StreamingAssets/AssetBundles/";
                _serverResUrl = UrlPacks.GetResUrl();
            }
            else if (Application.isMobilePlatform)
            {
                _persistentResUrl = "file://" + Application.persistentDataPath + "/AssetBundles/";
                PersistentResPath = Application.persistentDataPath + "/AssetBundles/";
                _serverResUrl = UrlPacks.GetResUrl();
            }
            else
            {
                _persistentResUrl = "file://" + Application.streamingAssetsPath + "/AssetBundles/";
                PersistentResPath = Application.streamingAssetsPath + "/AssetBundles/";
            }
        }

        //依次加载需要更新的资源
        private void DownLoadRes()
        {
            //资源全部下载完更新一次version
            if (_needDownFiles.Count == 0)
            {
                UpdateLocalVersionFile();

                Debug.Log(_curAssetbundleType + "资源更新完成！");

                _isLoadingAssetBundle = false;

                CacheManager.RecordUpdateTime(_curAssetbundleType);

                Messenger.Broadcast(XEvent.SwitchResDependence, _curAssetbundleType, _callback);

                return;
            }

            //每10个文件下载完之后 更新一次version
            if (_currentLoadFileNum > 0 && _currentLoadFileNum % 10 == 0)
                UpdateLocalVersionFile(_currentLoadFileId);

            string file = _needDownFiles[0];
            _needDownFiles.RemoveAt(0);
            StartCoroutine(DownLoad(_serverAssetbundlePath + file, delegate(WWW w)
            {
                //将下载的资源替换本地就的资源
                _currentLoadFileId = _serverResVersion[file].Id;
                ReplaceLocalRes(PersistentResPath + _curAssetbundleType + "/" + file, w.bytes);
                DownLoadRes();
            }));
        }

        private void ReplaceLocalRes(string filePath, byte[] data)
        {
            FileInfo fi = new FileInfo(filePath);
            var di = fi.Directory;
            if (di != null && !di.Exists)
                di.Create();
            FileStream stream = new FileStream(filePath, FileMode.Create);
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }

        //更新沙盒路径下的version 文件
        private void UpdateLocalVersionFile()
        {
            if (!_needUpdateLocalVersionFile) return;

            StringBuilder versions = new StringBuilder();
            versions.Append("#").Append(",").Append(_versionNum).Append("\n").Append("#").Append("\n");
            foreach (var item in _serverResVersion)
            {
                versions.Append(item.Key).Append(",").Append(item.Value.Md5).Append("\n");
            }
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            string path = PersistentResPath + _curAssetbundleType + "/" + VersionFile;
            ReplaceLocalRes(path, data);
        }

        private void UpdateLocalVersionFile(int id)
        {
            int count = 0;
            StringBuilder versions = new StringBuilder();
            versions.Append("#").Append(",").Append(_versionNum).Append("\n").Append("#").Append("\n");
            foreach (var item in _serverResVersion)
            {
                if (count > id)
                    break;
                versions.Append(item.Key).Append(",").Append(item.Value.Md5).Append("\n");
                count++;
            }
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            string path = PersistentResPath + _curAssetbundleType + "/" + VersionFile;
            ReplaceLocalRes(path, data);
        }

        private void CompareVersion()
        {
            foreach (var version in _serverResVersion)
            {
                string fileName = version.Key;
                string serverMd5 = version.Value.Md5;
                //新增的资源
                if (!_persistentResVersion.ContainsKey(fileName))
                {
                    _needDownFiles.Add(fileName);
                }
                else
                {
                    //需要替换的资源
                    ResInfoStruct localMd5;
                    _persistentResVersion.TryGetValue(fileName, out localMd5);
                    if (!serverMd5.Equals(localMd5.Md5))
                    {
                        _needDownFiles.Add(fileName);
                    }
                }
            }

            //本次有更新，同时更新本地的version.txt
            _needUpdateLocalVersionFile = _needDownFiles.Count > 0;
        }

        private void ParseVersionFile(string content, Dictionary<string, ResInfoStruct> dict)
        {
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogWarning("版本文件为空！");
                return;
            }
            string[] items = content.Split('\n');
            int index = 0;
            foreach (string item in items)
            {
                string[] info = item.Split(',');
                if (info.Length == 2 && !info[0].Contains(".meta") && !info[0].Contains("#"))
                {
                    ResInfoStruct resInfo;
                    resInfo.Id = index++;
                    resInfo.Md5 = info[1];
                    dict.Add(info[0], resInfo);
                }
            }
        }

        private IEnumerator DownLoad(string url, Action<WWW> finishFun)
        {
            _www = new WWW(url);
            yield return _www;
            if (!string.IsNullOrEmpty(_www.error))
            {
                Debug.LogError("download error:" + _www.error);
            }
            if (finishFun != null)
            {
                finishFun(_www);
            }
        }

        #endregion

    }
}
