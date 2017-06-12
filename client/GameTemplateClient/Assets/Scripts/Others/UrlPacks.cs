using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace GameTemplate
{
    public class UrlPacks : MonoBehaviour
    {
//      private const string FileUrl = "http://servers.kamengle.com/arplatform/arurls.txt";
        private const string FileUrl = "http://192.168.1.10:3333/urls.txt";

        public void InitUrls(Action<bool> cb)
        {
            StartCoroutine(DownLoad(FileUrl, delegate(WWW file)
            {
                if (string.IsNullOrEmpty(file.text))
                {
                    Urls = new Dictionary<String, ServerUrlPack>();
                    cb(false);
                }
                else
                {
                    Urls = JsonMapper.ToObject<Dictionary<string, ServerUrlPack>>(file.text);
                    cb(true);
                }
            }));
        }

        private IEnumerator DownLoad(string url, Action<WWW> finishFun)
        {
            WWW www = new WWW(url);
            yield return www;
            if (finishFun != null)
            {
                finishFun(www);
            }
        }

        public static Dictionary<string, ServerUrlPack> Urls;

        public static ServerUrlPack GetPack()
        {
#if release
            return Urls["ar_release"];
#else
            return Urls["ar_dev"];
#endif
        }

        public static string GetResUrl()
        {
            ServerUrlPack pack = GetPack();
            return pack.ResUrl;
        }

//        public static string GetApkUrl()
//        {
//            ServerUrlPack pack = GetPack();
//            return pack.ApkUrl;
//        }
//
//        public static string GetAccountUrl()
//        {
//            ServerUrlPack pack = GetPack();
//            return pack.AccountUrl;
//        }
//
//        public static string GetDirUrl()
//        {
//            ServerUrlPack pack = GetPack();
//            return pack.DirUrl;
//        }
//
//        public static string GetNotifyUrl()
//        {
//            ServerUrlPack pack = GetPack();
//            return pack.PayNotifyUrl;
//        }
//
//        public static string GetDownLoadUrl()
//        {
//            ServerUrlPack pack = GetPack();
//            return pack.DownLoadUrl;
//        }
    }
}
