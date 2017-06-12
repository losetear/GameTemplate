using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace GameTemplate
{
    public class LastUpdateResTime
    {
        public readonly Dictionary<string, DateTime> LastUpdateTime = new Dictionary<string, DateTime>();
    }
    public class CacheManager : MonoBehaviour
    {

        private const string PlayerPrefsKey = "LasUpdateResTime";
        private static LastUpdateResTime _lastUpdateRestTime;
        public static readonly Dictionary<string, string> EncryptRecord = new Dictionary<string, string>();
        public static readonly Dictionary<string, AssetBundle> RecordLuaAssetBundle = new Dictionary<string, AssetBundle>();
        public static readonly Dictionary<string, AssetBundle> RecordUIAssetBundle = new Dictionary<string, AssetBundle>();

        public static void InitAssetBundleCache()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKey))
                PlayerPrefs.DeleteKey(PlayerPrefsKey);
            string strUpdateTime = PlayerPrefs.GetString(PlayerPrefsKey);
            _lastUpdateRestTime = string.IsNullOrEmpty(strUpdateTime) ? new LastUpdateResTime() : JsonMapper.ToObject<LastUpdateResTime>(strUpdateTime);
        }

        public static bool VerifyAssetBundleUpdateTime(string abType)
        {
            return _lastUpdateRestTime.LastUpdateTime.ContainsKey(abType) &&
                   (DateTime.Now - _lastUpdateRestTime.LastUpdateTime[abType]).TotalHours < 1;
        }

        public static void RecordUpdateTime(string abType)
        {
            if (_lastUpdateRestTime.LastUpdateTime.ContainsKey(abType))
            {
                _lastUpdateRestTime.LastUpdateTime[abType] = DateTime.Now;
            }
            else
            {
                _lastUpdateRestTime.LastUpdateTime.Add(abType, DateTime.Now);
            }
            string json = JsonMapper.ToJson(_lastUpdateRestTime);
            PlayerPrefs.SetString(PlayerPrefsKey, json);
        }
        
        public static void ClearRecord()
        {
            EncryptRecord.Clear();
            RecordLuaAssetBundle.Clear();
            RecordUIAssetBundle.Clear();
        }
    }

}
