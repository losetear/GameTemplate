using System;
using System.Collections.Generic;
using Gamelogic.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using XLua;

namespace GameTemplate
{

    [LuaCallCSharp]
    public class UIManager : MonoBehaviour
    {
        #region 基础接口

        private void Awake()
        {
            SetCamSolid();

            InitRoot();

            //加载必备资源
            PreLoadUI();

            RegEvents();
        }

        void OnDestroy()
        {
            UnRegEvents();
        }

        private void InitRoot()
        {
            GameObject uiRoot = Instantiate(Resources.Load("UI/UIRoot")) as GameObject;
            uiRoot.transform.position = new Vector3(10000, 10000, 10000);
            DontDestroyOnLoad(uiRoot);
            _mainUi = uiRoot.transform.Find("Canvas");
        }

        public void SetCamSolid()
        {
            //            UICamera.mainCamera.clearFlags = CameraClearFlags.SolidColor;
        }

        public void SetCamDepth()
        {
            //            UICamera.mainCamera.clearFlags = CameraClearFlags.Depth;
        }

        private void RegEvents()
        {
            Messenger.AddListener<string>(XEvent.FinishLoadAssetBundle, LoadAssetbundle);
        }

        private void UnRegEvents()
        {
            Messenger.RemoveListener<string>(XEvent.FinishLoadAssetBundle, LoadAssetbundle);
        }

        private void LoadAssetbundle(string abType)
        {
            if (abType == AssetBundleType.UI.ToString())
                io.res.LoadAssetbundle(AssetBundleType.Lua.ToString());
        }


        private static Transform _mainUi;

        public static Transform MainUI
        {
            get
            {
                return _mainUi;
            }
        }

        #endregion

        #region 加载UI
        public void LoadUI(string uiType, Action<GameObject> cb)
        {
#if UNITY_EDITOR
            var path = string.Format("Assets/AssetBundles/UI/{0}.prefab", uiType);
            //                Debug.Log("path = " + path);
            GameObject newUi = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(path));

            if (cb != null)
                cb(newUi);
#else
            if (uiType.Contains("/"))
            {
                int index = uiType.LastIndexOf("/");
                uiType = uiType.Substring(index + 1, uiType.Length - index - 1);
            }

            io.res.LoadUIAssetbundle("ui.lijv", uiType, (go) =>
            {
                if(cb != null)
                    cb(go);
            });
#endif
        }

        GameObject LoadLocalUI(string uiname, bool isLocal = false)
        {
            GameObject newUi = null;

            if (isLocal)
                newUi = Instantiate(Resources.Load("UI/" + uiname)) as GameObject;
            else
            {
#if UNITY_EDITOR
                var path = string.Format("Assets/AssetBundles/UI/{0}.prefab", uiname);
                //                Debug.Log("path = " + path);
                newUi = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(path));
#endif
            }


            //GameObject newUi = Instantiate(Resources.Load("UI/" + uiname)) as GameObject;
            if (newUi != null)
            {
                newUi.transform.SetParent(MainUI.transform);
                newUi.transform.localPosition = Vector3.zero;
                newUi.transform.localScale = Vector3.one;
//                newUi.GetComponent<RectTransform>().offsetMin = Vector2.zero;
//                newUi.GetComponent<RectTransform>().offsetMax = Vector2.zero;

                return newUi;
            }
            else
            {
                Debug.LogError("找不到对应的UI：" + uiname);
                return null;
            }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     预加载UI资源
        /// </summary>
        public void PreLoadUI()
        {
            //ShowUI(UIClass.Menu);
        }

        private readonly Dictionary<string, GameObject> _uiGos = new Dictionary<string, GameObject>();

        public void ShowUI(string uiType)
        {
#if LocalUI
            ShowLocalUI(uiType);
#else
            if(uiType.Contains("/"))
            {
                int index = uiType.LastIndexOf("/");
                uiType = uiType.Substring(index + 1, uiType.Length - index - 1);
            }

            if (!_uiGos.ContainsKey(uiType))
            {
                io.res.LoadUIAssetbundle("ui.lijv", uiType, (go) =>
                {
                    go.transform.SetParent(MainUI.transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
//                    go.GetComponent<RectTransform>().offsetMin = Vector2.zero;
//                    go.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                    go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                    _uiGos.Add(uiType, go);
                });
            }
            else
            {
                _uiGos[uiType].SetActive(true);
            }
#endif
        }

        public void ShowLocalUI(string uiType, bool isLocal = false)
        {
            if (!_uiGos.ContainsKey(uiType))
            {
                GameObject go = LoadLocalUI(uiType, isLocal);
                _uiGos.Add(uiType, go);
            }
            else
            {
                _uiGos[uiType].SetActive(true);
            }
        }

        public void HideUI(string uiType)
        {
#if !LocalUI
            if (uiType.Contains("/"))
            {
                int index = uiType.LastIndexOf("/");
                uiType = uiType.Substring(index + 1, uiType.Length - index - 1);
            }
#endif
            if (_uiGos.ContainsKey(uiType))
            {
                _uiGos[uiType].SetActive(false);
            }
        }

        public GameObject GetUI(string uiType)
        {
#if !LocalUI
            if (uiType.Contains("/"))
            {
                int index = uiType.LastIndexOf("/");
                uiType = uiType.Substring(index + 1, uiType.Length - index - 1);
            }
#endif

            if (_uiGos.ContainsKey(uiType))
            {
                return _uiGos[uiType];
            }
            return null;
        }

        #endregion
    }

}
