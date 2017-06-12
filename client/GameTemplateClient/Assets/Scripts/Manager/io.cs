using UnityEngine;
using System.Collections;
using System.Text;
using GameTemplate;
using XLua;

/// <summary>
/// Interface Manager Object
/// </summary>
[LuaCallCSharp]
public class 
    io {
    /// <summary>
    /// 游戏管理器对象
    /// </summary>
    private static GameObject _manager = null;
    public static GameObject manager {
        get {
            return _manager;
        }
        set {
            _manager = value;
        }
        
    }

    /// <summary>
    /// 游戏管理器
    /// </summary>
    private static GameManager _gameManager = null;
    public static GameManager gameManager {
        get {
            if (_gameManager == null)
                _gameManager = manager.GetComponent<GameManager>();
            return _gameManager;
        }
    }

    private static UIManager _uiManager = null;
    public static UIManager ui {
        get {
            if (_uiManager == null)
                _uiManager = manager.GetComponent<UIManager>();
            return _uiManager;
        }
    }

    private static DataBaseSystem _db = null;
    public static DataBaseSystem db
    {
        get
        {
            if (_db == null)
                _db = manager.GetComponent<DataBaseSystem>();
            return _db;
        }
    }

    private static LuaManager _lua = null;
    public static LuaManager lua
    {
        get
        {
            if (_lua == null)
                _lua = manager.GetComponent<LuaManager>();
            return _lua;
        }
    }

    private static ResourcesManager _res = null;
    public static ResourcesManager res
    {
        get
        {
            if (!_res)
                _res = manager.GetComponent<ResourcesManager>();
            return _res;
        }
    }

    private static DownloadManager _download = null;
    public static DownloadManager download
    {
        get
        {
            if (!_download)
                _download = manager.GetComponent<DownloadManager>();
            return _download;
        }
    }

    private static CacheManager _cache = null;
    public static CacheManager cache
    {
        get
        {
            if (!_cache)
                _cache = manager.GetComponent<CacheManager>();
            return _cache;
        }
    }

    private static LuaWrap _wrap = null;
    public static LuaWrap wrap
    {
        get
        {
            if (!_wrap)
                _wrap = manager.GetComponent<LuaWrap>();
            return _wrap;
        }
    }
}
