#define  UnityClient

using UnityEngine;

namespace GameTemplate
{
    public class DBStruct
    {
        public static _DATABASE_DEFINE[] s_dbToLoad =
        {
        };


        // 注意，路径不要带后缀名 [3/17/2012 Ivan]
        public static string GetResources(string fileName)
        {
            var obj = Resources.Load("Config/" + fileName);
            var asset = obj as TextAsset;
            if (asset != null)
            {
                return asset.text;
            }
            return "";
        }
    }
}