using Gamelogic.Extensions;
using UnityEngine;

namespace GameTemplate
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// ��ʼ����Ϸ������
        /// </summary>
        void Awake()
        {
            io.manager = gameObject;

            DontDestroyOnLoad(gameObject);  //��ֹ�����Լ�
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
            Init();
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        void Init()
        {
            InitManagers();
            InitOthers();
        }

        /// <summary>
        /// ֡Ƶ����
        /// </summary>
        void Update()
        {
            Messenger.Tick();
        }

        /// <summary>
        /// ��ʼ��������
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
            DontDestroyOnLoad(go); //��ֹ�����Լ�
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
                    Debug.LogError("�޷�����URL���ݡ�");
                }
            });
        }

        /// <summary>
        /// ��Դ��ʼ������
        /// </summary>
        public void OnResourceInited()
        {
        }

        /// <summary>
        /// ��������
        /// </summary>
        void OnDestroy()
        {
            if (Debug.isDebugBuild) print("~GameManager was destroyed");
        }
    }
}

