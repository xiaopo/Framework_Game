//��Ծ
using System;
using FileStruct = XAssetsFiles.FileStruct;
using FileOptions = XAssetsFiles.FileOptions;
using System.Collections.Generic;
using System.Threading;
using System.IO;
/// <summary>
/// �ȸ���ģ��
/// ����
/// 1.����web Version�ļ� �� ����local Version �ļ�
/// 2.����web File.txt �ͼ���local File.txt
/// 3.�Ա���û�а汾�仯
///     û��
///        ɸѡ�� ��̨�����嵥 �� 6 ��
///     ��
///         �� 4 ��
///
/// 4.�Ա������ļ�ɸѡ�������������嵥����̨�����嵥
/// 5.ִ���������ؽ������
/// 6.������Ͻ�����һ�׶�
/// 
/// </summary>

namespace AssetManagement
{
    //����ʹ�ûص����
    public partial class UpdateManager: SingleTemplate<UpdateManager>
    {
        public const string versionFileName = "version.txt";
        public const string fileListFileName = "files.txt";

        //�����ļ������ǿգ�sdcardû��
        protected XVersionFile L_XVersionFile = null;
        protected XAssetsFiles L_XAssetFiles = null;

        //streamAssets
        protected XAssetsFiles B_XAssetFiles = null;
        public XAssetsFiles BXAssetFiles { get { return B_XAssetFiles; } }
        /// <summary>
        /// Unity�ڲ�·������Assets��ʼ
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsBuildInAsset(string path)
        {
            if (B_XAssetFiles.allFilesMap.TryGetValue(path, out var lofile)) return true;

            return false;
        }
        public XVersionFile LXVersionFile { get { return L_XVersionFile; } }
        public XAssetsFiles LXAssetsFiles { get { return L_XAssetFiles; } }

        //Web �ļ�������Ϊ�գ�һ����ȥ��������
        protected XVersionFile W_XVersionFile = null;
        protected XAssetsFiles W_XAssetFiles = null;
        public XVersionFile WXVersionFile { get { return W_XVersionFile; } }
        public XAssetsFiles WXAssetFiles { get { return W_XAssetFiles; } }

        public FileStruct TryGetFileStruct(string abPath)
        {
            //ֻ���ر������µĶ���
            if (W_XAssetFiles == null) return null;

            if (W_XAssetFiles.allFilesMap.TryGetValue(abPath, out FileStruct file)) return file;

            return null;
        }

        public string TryGetFileMD5(string abPath)
        {
            FileStruct info = TryGetFileStruct(abPath);
            if(info != null)
            {
                return info.md5;
            }

            return string.Empty;
        }

        //�ȸ������
        public event Action update_done;
        private bool b_start;

        protected bool invaildVersionAnalysis;
        protected bool invaildFileAnalysis;
        protected bool invaildBackgroundAnalysis;
        protected SynchronizationContext mainThreadSynContext;
        public void Start()
        {
            b_start = true;

             mainThreadSynContext = SynchronizationContext.Current;

            //����  Version.txt
            TextFileRequest<XVersionFile>(versionFileName,out L_XVersionFile, delegate (XVersionFile vv2) {
                W_XVersionFile = vv2;
                invaildVersionAnalysis = true;
                //�ȴ�Update ���� VersionAnalysis()

                LauncherGUIManager.Instance.UpdatePage.DrawVersion();
            });
        }

        #region �ȸ����߼�.....
        protected void VersionAnalysis()
        {
            //�����ױ��ڼ�¼�ļ�
            B_XAssetFiles = LoadFile<XAssetsFiles>(fileListFileName, true,2);
            if(B_XAssetFiles == null)
            {
                GameDebug.Log(string.Format("======================UpdateManager.VersionAnalysis {0}", "��װ�������Ҳ���file.txt�ļ�"));
                return;
            }

            if (CompareVersion(L_XVersionFile, W_XVersionFile))
            {
                //�а汾�仯��Ҫ�ȸ�
                //����AB��¼�ļ�
                GameDebug.Log(string.Format("======================UpdateManager.VersionAnalysis {0}", "��ʼ�ȸ�!"));
                TextFileRequest<XAssetsFiles>(fileListFileName, out L_XAssetFiles, delegate(XAssetsFiles vv2){
                    this.W_XAssetFiles = vv2;
                    invaildFileAnalysis = true;
                    //�ȴ�Update ���� FileAnalysis()
                }, true);
            }
            else
            {
                GameDebug.Log(string.Format("======================UpdateManager.VersionAnalysis {0}", "û�а汾�仯!"));
                //û�а汾�仯
                //����Ҫ��������AB��¼�ļ���assetminifest�ļ�, ������ɱ��ؼ��ش���
                //ֱ����Ҫ����AB��¼�ļ�
                L_XAssetFiles = LoadFile<XAssetsFiles>(fileListFileName, true);
                W_XAssetFiles = L_XAssetFiles;
                
                SignalActionTool.Check(OnNotvariety, Launcher.signalName);

            }
        }


        protected void FileAnalysis()
        {
            Thread th = new Thread(CollectUpdateloadFiles)
            {
                IsBackground = true,
                Priority = System.Threading.ThreadPriority.BelowNormal
            };
            th.Start();

        }

