using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace GameTemplate
{
    [LuaCallCSharp]
    public class FullLuaBehaviour : MonoBehaviour
    {
        public string LuaScriptName;
        public Injection[] injections;

        public LuaTable scriptEnv;

        private Action luaAwake;
        private Action luaStart;
        private Action luaUpdate;
        private Action luaFixedUpdate;
        private Action luaOnDestroy;
        private Action luaOnEnable;
        private Action luaOnDisable;

        void Awake()
        {
            InitLua();
            if (luaAwake != null)
            {
                luaAwake();
            }
        }

        // Use this for initialization
        void Start()
        {
            InitLua();
            if (luaStart != null)
            {
                luaStart();
            }
        }
        void OnEnable()
        {
            if (luaOnEnable != null)
            {
                luaOnEnable();
            }
        }

        void OnDisable()
        {
            if (luaOnDisable != null)
            {
                luaOnDisable();
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate();
            }
        }

        void FixedUpdate()
        {
            if (luaFixedUpdate != null)
            {
                luaFixedUpdate();
            }
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy();
            }
            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            scriptEnv.Dispose();
            injections = null;
        }

        void InitLua()
        {
            if (!string.IsNullOrEmpty(LuaScriptName) && scriptEnv == null)
            {
                scriptEnv = LuaManager.LuaEnv.NewTable();

                LuaTable meta = LuaManager.LuaEnv.NewTable();
                meta.Set("__index", LuaManager.LuaEnv.Global);
                scriptEnv.SetMetaTable(meta);
                meta.Dispose();

                scriptEnv.Set("self", this);

                if (injections != null)
                {
                    foreach (var injection in injections)
                    {
                        scriptEnv.Set(injection.name, injection.value);
                    }
                }

                LuaManager.LuaEnv.DoString(io.lua.LoadLuaContent(LuaScriptName), "FullLuaBehaviour", scriptEnv);
                //        LuaManager.LuaEnv.DoString(luaScript.text, "FullLuaBehaviour", scriptEnv);

                scriptEnv.Get("awake", out luaAwake);
                scriptEnv.Get("start", out luaStart);
                scriptEnv.Get("update", out luaUpdate);
                scriptEnv.Get("fixedupdate", out luaFixedUpdate);
                scriptEnv.Get("ondestroy", out luaOnDestroy);
                scriptEnv.Get("onenable", out luaOnEnable);
                scriptEnv.Get("ondisable", out luaOnDisable);
            }
        }
    }
}
