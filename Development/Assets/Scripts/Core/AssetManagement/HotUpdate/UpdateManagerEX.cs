//龙跃
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using FileStruct = XAssetsFiles.FileStruct;
using FileOptions = XAssetsFiles.FileOptions;

/// <summary>
/// 这里放功能函数
/// </summary>
namespace AssetManagement
{
    //尽量使用回调完成
    public partial  class UpdateManager: SingleTemplate<UpdateManager>
    {

        protected void TextFileRequest<T>(string fileName, out T v1, Action<T> nextFun, bool needUnCompress = false)
        {
            //1.加载local文件
            //本地文件
            v1 = LoadFile<T>(fileName, needUnCompress);

            T vv2 = default(T);
            //2.下载web网络文件
            string localvpath = Path.Combine(AssetDefine.ExternalSDCardsPath, fileName);
            string vpath = AssetDefine.RemoteDownloadUrls[0] + fileName;
            UnityWebRequestHandler.GetWebAssetByte2(vpath, localvpath, delegate (DownloadHandler handle)
            {
                string filetxt = handle.text;
                
                if (needUnCompress)//需要Lz4解压
                    filetxt = LZ4Sharp.LZ4Util.DecompressString(filetxt);

                vv2 = JsonUtility.FromJson<T>(filetxt);
               
            
                nextFun.Invoke(vv2);

            }, delegate ()
            {
                string loadInfo = string.Format("无法链接资源服务器! {0},\n{1}", vpath, "请查看DownloadSetting.asset");
                GameDebug.Log(loadInfo);
                //无法下载记录文件
                LauncherGUIManager.Instance.Alert(loadInfo);
            });


        }
        //本地加载
        protected T LoadFile<T>(string fileName,bool needUnCompress = false,int pathTag = -1)
        {

            string filestr = "";

            if(pathTag == -1 || pathTag == 1)
            {
                string localvpath = Path.Combine(AssetDefine.ExternalSDCardsPath, fileName);
                if (File.Exists(localvpath))//扩展卡加载
                    filestr = File.ReadAllText(localvpath);
            }


            if (pathTag == -1 || pathTag == 2)
            {
                if (string.IsNullOrEmpty(filestr))
                {
                    //apk 内加载
                    string sdvpth = Path.Combine(AssetDefine.BuildinAssetPath, fileName);

                    filestr = SFileUtility.ReadStreamingFile(sdvpth, out var error);
                }
            }

 
            if (!string.IsNullOrEmpty(filestr))
            {
                if (needUnCompress)//需要Lz4解压
                    filestr = LZ4Sharp.LZ4Util.DecompressString(filestr);

                T vf = JsonUtility.FromJson<T>(filestr);
                return vf;
            }

            return default(T);
        }
      
        public static bool CompareVersion(XVersionFile Local, XVersionFile Web)
        {
            if (Local == null) return true;//本地文件为空是 启动了空包

            if (Local.p_LuaVersion.svnVer != Web.p_LuaVersion.svnVer ||
                Local.p_DevVersion.svnVer != Web.p_DevVersion.svnVer ||
                Local.p_ArtVersion.svnVer != Web.p_ArtVersion.svnVer) return true;

            return false;
        }


        public void NeedUpdateload(FileStruct wbfile,ref bool needDown)
        {
            needDown = false;
            string sdCardPath = Path.Combine(AssetDefine.ExternalSDCardsPath, wbfile.path);
            if (!File.Exists(sdCardPath))
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS
                string streamPath = Path.Combine(AssetDefine.BuildinAssetPath, wbfile.path);
                if (!File.Exists(streamPath)) needDown = true;
     
#elif UNITY_ANDROID
                if (!IsBuildInAsset(wbfile.path)) needDown = true;//包内不存在
#endif

            }

            if (needDown) return;//本地不存在必须下载

            if (L_XAssetFiles != null)
            {
                if (L_XAssetFiles.allFilesMap.TryGetValue(wbfile.path, out FileStruct lofile))
                {
                    if (lofile.md5 != wbfile.md5)
                    {
                        needDown = true;//本地记录与Web记录 md5不对
                    }
                }
            }

        }

    }
}