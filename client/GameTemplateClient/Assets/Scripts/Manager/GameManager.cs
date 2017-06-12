using Gamelogic.Extensions;
using UnityEngine;

namespace GameTemplate
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// 初始化游戏管理器
        /// </summary>
        void Awake()
        {
            io.manager = gameObject;

            DontDestroyOnLoad(gameObject);  //防止销毁自己
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {
            InitManagers();
            InitOthers();
        }

        /// <summary>
        /// 帧频更新
        /// </summary>
        void Update()
        {
            Messenger.Tick();
        }

        /// <summary>
        /// 初始化管理器
        /// </summary>
        public void InitManagers()
        {
            //first is db
            gameObject.AddComponent<DataBaseSystem>();
            gameObject.AddComponent<UIManager>();
            gameObject.AddComponent<LuaManager>();
            gameObject.AddComponent<ResourcesManager>();
            gameObject.AddComponent<DownloadManager>();
            gameObject.AddComponent<CacheManager>();
            gameObject.AddComponent<LuaWrap>();
        }

        private void InitOthers()
        {
            GameObject go = new GameObject("UrlPacks");
            DontDestroyOnLoad(go); //防止销毁自己
            UrlPacks url = go.AddComponent<UrlPacks>();
            url.InitUrls((success) =>
            {
                if (success)
                {
                    io.download.InitRelativePath();
                    io.ui.ShowLocalUI("LocalLoading", true);
                    io.res.LoadAssetbundle(AssetBundleType.UI.ToString());
                }
                else
                {
                    Debug.LogError("无法解析URL数据。");
                }
            });
        }

        /// <summary>
        /// 资源初始化结束
        /// </summary>
        public void OnResourceInited()
        {
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy()
        {
            if (Debug.isDebugBuild) print("~GameManager was destroyed");
        }
    }
}

