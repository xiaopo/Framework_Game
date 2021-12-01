//龙跃
using System;
using System.Collections;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetManagement
{
    /// <summary>
    /// 注意：
    ///     只能下载非 file.txt 记录的文件，file.txt 记录的文件请使用 AssetHttpRequestManager.Instance
    /// </summary>
    public class UnityWebRequestHandler
    {
        //下载文件直接保存到磁盘中
        public static IEnumerator GetWebFile(string path, string savePath, Action loadDone = null)
        {
            using (UnityWebRequest uwr = new UnityWebRequest(path, UnityWebRequest.kHttpVerbGET))
            {
                string folderName = Path.GetDirectoryName(savePath);
                if (!File.Exists(folderName)) Directory.CreateDirectory(folderName);

                DownloadHandlerFile hand = new DownloadHandlerFile(savePath);
                hand.removeFileOnAbort = true;//执行 abort时，移除正在下载的对象
                uwr.downloadHandler = hand;

                if (File.Exists(savePath)) File.Delete(savePath);

                yield return uwr.SendWebRequest();


#if UNITY_2020
               if (uwr.result != UnityWebRequest.Result.Success)
#else
               if (uwr.error != null)
#endif
                {
                    throw new Exception("UnityWebRequest download had an error\n" + path);
                }



                if (uwr.downloadHandler.isDone)
                    loadDone?.Invoke();


            }


        }


        private static void AssetByteHandler(UnityWebRequest uwr, string path, string savePath, Action<DownloadHandler> loadDone = null, Action loadError = null)
        {
#if UNITY_2020
            if (uwr.result != UnityWebRequest.Result.Success)
#else
            if (uwr.error != null)
#endif
            {
                loadError?.Invoke();
                throw new Exception("UnityWebRequest download had an error\n" + path);
            }

            if (uwr.downloadHandler.isDone)
            {
                byte[] results = uwr.downloadHandler.data;

                string folderName = Path.GetDirectoryName(savePath);
                if (!File.Exists(folderName)) Directory.CreateDirectory(folderName);

                if (File.Exists(savePath)) File.Delete(savePath);//删除本地旧文件

                FileInfo fileInfo = new FileInfo(savePath);
                FileStream fs = fileInfo.Create();

                //fs.Write(字节数组, 开始位置, 数据长度);
                fs.Write(results, 0, results.Length);

                fs.Flush();     //文件写入存储到硬盘
                fs.Close();     //关闭文件流对象
                fs.Dispose();   //销毁文件对象


                loadDone?.Invoke(uwr.downloadHandler);

            }

        }


        public static void GetWebAssetByte2(string path, string savePath, Action<DownloadHandler> loadDone = null, Action loadError = null,int timeout = 2)
        {
            GameDebug.Log("WebRequest: " + path);
            UnityWebRequest uwr = new UnityWebRequest(path);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.timeout = timeout;
            UnityWebRequestAsyncOperation uao = uwr.SendWebRequest();

            uao.completed += delegate (AsyncOperation aa) 
            {
                AssetByteHandler(uwr, path, savePath, loadDone, loadError);

                uwr.Dispose();
            };

        }

        /// <summary>
        /// 以字节方式下载 同时保存到本地
        /// </summary>
        /// <returns></returns>
        public static IEnumerator GetWebAssetByte(string path, string savePath, Action<DownloadHandler> loadDone = null,Action loadError = null, int timeout = 2)
        {
            using (UnityWebRequest uwr = new UnityWebRequest(path))
            {
                uwr.timeout = timeout;
                uwr.downloadHandler = new DownloadHandlerBuffer();
                
                yield return uwr.SendWebRequest();
                AssetByteHandler(uwr, path, savePath, loadDone, loadError);
            }
        }

        public static IEnumerator GetWebTexture(string path, string savePath, Action<Sprite> loadDone = null, int timeout = 2)
        {
            using (UnityWebRequest uwr = new UnityWebRequest(path))
            {
                uwr.timeout = timeout;
                DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
                uwr.downloadHandler = texDl;

                yield return uwr.SendWebRequest();

#if UNITY_2020
                if (uwr.result != UnityWebRequest.Result.Success)
#else
               if (uwr.error != null)
#endif
                {
                    throw new Exception("UnityWebRequest download had an error\n" + path);
                }

                if (uwr.downloadHandler.isDone)
                {
                    Texture2D t = texDl.texture;
                    Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height),Vector2.zero, 1f);

                    loadDone?.Invoke(s);
                }
            }
        }
    }
}
