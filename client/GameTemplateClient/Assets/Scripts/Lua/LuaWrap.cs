using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using XLua;
using Object = UnityEngine.Object;

namespace GameTemplate
{
    [LuaCallCSharp]
    public class LuaWrap : MonoBehaviour
    {
        private void Awake()
        {
            RegEvents();
        }

        private void OnDestroy()
        {
            UnRegEvents();
        }

        private void RegEvents()
        {
        }

        private void UnRegEvents()
        {
        }

        #region Common

        public bool IsNull(Object obj)
        {
            return obj == null;
        }

#if LuaDebug
        public bool LuaDebug = true;
#else
        public bool LuaDebug = false;
#endif

#if release
        public bool IsRelease = true;
#else
        public bool IsRelease = false;
#endif
        #endregion


        #region Md5

        public string GenMd5(string text)
        {
            //Create a byte array from source data.
            var tmpSource = Encoding.Default.GetBytes(text);
            var tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            var md5 = BitConverter.ToString(tmpHash).Replace("-", "").ToLower();
            return md5;
        }

        #endregion
    }
}