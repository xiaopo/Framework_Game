//��Ծ
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using FileStruct = XAssetsFiles.FileStruct;
using FileOptions = XAssetsFiles.FileOptions;

/// <summary>
/// ����Ź��ܺ���
/// </summary>
namespace AssetManagement
{
    //����ʹ�ûص����
    public partial  class UpdateManager: SingleTemplate<UpdateManager>
    {

        protected void TextFileRequest<T>(string fileName, out T v1, Action<T> nextFun, bool needUnCompress = false)
        {
            //1.����local�ļ�
            //�����ļ�
            v1 = LoadFile<T>(fileName, needUnCompress);

            T vv2 = default(T);
            //2.����web�����ļ�
            string localvpath = Path.Combine(AssetDefine.ExternalSDCardsPath, fileName);
            string vpath = AssetDefine.RemoteDownloadUrls[0] + fileName;
            UnityWebRequestHandler.GetWebAssetByte2(vpath, localvpath, delegate (DownloadHandler handle)
            {
                string filetxt = handle.text;
                
                if (needUnCompress)//��ҪLz4��ѹ
                    filetxt = LZ4Sharp.LZ4Util.DecompressString(filetxt);

                vv2 = JsonUtility.FromJson<T>(filetxt);
               
            
                nextFun.Invoke(vv2);

            }, delegate ()
            {
                string loadInfo = string.Format("�޷�������Դ������! {0},\n{1}", vpath, "��鿴DownloadSetting.asset");
                GameDebug.Log(loadInfo);
                //�޷����ؼ�¼�ļ�
                LauncherGUIManager.Instance.Alert(loadInfo);
            });


        }
        //���ؼ���
        protected T LoadFile<T>(string fileName,bool needUnCompress = false,int pathTag = -1)
        {

            string filestr = "";

            if(pathTag == -1 || pathTag == 1)
            {
                string localvpath = Path.Combine(AssetDefine.ExternalSDCardsPath, fileName);
                if (File.Exists(localvpath))//��չ������
                    filestr = File.ReadAllText(localvpath);
            }


            if (pathTag == -1 || pathTag == 2)
            {
                if (string.IsNullOrEmpty(filestr))
                {
                    //apk �ڼ���
                    string sdvpth = Path.Combine(AssetDefine.BuildinAssetPath, fileName);

                    filestr = SFileUtility.ReadStreamingFile(sdvpth, out var error);
                }
            }

 
            if (!string.IsNullOrEmpty(filestr))
            {
                if (needUnCompress)//��ҪLz4��ѹ
                    filestr = LZ4Sharp.LZ4Util.DecompressString(filestr);

                T vf = JsonUtility.FromJson<T>(filestr);
                return vf;
            }

            return default(T);
        }
      
        public static bool CompareVersion(XVersionFile Local, XVersionFile Web)
        {
            if (Local == null) return true;//�����ļ�Ϊ���� �����˿հ�

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
                if (!IsBuildInAsset(wbfile.path)) needDown = true;//���ڲ�����
#endif

            }

            if (needDown) return;//���ز����ڱ�������

            if (L_XAssetFiles != null)
            {
                if (L_XAssetFiles.allFilesMap.TryGetValue(wbfile.path, out FileStruct lofile))
                {
                    if (lofile.md5 != wbfile.md5)
                    {
                        needDown = true;//���ؼ�¼��Web��¼ md5����
                    }
                }
            }

        }

    }
}