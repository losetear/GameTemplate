using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gamelogic.Extensions;
using UnityEngine;
using XLua;

namespace GameTemplate
{
    public class LuaManager : MonoBehaviour
    {
        internal static LuaEnv LuaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 

        void Awake()
        {
            InitLua();
            RegEvents();
            InitLoader();
            //            InitMain();
        }

        private void InitLua()
        {
            LuaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
        }

        void OnDestroy()
        {
            UnRegEvents();
        }

        void Update()
        {
            if (Time.time - lastGCTime > GCInterval)
            {
                LuaEnv.Tick();
                lastGCTime = Time.time;
            }
        }

        private void RegEvents()
        {
            Messenger.AddListener(XEvent.FinishLoading, InitMain);
        }

        private void UnRegEvents()
        {
            Messenger.RemoveListener(XEvent.FinishLoading, InitMain);
        }

        private void InitMain()
        {
            LuaEnv.DoString("require 'Main'");
        }

        #region Loader

        private void InitLoader()
        {
#if UNITY_EDITOR
            LuaEnv.AddLoader((ref string filename) =>
            {
                var path = string.Format("{0}/AssetBundles/ScriptsLua/{1}.lua", Application.dataPath, filename);
                if (File.Exists(path))
                {
                    var script = File.ReadAllText(path);
                    return Encoding.UTF8.GetBytes(script);
                }
                return null;
            });
#else

            LuaEnv.AddLoader((ref string fileName) =>
            {
                string tempName = fileName;
                string path = "lua.lijv";
                if (fileName.Contains("/"))
                {
                    int index = fileName.LastIndexOf('/');
                    fileName = fileName.Substring(index + 1, fileName.Length - index - 1);

                    index = tempName.LastIndexOf('/');
                    tempName = tempName.Substring(0, index);
                    path = "lua/" + tempName + ".lijv";
                }


                string script = io.res.LoadLuaFile(path, fileName + ".lua");
                return Encoding.UTF8.GetBytes(script);
            });
#endif
        }

        public string LoadLuaContent(string fileName)
        {
#if UNITY_EDITOR
            string path = string.Format("{0}/AssetBundles/ScriptsLua/{1}.lua", Application.dataPath, fileName);
            if (GameUtil.FileExists(path,true))
            {
                string script = File.ReadAllText(path);
                if (string.IsNullOrEmpty(script))
                {
                    Debug.LogError("加载lua异常:"+fileName);
                }
                return script;
            }
            else
            {
                Debug.LogError("找不到lua文件：" + path);
            }
            return null;
#else
            string tempName = fileName;
            string path = "lua.lijv";
            if (fileName.Contains("/"))
            {
                int index = fileName.LastIndexOf('/');
                fileName = fileName.Substring(index + 1, fileName.Length - index - 1);

                index = tempName.LastIndexOf('/');
                tempName = tempName.Substring(0, index);
                path = "lua/" + tempName + ".lijv";
            }


            string script = io.res.LoadLuaFile(path, fileName + ".lua");
            if (string.IsNullOrEmpty(script))
            {
                Debug.LogError("加载lua异常:"+fileName);
            }
            return script;
#endif
        }
        #endregion
    }
}