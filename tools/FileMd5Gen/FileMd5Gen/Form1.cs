using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace FileMd5Gen
{
    public partial class Form1 : Form
    {
        private readonly StringBuilder _fileInfoes;
        private readonly StringBuilder _apkInfo;
        private readonly string _basePath;
        private readonly List<string> _filterNames = new List<string> { "version.txt", "versionNum.txt", "apkInfo.txt", "ICSharpCode.SharpZipLib.dll", "Iteedee.ApkReader.dll", "TD.apk", "NewApkInfo.txt" };

        public Form1()
        {
            InitializeComponent();
            _fileInfoes = new StringBuilder();
            _apkInfo = new StringBuilder();
            _basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        private void GenMd5(object sender, EventArgs e)
        {
            _fileInfoes.Clear();
            _apkInfo.Clear();
            info.Items.Clear();


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

                if (fileName.Contains(".exe") || fileName.Contains(".meta") || fileNameShort.Contains(".apk") || _filterNames.Contains(fileNameShort))
                    continue;

                string md5 = GetMD5HashFromFile(fileName);

                _fileInfoes.Append(fileNameShort.Replace("\\", "/")).Append(",").Append(md5).Append("\n");
                info.Items.Add(fileNameShort + "," + md5);
            }
            //GenApkInfo();
            //兼顾老版本
            //GenOldApkInfo();
            saveFile();
        }

        //获取历史版本号
        private int GetVersionNum()
        {
            string fileName = _basePath + "/" + "versionNum.txt";

            if (!File.Exists(fileName)) 
                return 0;

            StreamReader sr = new StreamReader(fileName, Encoding.Default);
            var versionNum = int.Parse(sr.ReadLine());
            sr.Close();
            return versionNum;
        }

        //记录历史版本号
        private void SaveVersionNum(int num)
        {
            string fileName = _basePath + "/" + "versionNum.txt";
            GenFile(fileName, num.ToString());
        }

        private void saveFile()
        {
            string fileName = _basePath + "/" + "version.txt";
            GenFile(fileName, _fileInfoes.ToString());
            MessageBox.Show("生成成功！");
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

//        //生成apk信息
//        private void GenApkInfo()
//        {
//            String[] fileNames = Directory.GetFiles(_basePath, "*.apk", SearchOption.AllDirectories);
//            JsonData json = new JsonData();
//            foreach (var fileName in fileNames)
//            {
//                string fileNameShort = fileName.Substring(_basePath.Length);
//                if (File.Exists(fileName))
//                {
//                    ApkInfo info = ReadApk.ReadApkFromPath(fileName);
//                    json[fileNameShort] = info.versionName;
//                }
//                else
//                    json[fileNameShort] = "";
//            }
//            string datastr = json.ToJson();
//            _apkInfo.Append(datastr);
//            string apkInfoFilePath = _basePath + "NewApkInfo.txt";
//            GenFile(apkInfoFilePath, _apkInfo.ToString());
//        }
//
//        private void GenOldApkInfo()
//        {
//            string APKfilePath = _basePath + "TD.apk";
//            string ApkInfoFilePath = _basePath + "apkInfo.txt";
//            ApkInfo info = ReadApk.ReadApkFromPath(APKfilePath);
//            StringBuilder oldData = new StringBuilder();
//            oldData.Append("versionName,").Append(info.versionName);
//            GenFile(ApkInfoFilePath, oldData.ToString());
//        }

        //生成文件
        private void GenFile(string path, string content)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            byte[] data = new UTF8Encoding().GetBytes(content);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }


    }
}

