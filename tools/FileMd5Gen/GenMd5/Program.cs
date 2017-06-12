using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GenMd5
{
    class Program
    {

        private static StringBuilder _fileInfoes;
        private static StringBuilder _apkInfo;
        private static string _basePath;
        private static readonly List<string> FilterNames = new List<string> { "version.txt", "versionNum.txt", "apkInfo.txt", "ICSharpCode.SharpZipLib.dll", "Iteedee.ApkReader.dll", "TD.apk", "NewApkInfo.txt" };

        static void Main(string[] args)
        {
            _fileInfoes = new StringBuilder();
            _apkInfo = new StringBuilder();
            _basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            _fileInfoes.Clear();
            _apkInfo.Clear();


            //获取版本号
            int versionNum = GetVersionNum() + 1;
            _fileInfoes.Append("#versionNum").Append(",").Append(versionNum).Append("\n");
            _fileInfoes.Append("#versionDate").Append(",").Append(DateTime.Now.ToString()).Append("\n");

            //重新记录版本号
            SaveVersionNum(versionNum);

            String[] fileNames = Directory.GetFiles(_basePath, "*", SearchOption.AllDirectories);
            foreach (String fileName in fileNames)
            {
                string fileNameShort = fileName.Substring(_basePath.Length);

                if (fileName.Contains(".exe") || fileName.Contains(".meta") || fileNameShort.Contains(".apk") || FilterNames.Contains(fileNameShort))
                    continue;

                string md5 = GetMD5HashFromFile(fileName);

                _fileInfoes.Append(fileNameShort.Replace("\\", "/")).Append(",").Append(md5).Append("\n");
            }

            saveFile();

        }

        private static int GetVersionNum()
        {
            string fileName = _basePath + "/" + "versionNum.txt";

            if (!File.Exists(fileName))
                return 0;

            StreamReader sr = new StreamReader(fileName, Encoding.Default);
            var versionNum = int.Parse(sr.ReadLine());
            sr.Close();
            return versionNum;
        }

        private static void SaveVersionNum(int num)
        {
            string fileName = _basePath + "/" + "versionNum.txt";
            GenFile(fileName, num.ToString());
        }

        private static void saveFile()
        {
            string fileName = _basePath + "/" + "version.txt";
            GenFile(fileName, _fileInfoes.ToString());
        }

        private static string GetMD5HashFromFile(string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(file);

                StringBuilder result = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("x2"));
                }
                return result.ToString();
            }
        }

        private static void GenFile(string path, string content)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            byte[] data = new UTF8Encoding().GetBytes(content);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }
    }
}
