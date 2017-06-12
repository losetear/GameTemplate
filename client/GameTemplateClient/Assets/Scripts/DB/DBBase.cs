using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using UnityEngine;

namespace GameTemplate
{
    public class COMMON_DBC
    {
        /// <summary>
        ///     存放map所存数据的元素个数方便外部调用
        /// </summary>
        private static int m_iDataNum;

        public static int DataNum
        {
            get { return m_iDataNum; }
            set { m_iDataNum = value; }
        }

        /// <summary>
        ///     按照数据索引建立数据Dictionary
        /// </summary>
        //public Dictionary<int, T> StructDict = new Dictionary<int, T>();
        public Hashtable Dicts = new Hashtable();

        /// <summary>
        ///     存放数据表结构体内部成员参数名称
        /// </summary>
        private static readonly List<string> strList = new List<string>();

        /// <summary>
        ///     数据表数据类型
        /// </summary>
        private enum FIELD_TYPE
        {
            T_INT = 0, //整数
            T_FLOAT = 1, //浮点数
            T_STRING = 2 //字符串
        }

        /// <summary>
        ///     数据表数组的维数
        /// </summary>
        private enum ARRAY_RANK
        {
            RANK_ONE = 1,
            RANK_TWO
        }

        /// <summary>
        ///     存放数据表参数类型
        /// </summary>
        private static readonly List<FIELD_TYPE> FieldTypeList = new List<FIELD_TYPE>();

        /// <summary>
        ///     判断每行数据的状态
        /// </summary>
        private enum TXTLineStatus
        {
            LINE_OK = 0, //正确行
            LINE_EMPTY = 1, //空行
            LINE_ANNOTATE = 2, //注释行
            LINE_ERROR = 3 //错误行
        }

        /// <summary>
        ///     将一行字符串截取到一个List中
        /// </summary>
        /// <param name="str">将要进行截取的字符串</param>
        /// <param name="LString">用来存放截取后的字符串的List</param>
        /// <returns>是否成功截取</returns>
        private static bool Split_To_StringList(string str, List<string> LString)
        {
            try
            {
                LString.Clear();

                if (str == null)
                {
                    return false;
                }

                var szTemp = str.Split(new string[1] {"\t"}, StringSplitOptions.None);

                for (var i = 0; i < szTemp.Length; i++)
                {
                    LString.Add(szTemp[i].Trim('"'));
                }

                return true;
            }
            catch (Exception ex)
            {
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += ex.ToString();
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += "按制表符读取数据时出错。";
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                throw new Exception(DataBaseSystem.StrError);
            }
        }

        /// <summary>
        ///     从表中读取一行数据并进行处理
        /// </summary>
        /// <param name="sr">文件读取流</param>
        /// <param name="LString">用来存放从数据流读取一行并按制表符截取后的字符串</param>
        /// <returns>读取数据流中每行数据流的状态</returns>
        private static TXTLineStatus ReadOneLineFromTXT(string str, List<string> LString)
        {
            //将读取的一行数据分割并放入List中
            var bTemp = Split_To_StringList(str, LString);

            //读取是否空行
            if (!bTemp)
            {
                return TXTLineStatus.LINE_EMPTY;
            }

            //读取是否注释行
            if (str.StartsWith("#"))
            {
                return TXTLineStatus.LINE_ANNOTATE;
            }

            //列数不能为空
            if (LString.Count == 0)
            {
                return TXTLineStatus.LINE_ERROR;
            }

            //第一列不能为空
            if (LString[0].Equals(""))
            {
                return TXTLineStatus.LINE_ERROR;
            }

            //补空格
            if (LString.Capacity < strList.Capacity)
            {
                var iTemp = strList.Capacity - LString.Capacity;
                for (var i = 0; i < iTemp; i++)
                {
                    LString.Add("");
                }
            }
            return TXTLineStatus.LINE_OK;
        }

        private int ChangStringToInt(string str)
        {
            try
            {
                int intTemp;
                if (str.StartsWith("0x"))
                {
                    str = str.Replace("0x", "");
                    intTemp = int.Parse(str, NumberStyles.HexNumber);
                }
                else
                {
                    intTemp = int.Parse(str);
                }
                return intTemp;
            }
            catch (Exception ex)
            {
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += ex.ToString();
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += "转换字符串为Int型时出错。出错的字符串为:" + str;
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                throw new Exception(DataBaseSystem.StrError);
            }
        }