        protected void CollectUpdateloadFiles(object obj)
        {
            //file.txt �ռ����������ļ�

            //�������ع���:LAUNCHDOWNLOAD,LUA,DLL 
            //��̨���ع���:NONE,BUILDING
            //�����ж�����
            // 1.md5 �벻ͬ
            // 2.���ز�����

            List<FileStruct> needStartDown = new List<FileStruct>();
            long totalSize = 0;
            foreach (var map in W_XAssetFiles.allFilesMap)
            {
                FileStruct wbfile = map.Value;//����web�ļ�



                bool needCheck = (wbfile.options & FileOptions.LAUNCHDOWNLOAD) > 0 ||
                                 (wbfile.options & FileOptions.LUA) > 0 ||
                                 (wbfile.options & FileOptions.DLL) > 0 ||
                                 (wbfile.options & FileOptions.INSTALL) > 0;


                bool needDown = false;
                if (needCheck)
                    NeedUpdateload(wbfile, ref needDown);//����Ƿ���Ҫ����

                if (needDown)
                {
                    totalSize += wbfile.size;
                    needStartDown.Add(wbfile);
                }

            }


            GameDebug.Log(string.Format("======================UpdateManager.CollectUpdateloadFiles �ȸ� Count {0}  Size {1}", needStartDown.Count, SUtility.FormatBytes(totalSize)));
            HotDownloadManager.Instance.DownloadTotalSize = totalSize;


            mainThreadSynContext.Send(new SendOrPostCallback((ss) =>
            {
                LauncherUpdateDownLoad(needStartDown);
            }), null);//֪ͨ���߳�
        }

        protected void LauncherUpdateDownLoad(List<FileStruct> needStartDown)
        {

            if (needStartDown.Count > 0)
            {
                HotDownloadManager.Instance.Start(needStartDown);
                SignalActionTool.Single(1.0f, Launcher.signalName);
                HotDownloadManager.Instance.oncomplete += delegate() {
                    SignalActionTool.Check(HotDownloadComplete, Launcher.signalName);
                };
            }
            else
            {
                SignalActionTool.Check(HotDownloadComplete, Launcher.signalName);
            }
        }

       
        protected void HotDownloadComplete()
        {
            GameDebug.Log(string.Format("======================UpdateManager.HotDownloadComplete {0}", "�ȸ������!"));
            StartGame();

            OnBackGorundDownLoadAnlysis();
        }

        #endregion

        protected void StartGame()
        {
            update_done.Invoke();//��ʼ��Ϸ
        }

        #region ��Դ��̨�����߼�
        protected void OnNotvariety()
        {

            StartGame();

            //ͬʱ����̨����
            TextFileRequest<XAssetsFiles>(fileListFileName,out L_XAssetFiles, delegate(XAssetsFiles vv2)
            {
                this.W_XAssetFiles =  vv2;
                this.invaildBackgroundAnalysis = true;
                //��Update �е��� OnBackGorundDownLoadAnlysis
            }, true);
        }

        protected void OnBackGorundDownLoadAnlysis()
        {

            Thread th = new Thread(CollectBackGroundFiles)
            {
                IsBackground = true,
                Priority = System.Threading.ThreadPriority.BelowNormal
            };
            th.Start();

        }

        /// <summary>
        /// ���̷߳���
        /// </summary>
        /// <param name="obj"></param>
        protected void CollectBackGroundFiles(object obj)
        {
            //�ռ���̨�����ļ�
            //��̨���ع���:NONE,BUILDING
            List<FileStruct> needStartDown = new List<FileStruct>();
            long totalSize = 0;
            foreach (var map in W_XAssetFiles.allFilesMap)
            {
                FileStruct wbfile = map.Value;//����web�ļ�



                bool needCheck = (wbfile.options & FileOptions.NONE) == FileOptions.NONE ||
                                 (wbfile.options & FileOptions.LAUNCHDOWNLOAD) == 0;


                bool needDown = false;
                if (needCheck)
                    NeedUpdateload(wbfile,ref needDown);//����Ƿ���Ҫ����

                if (needDown)
                {
                    totalSize += wbfile.size;
                    needStartDown.Add(wbfile);
                }

            }

            GameDebug.Log(string.Format("======================UpdateManager.CollectBackGroundFiles ��̨���� Count {0}  Size {1}", needStartDown.Count, SUtility.FormatBytes(totalSize)));
            BackDownloadManager.Instance.DownloadTotalSize = totalSize;

            mainThreadSynContext.Send(new SendOrPostCallback((ss) =>
            {
                BackDownloadManager.Instance.Start(needStartDown);//������̨����
            }), null);//֪ͨ���߳�
        }
        #endregion
        public void Update()
        {
            if (!b_start) return;

            HotDownloadManager.Instance.Update();

            BackDownloadManager.Instance.Update();


            if(invaildVersionAnalysis)
            {
                invaildVersionAnalysis = false;
                VersionAnalysis();
               
            } 
            
            if(invaildFileAnalysis)
            {
                invaildFileAnalysis = false;
                FileAnalysis();
               
            }

            if(invaildBackgroundAnalysis)
            {
                invaildBackgroundAnalysis = false;
                OnBackGorundDownLoadAnlysis();
                
            }
        }

    }
}