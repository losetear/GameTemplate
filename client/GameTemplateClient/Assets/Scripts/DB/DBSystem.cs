using System;
using System.Collections.Generic;
using Gamelogic.Extensions;
using UnityEngine;

namespace GameTemplate
{
    /// <summary>
    ///     数据表结构体
    /// </summary>
    public struct _DATABASE_DEFINE
    {
        public int StructEnum;
        public Type StructType;
        public string FileName;

        public _DATABASE_DEFINE(int StructEnum, Type StructType, string FileName)
        {
            this.StructEnum = StructEnum;
            this.StructType = StructType;
            this.FileName = FileName;
        }
    }

    /// <summary>
    ///     数据库系统类
    /// </summary>
    public class DataBaseSystem : MonoBehaviour
    {
        public delegate string GetResources(string fileName);

        public static string strerror = string.Empty;

        private Dictionary<string, string> configs;
        private GetResources GetRes;

        /// <summary>
        ///     按照索引建立数据表的Dictionary
        /// </summary>
        public Dictionary<int, object> m_mapDataBaseBuf = new Dictionary<int, object>();

        /// <summary>
        ///     得到最后的错误信息
        /// </summary>
        public static string StrError
        {
            get { return strerror; }
            set { strerror = value; }
        }

        private void Awake()
        {
            Initial();
        }

        public void Initial()
        {
            try
            {
                GetRes = DBStruct.GetResources;
                OpenAllDataBase(DBStruct.s_dbToLoad);
                //广播配置表加载完成
                Messenger.Broadcast(XEvent.FinishLoadDB);
            }
            catch (Exception e)
            {
                StrError = e.ToString();
            }
        }

        internal void Initial(Dictionary<string, string> configFile)
        {
            try
            {
                configs = configFile;
                GetRes = GetJsonResources;
                OpenAllDataBase(DBStruct.s_dbToLoad);
            }
            catch (Exception e)
            {
                StrError = e.ToString();
            }
            finally
            {
                //广播加载数据完成
                //Messenger.Broadcast(GameEvent.FinishLoadDB);
            }
        }

        private string GetJsonResources(string fileName)
        {
            try
            {
                return configs[fileName];
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
                return "";
            }
        }

        public void Release()
        {
            CloseAllDataBase();
        }

        /// <summary>
        ///     打开所有数据库
        /// </summary>
        /// <param name="s_dbToLoad">数据表结构对应的数组</param>
        public void OpenAllDataBase(_DATABASE_DEFINE[] s_dbToLoad)
        {
            try
            {
                for (var i = 0; i < s_dbToLoad.Length; i++)
                {
                    var t = GetType();
                    var mi = t.GetMethod("CreateDBC").MakeGenericMethod(s_dbToLoad[i].StructType);
                    var dbc =
                        (COMMON_DBC) mi.Invoke(this, new object[] {s_dbToLoad[i].StructEnum, s_dbToLoad[i].FileName});
                    if (dbc != null)
                    {
                        m_mapDataBaseBuf.Add(s_dbToLoad[i].StructEnum, dbc);
                    }
                }
                Debug.Log(string.Format("成功加载{0}张配置表。", s_dbToLoad.Length));
            }
            catch (Exception e)
            {
                StrError += "\r\n----------------------------------------------------\r\n";
                StrError += "OpenAllDataBase方法出错。";
                StrError += "\r\n----------------------------------------------------\r\n";
                StrError += e.ToString();
                StrError += "\r\n----------------------------------------------------\r\n";

                Debug.LogError(StrError);
                //throw new Exception(StrError);
            }
        }

        /// <summary>
        ///     关闭所有数据库
        /// </summary>
        public void CloseAllDataBase()
        {
            m_mapDataBaseBuf.Clear();
        }

        /// <summary>
        ///     得到一个已经打开的数据库并返回
        /// </summary>
        /// <typeparam name="T">数据表泛型</typeparam>
        /// <param name="iDataBase">数据表对应的枚举key值</param>
        /// <returns>枚举对应的数据表数据引用</returns>
        public COMMON_DBC GetDataBase(int iDataBase)
        {
            if (m_mapDataBaseBuf.ContainsKey(iDataBase))
            {
                return (COMMON_DBC) m_mapDataBaseBuf[iDataBase];
            }
            return null;
        }

        public COMMON_DBC GetDataBase(DataBaseStruct edb)
        {
            return GetDataBase((int) edb);
        }

        /// <summary>
        ///     创建数据表的DBC
        /// </summary>
        /// <param name="dbcTag">数据表对应的枚举key值</param>
        /// <param name="fileName">数据文件的文件名</param>
        /// <param name="dbc">数据表结构的引用</param>
        /// <param name="entitymanage">数据表结构的类型</param>
        /// <returns>是否创建数据表DBC成功</returns>
        public COMMON_DBC CreateDBC<T>(int dbcTag, string fileName)
        {
            try
            {
                var dbc = new COMMON_DBC();
                var result = dbc.OpenFromTXT<T>(fileName, GetRes);

                if (!result)
                {
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    Debug.LogError("打开文件 " + fileName + " 失败。");
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    return null;
                }

                if (m_mapDataBaseBuf.ContainsKey(dbcTag))
                {
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    Debug.LogError("数据文件" + fileName + "已经加载过。");
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    return null;
                }
                return dbc;
            }
            catch (Exception e)
            {
                StrError += "\r\n----------------------------------------------------\r\n";
                StrError += "打开文件出错，出错文件目录为：" + fileName;
                StrError += "\r\n----------------------------------------------------\r\n";
                StrError += e.ToString();
                StrError += "\r\n----------------------------------------------------\r\n";

                throw new Exception(StrError);
            }
        }
    }
}