        /// <summary>
        ///     给结构体内的结构体数组赋值
        /// </summary>
        /// <param name="objTemp">要被赋值的对象</param>
        /// <param name="LString">要赋的值</param>
        /// <param name="index">结构体对象的元素数目</param>
        /// <returns>是否复制成功</returns>
        private bool SetValuetoStructInStruct(object objTemp, List<string> LString, ref int index)
        {
            try
            {
                var typeTemp = objTemp.GetType();
                var fields = typeTemp.GetFields();
                for (var i = 0; i < fields.Length; i++)
                {
                    if (index > LString.Count)
                    {
                        Debug.LogError("\r\n----------------------------------------------------\r\n");
                        Debug.LogError("结构类型内部的成员结构类型的数据表列数超出范围出错。");
                        Debug.LogError("\r\n----------------------------------------------------\r\n");
                        return false;
                    }
                    if (fields[i].FieldType.IsArray)
                    {
                        //结构体内一维数组的处理
                        if ((int) ARRAY_RANK.RANK_ONE == fields[i].FieldType.GetArrayRank())
                        {
                            var fieldValue = fields[i].GetValue(objTemp);
                            for (var j = 0; j < ((Array) fieldValue).Length; j++, index++)
                            {
                                ((Array) fieldValue).SetValue(Convert.ChangeType(LString[index],
                                    fields[i].FieldType.GetElementType()), j);
                            }
                        }
                        //结构体内二维数组的处理
//                         else if ((int)ARRAY_RANK.RANK_TWO == fields[i].FieldType.GetArrayRank())
//                         {
//                             object fieldValue = fields[i].GetValue(objTemp);
//                             for (int j = 0; j < ((Array)fieldValue).GetLength(0); j++)
//                             {
//                                 for (int k = 0; k < ((Array)fieldValue).GetLength(1); k++, index++)
//                                 {
//                                     ((Array)fieldValue).SetValue(Convert.ChangeType(LString[index],
//                                         fields[i].FieldType.GetElementType()), j, k);
//                                 }
//                             }
//                         }
                        else
                        {
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            Debug.LogError("结构类型内部的成员结构类型的成员数组维数大于二维。");
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            return false;
                        }
                    }
                    else if (fields[i].FieldType == typeof (bool))
                    {
                        //对于数据表中bool值的处理
                        if (LString[index] == "")
                        {
                            fields[i].SetValue(objTemp, false);
                            index++;
                        }
                        else if (Convert.ToInt32(LString[index]) > 0)
                        {
                            fields[i].SetValue(objTemp, true);
                            index++;
                        }
                        else if (Convert.ToInt32(LString[index]) <= 0)
                        {
                            fields[i].SetValue(objTemp, false);
                            index++;
                        }
                        else
                        {
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            Debug.LogError("结构类型内部的布尔值赋值出错。出错的位置在第 " + (Dicts.Count + 3) + " 行，第 " + (index + 1) + " 列。");
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            return false;
                        }
                    }
                    else
                    {
                        if ((fields[i].FieldType == typeof (int)
                             || fields[i].FieldType == typeof (uint)
                             || fields[i].FieldType == typeof (short)
                             || fields[i].FieldType == typeof (ushort)) && LString[index] == "")
                        {
                        }
                        else if ((fields[i].FieldType == typeof (float)
                                  || fields[i].FieldType == typeof (double)) && LString[index] == "")
                        {
                        }
                        else if (fields[i].FieldType == typeof (int))
                        {
                            var intTemp = ChangStringToInt(LString[index]);
                            fields[i].SetValue(objTemp, intTemp);
                        }
                        else
                        {
                            fields[i].SetValue(objTemp, Convert.ChangeType(LString[index], fields[i].FieldType));
                        }
                        index++;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += ex.ToString();
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += "结构中的结构体赋值出错。出错的字段是第 " + (Dicts.Count + 3) + " 行,第 " + (index + 1) + " 列。";
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                throw new Exception(DataBaseSystem.StrError);
            }
        }

        /// <summary>
        ///     将List中的值赋值给对象objTemp
        /// </summary>
        /// <param name="LString">要赋的值</param>
        /// <param name="objTemp">赋值的对象</param>
        /// <returns>是否赋值成功</returns>
        private bool ChangeListStringToStruct(List<string> LString, object objTemp)
        {
            var index = 0;
            var typeTemp = objTemp.GetType();
            try
            {
                var fields = typeTemp.GetFields();

                for (var i = 0; i < fields.Length; i++)
                {
                    if (index > LString.Count)
                    {
                        Debug.LogError("\r\n----------------------------------------------------\r\n");
                        Debug.LogError("数据表列数超出范围出错。");
                        Debug.LogError("\r\n----------------------------------------------------\r\n");
                        return false;
                    }
                    if (fields[i].FieldType.IsArray)
                    {
                        //结构体内一维数组的处理
                        if ((int) ARRAY_RANK.RANK_ONE == fields[i].FieldType.GetArrayRank())
                        {
                            var fieldValue = fields[i].GetValue(objTemp);

                            if (fields[i].FieldType.GetElementType().IsClass &&
                                fields[i].FieldType.GetElementType() != typeof (string))
                            {
                                //结构体内的一位结构体数组的处理
                                for (var iStructArray = 0; iStructArray < ((Array) fieldValue).Length; iStructArray++)
                                {
                                    var typeClassArray = fields[i].FieldType.GetElementType();
                                    var objClassArray = Activator.CreateInstance(typeClassArray);
                                    var LStringClassArray = LString.GetRange(index, typeClassArray.GetFields().Length);
                                    var indexClassArray = 0;
                                    if (SetValuetoStructInStruct(objClassArray, LStringClassArray, ref indexClassArray))
                                    {
                                        ((Array) fieldValue).SetValue(
                                            Convert.ChangeType(objClassArray, fields[i].FieldType.GetElementType()),
                                            iStructArray);
                                        index += indexClassArray;
                                    }
                                }
                            }
                            else
                            {
                                for (var j = 0; j < ((Array) fieldValue).Length; j++)
                                {
                                    ((Array) fieldValue).SetValue(Convert.ChangeType(LString[index],
                                        fields[i].FieldType.GetElementType()), j);
                                    index++;
                                }
                            }
                        }
                        //结构体内二维数组的处理
                        //else if ((int)ARRAY_RANK.RANK_TWO == fields[i].FieldType.GetArrayRank())
                        //{
                        //    object fieldValue = fields[i].GetValue(objTemp);
                        //    for (int j = 0; j < ((Array)fieldValue).GetLength(0); j++)
                        //    {
                        //        for (int k = 0; k < ((Array)fieldValue).GetLength(1); k++)
                        //        {
                        //            ((Array)fieldValue).SetValue(Convert.ChangeType(LString[index],
                        //                fields[i].FieldType.GetElementType()), j, k);
                        //            index++;
                        //        }
                        //    }
                        //}
                        else
                        {
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            Debug.LogError("结构内部数组维数大于二维。");
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            return false;
                        }
                    }
                    else if (fields[i].FieldType == typeof (bool))
                    {
                        //对于数据表中bool值的处理
                        if (LString[index] == "")
                        {
                            fields[i].SetValue(objTemp, false);
                            index++;
                        }
                        else if (Convert.ToInt32(LString[index]) > 0)
                        {
                            fields[i].SetValue(objTemp, true);
                            index++;
                        }
                        else if (Convert.ToInt32(LString[index]) <= 0)
                        {
                            fields[i].SetValue(objTemp, false);
                            index++;
                        }
                        else
                        {
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            Debug.LogError("结构类型内部的布尔值赋值出错。出错的位置在第 " + (Dicts.Count + 3) + " 行，第 " + (index + 1) + " 列。");
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            return false;
                        }
                    }
                    else if (fields[i].FieldType.IsClass && fields[i].FieldType != typeof (string))
                    {
                        var objClass = Activator.CreateInstance(fields[i].FieldType);
                        var LStringClass = LString.GetRange(index, fields[i].FieldType.GetFields().Length);
                        var indexClass = 0;
                        if (SetValuetoStructInStruct(objClass, LStringClass, ref indexClass))
                        {
                            fields[i].SetValue(objTemp, Convert.ChangeType(objClass, fields[i].FieldType));
                            index += indexClass;
                        }
                        else
                        {
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            Debug.LogError("结构类型" + typeTemp + "中的结构体" + fields[i].FieldType + "赋值出错。");
                            Debug.LogError("\r\n----------------------------------------------------\r\n");
                            return false;
                        }
                    }
                    else
                    {
                        if ((fields[i].FieldType == typeof (int)
                             || fields[i].FieldType == typeof (uint)
                             || fields[i].FieldType == typeof (short)
                             || fields[i].FieldType == typeof (ushort)) && LString[index] == "")
                        {
                        }
                        else if ((fields[i].FieldType == typeof (float)
                                  || fields[i].FieldType == typeof (double)) && LString[index] == "")
                        {
                        }
                        else if (fields[i].FieldType == typeof (int))
                        {
                            var intTemp = ChangStringToInt(LString[index]);
                            fields[i].SetValue(objTemp, intTemp);
                        }
                        else
                        {
                            fields[i].SetValue(objTemp, Convert.ChangeType(LString[index], fields[i].FieldType));
                        }
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += ex.ToString();
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += "DBC.COMMON_DBC<T>.ChangeListStringToStruct方法出错。";
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += "出错的字段在类型为" + typeTemp + "的数据表第 " + (Dicts.Count + 3) + " 行,第 " + (index + 1) +
                                           " 列。";
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";

                throw new Exception(DataBaseSystem.StrError);
            }
            return true;
        }

        /// <summary>
        ///     打开并读取文件
        /// </summary>
        /// <param name="filename">要打开的文件夹的文件名</param>
        /// <returns>文件是否打开成功</returns>
        public bool OpenFromTXT<T>(string filename, DataBaseSystem.GetResources GetRes)
        {
            try
            {
                if (filename == string.Empty)
                {
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    Debug.LogError("要打开的文件名为空!");
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    return false;
                }

                var contents = GetRes(filename);

                if (contents == null || contents.Length == 0 || contents == "")
                {
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    Debug.LogError(filename + " 的FileObject is Null! ");
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    return false;
                }

                var strsTemp = contents.Split(new string[1] {"\r\n"}, StringSplitOptions.None);

                //从数据表中读取第一行(数据表所有数据对应的类型枚举),并将其存放到List中
                var ListTemp = new List<string>();

                if (Split_To_StringList(strsTemp[0], ListTemp))
                {
                    foreach (var str in ListTemp)
                    {
                        if (str == "INT")
                        {
                            FieldTypeList.Add(FIELD_TYPE.T_INT);
                        }
                        else if (str == "FLOAT")
                        {
                            FieldTypeList.Add(FIELD_TYPE.T_FLOAT);
                        }
                        else if (str == "STRING")
                        {
                            FieldTypeList.Add(FIELD_TYPE.T_STRING);
                        }
                        else
                        {
                            if (str.Equals(""))
                            {
                                Debug.LogError("\r\n----------------------------------------------------\r\n");
                                Debug.LogError("读取类型为" + typeof (T) + "的数据表第一行出错。第一行列数有未进行标记。");
                                Debug.LogError("\r\n----------------------------------------------------\r\n");
                            }
                            else
                            {
                                Debug.LogError("\r\n----------------------------------------------------\r\n");
                                Debug.LogError("读取类型为" + typeof (T) + "的数据表第一行出错。可能是编码格式不正确。");
                                Debug.LogError("\r\n----------------------------------------------------\r\n");
                            }

                            return false;
                        }
                    }
                }
                else
                {
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    Debug.LogError("读取类型为" + typeof (T) + "的数据表第一行类型行失败。");
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    return false;
                }

                //从数据表中读取第二行(此行为数据表对应结构的成员参数名行，或者此行为各列的解释行)
                if (!Split_To_StringList(strsTemp[1], strList))
                {
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    Debug.LogError("数据表结构的成员参数行或者各列的解释行为null 。");
                    Debug.LogError("\r\n----------------------------------------------------\r\n");
                    return false;
                }

                //读取数据，从第四行起，至倒数第二行，跳过第三行解释行和最后一行空行
                for (var index = 2; index < strsTemp.Length - 1; index++)
                {
                    var LineStatusTemp = ReadOneLineFromTXT(strsTemp[index], ListTemp);
                    if (LineStatusTemp == TXTLineStatus.LINE_EMPTY)
                    {
                        break;
                    }
                    if (LineStatusTemp == TXTLineStatus.LINE_ANNOTATE || LineStatusTemp == TXTLineStatus.LINE_ERROR)
                    {
                        continue;
                    }

                    var objTemp = Activator.CreateInstance(typeof (T));

                    //按照数据ID将数据结构存入hashtable
                    if (ChangeListStringToStruct(ListTemp, objTemp))
                    {
                        //StructDict.Add(ChangStringToInt(ListTemp[0]), (T)objTemp);
                        Dicts.Add(ChangStringToInt(ListTemp[0]), (T) objTemp);
                    }
                    else
                    {
                        Debug.LogError("\r\n----------------------------------------------------\r\n");
                        Debug.LogError("对类型为" + typeof (T) + "的数据表的第" + (index + 1) + "行进行反射赋值失败。");
                        Debug.LogError("\r\n----------------------------------------------------\r\n");
                        return false;
                    }
                }

                m_iDataNum = Dicts.Count;
                return true;
            }
            catch (Exception ex)
            {
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += "类型为" + typeof (T) + "的OpenFromTXT方法出错。" + "出错的表名为: " + filename;
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += ex.ToString();
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";

                throw new Exception(DataBaseSystem.StrError);
            }
        }

        /// <summary>
        ///     根据索引返回数据结构
        /// </summary>
        /// <param name="index">存放数据表数据的Dictionary所对应的key值</param>
        /// <returns>数据表对应该key值的一条数据数据</returns>
        public T Search_Index_EQU<T>(int index)
        {
            try
            {
                if (Dicts.ContainsKey(index))
                {
                    return (T) Dicts[index];
                }
                return default(T);
            }
            catch (Exception ex)
            {
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += "从类型为" + typeof (T) + "的表读索引为" + index + "的结果出错。";
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";
                DataBaseSystem.StrError += ex.ToString();
                DataBaseSystem.StrError += "\r\n----------------------------------------------------\r\n";

                throw new Exception(DataBaseSystem.StrError);
            }
        }
    }

}