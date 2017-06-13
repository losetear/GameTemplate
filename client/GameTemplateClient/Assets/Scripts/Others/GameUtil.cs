using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameTemplate
{
    public class GameUtil : MonoBehaviour
    {
        public static void AddFps(GameObject parentGo)
        {
            GameObject fps = Instantiate(Resources.Load("UI/FPS")) as GameObject;
            fps.gameObject.transform.parent = parentGo.transform.Find("Camera (eye)");
        }

        #region 组件辅助接口

        /// <summary>
        ///     添加组件
        /// </summary>
        public static T Add<T>(GameObject go) where T :
            Component
        {
            if (go != null)
            {
                var ts = go.GetComponents<T>();
                for (var i = 0; i < ts.Length; i++)
                {
                    if (ts[i] != null)
                        Object.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        ///     添加组件
        /// </summary>
        public static T Add<T>(Transform go) where T :
            Component
        {
            return Add<T>(go.gameObject);
        }

        #region 渲染

        /// <summary>
        /// 设置为黑影
        /// </summary>
        /// <param name="go"></param>
        public static void ResetBlack(GameObject go)
        {
            if (!go)
                return;
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].sharedMaterials == null)
                    continue;
                for (var j = 0; j < renderers[i].sharedMaterials.Length; j++)
                {
                    if (renderers[i].sharedMaterials[j] != null)
                    {
                        renderers[i].sharedMaterials[j].shader = Shader.Find("Unlit/Color");
                        renderers[i].sharedMaterials[j].color = Color.black;
                    }
                }
            }
        }

        /// <summary>
        /// 不设置公共材质球，防止大厅玩家同时变黑
        /// </summary>
        /// <param name="go"></param>
        public static void ResetSingleBlack(GameObject go)
        {
            if (!go)
                return;
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].materials == null)
                    continue;
                for (var j = 0; j < renderers[i].materials.Length; j++)
                {
                    if (renderers[i].materials[j] != null)
                    {
                        renderers[i].materials[j].shader = Shader.Find("Unlit/Color");
                        renderers[i].materials[j].color = Color.black;
                    }
                }
            }
        }

        public static void ResetSkinMeshBlack(GameObject go)
        {
            if (!go)
                return;
            var renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].materials == null)
                    continue;
                for (var j = 0; j < renderers[i].materials.Length; j++)
                {
                    if (renderers[i].materials[j] != null)
                    {
                        renderers[i].materials[j].shader = Shader.Find("Unlit/Color");
                        renderers[i].materials[j].color = Color.black;
                    }
                }
            }
        }

        public static void ResetUnLightShader(GameObject go)
        {
            if (!go)
                return;
            string sname = "Unlit/Texture";
            Shader shader = Shader.Find(sname);
            if (shader == null)
                return;
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].materials == null)
                    continue;
                for (var j = 0; j < renderers[i].materials.Length; j++)
                {
                    if (renderers[i].materials[j] != null)
                    {
                        renderers[i].materials[j].shader = shader;
                    }
                }
            }
        }

        public static string GetBaseShader()
        {
            return "Custom/XRay";
            //return "Mobile/Diffuse";
        }

        public static void ResetSelfShaders(GameObject go, string sname)
        {
            if (go == null)
                return;
            Shader shader = Shader.Find(sname);

            if (shader == null)
                return;
            var renderers = go.GetComponents<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].sharedMaterials == null)
                    continue;
                for (var j = 0; j < renderers[i].sharedMaterials.Length; j++)
                {
                    if (renderers[i].sharedMaterials[j] != null)
                    {
                        renderers[i].sharedMaterials[j].shader = shader;
                    }
                }
            }
            Projector pro = go.GetComponent<Projector>();
            if (pro != null)
                pro.material.shader = shader;
        }

        public static void ResetSkinedMeshShaders(GameObject go, string sname)
        {
            if (go == null)
                return;
            Shader shader = Shader.Find(sname);
            if (shader == null)
                return;
            var renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].sharedMaterials == null)
                    continue;
                for (var j = 0; j < renderers[i].sharedMaterials.Length; j++)
                {
                    if (renderers[i].sharedMaterials[j] != null)
                    {
                        renderers[i].sharedMaterials[j].shader = shader;
                    }
                }
            }
        }

        public static void ResetParticles(GameObject go)
        {
            Shader shader = Shader.Find("Particles/Additive");
            if (shader == null || !go)
                return;
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].sharedMaterials == null)
                    continue;
                for (var j = 0; j < renderers[i].sharedMaterials.Length; j++)
                {
                    if (renderers[i].sharedMaterials[j] != null
                        && renderers[i].sharedMaterials[j].shader.name.Equals("Mobile/Particles/Additive"))
                    {
                        renderers[i].sharedMaterials[j].shader = shader;
                    }
                }
            }
        }

        public static void RetShader(GameObject go)
        {
            if (go == null)
                return;
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].sharedMaterials == null)
                    continue;
                for (var j = 0; j < renderers[i].sharedMaterials.Length; j++)
                {
                    if (renderers[i].sharedMaterials[j] != null)
                    {
                        renderers[i].sharedMaterials[j].shader = Shader.Find(renderers[i].sharedMaterials[j].shader.name);
                    }
                }
            }
            renderers = null;
        }

        public static void RetShader(Material mat)
        {
            if (mat == null)
                return;
            mat.shader = Shader.Find(mat.shader.name);
        }

        public static void TurnGrayBuild(GameObject go)
        {
            go.transform.Find("weikaiqi").gameObject.SetActive(true);
            go.transform.Find("kaiqi").gameObject.SetActive(false);
            GameUtil.ResetSelfShaders(go, "Custom/Gray_Diffuse");

            //这个着色器需要第二张贴图
            Texture2D lineTex = Resources.Load("Texture/BuildTurnLight") as Texture2D;

            var renderers = go.GetComponents<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].sharedMaterials == null)
                    continue;
                for (var j = 0; j < renderers[i].sharedMaterials.Length; j++)
                {
                    if (renderers[i].sharedMaterials[j] != null)
                    {
                        renderers[i].sharedMaterials[j].SetTexture("_LineTex", lineTex);
                    }
                }
            }
        }

        public static void TurnLightBuild(GameObject go)
        {
            go.transform.Find("weikaiqi").gameObject.SetActive(false);
            go.transform.Find("kaiqi").gameObject.SetActive(true);
            GameUtil.ResetSelfShaders(go, "Mobile/Diffuse");
        }

        #endregion

        #endregion

        #region 递归设置标签

        public static void RecursiveMarkTag(GameObject parentGameObject, string tag)
        {
            parentGameObject.tag = tag;
            foreach (Transform child in parentGameObject.transform)
            {
                child.gameObject.tag = tag;
                RecursiveMarkTag(child.gameObject, tag);
            }
        }

        #endregion

        #region 递归设置层级

        public static void RecursiveMarkLayer(GameObject parentGo, string layer)
        {
            parentGo.layer = LayerMask.NameToLayer(layer);
            foreach (Transform child in parentGo.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer(layer);
                RecursiveMarkLayer(child.gameObject, layer);
            }
        }

        #endregion

        #region 递归查询父节点组件

        public static T RecursiveGetParentComp<T>(GameObject child) where T :
            Component
        {
            if (child.GetComponentInChildren<T>() != null)
                return child.GetComponentInChildren<T>();
            if (child.transform.parent)
                return RecursiveGetParentComp<T>(child.transform.parent.gameObject);
            else
                return null;
        }

        #endregion

        public static bool Probability(float happen) //概率函数
        {
            int rand = UnityEngine.Random.Range(0, 10000);
            return rand < (int)(happen * 10000);
        }

        #region Des加密
        private static string _key = "td.matrixgame.cn.*#FDlk12";
        public static string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
        public static string DesEncrypt(string strEncryptString)
        {
            StringBuilder strRetValue = new StringBuilder();

            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(_key.Remove(0, _key.Length - 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(strEncryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB,//兼容其他语言的Des加密算法  
                    Padding = PaddingMode.Zeros                //自动补0
                };

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();

                //组织成16进制字符串            
                foreach (byte b in mStream.ToArray())
                {
                    strRetValue.AppendFormat("{0:X2}", b);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return strRetValue.ToString();
        }

        public static string DesDecrypt(string strDecryptString)
        {
            string strRetValue = "";

            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(_key.Remove(0, _key.Length - 8));
                byte[] keyIV = keyBytes;

                //16进制转换为byte字节
                byte[] inputByteArray = new byte[strDecryptString.Length / 2];
                for (int x = 0; x < strDecryptString.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(strDecryptString.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                DESCryptoServiceProvider provider = new DESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB,//兼容其他语言的Des加密算法  
                    Padding = PaddingMode.Zeros//自动补0  
                };

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();

                strRetValue = Encoding.UTF8.GetString(mStream.ToArray()).TrimEnd('\0');
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return strRetValue;
        }
        #endregion


        #region Folder
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetLongPathName(
            string path,
            StringBuilder longPath,
            int longPathLength
            );

        /// <summary>
        /// Return true if file exists. Non case sensitive by default.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public static bool FileExists(string filename, bool caseSensitive = false)
        {
            filename = filename.Replace('/', '\\');
            if (!File.Exists(filename))
            {
                return false;
            }

            if (!caseSensitive)
            {
                return true;
            }

            //check case
            StringBuilder longPath = new StringBuilder(255);
            GetLongPathName(Path.GetFullPath(filename), longPath, longPath.Capacity);

            string realPath = Path.GetDirectoryName(longPath.ToString());
            return Array.Exists(Directory.GetFiles(realPath), s => s == filename);
        }
        #endregion
    }
}