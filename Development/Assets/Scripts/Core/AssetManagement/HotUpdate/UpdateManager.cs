//龙跃
using System;
using FileStruct = XAssetsFiles.FileStruct;
using FileOptions = XAssetsFiles.FileOptions;
using System.Collections.Generic;
using System.Threading;
using System.IO;
/// <summary>
/// 热更新模块
/// 步骤
/// 1.下载web Version文件 和 加载local Version 文件
/// 2.下载web File.txt 和加载local File.txt
/// 3.对比有没有版本变化
///     没有
///        筛选出 后台下载清单 第 6 步
///     有
///         第 4 步
///
/// 4.对比两份文件筛选出、启动下载清单，后台下载清单
/// 5.执行启动下载界面读条
/// 6.下载完毕进入下一阶段
/// 
/// </summary>

namespace AssetManagement
{
    //尽量使用回调完成
    public partial class UpdateManager: SingleTemplate<UpdateManager>
    {
        public const string versionFileName = "version.txt";
        public const string fileListFileName = "files.txt";

        //本地文件可能是空，sdcard没有
        protected XVersionFile L_XVersionFile = null;
        protected XAssetsFiles L_XAssetFiles = null;

        //streamAssets
        protected XAssetsFiles B_XAssetFiles = null;
        public XAssetsFiles BXAssetFiles { get { return B_XAssetFiles; } }
        /// <summary>
        /// Unity内部路径，由Assets开始
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

        //Web 文件不可能为空，一定会去网络下载
        protected XVersionFile W_XVersionFile = null;
        protected XAssetsFiles W_XAssetFiles = null;
        public XVersionFile WXVersionFile { get { return W_XVersionFile; } }
        public XAssetsFiles WXAssetFiles { get { return W_XAssetFiles; } }

        public FileStruct TryGetFileStruct(string abPath)
        {
            //只返回本次最新的对象
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

        //热更新完毕
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

            //加载  Version.txt
            TextFileRequest<XVersionFile>(versionFileName,out L_XVersionFile, delegate (XVersionFile vv2) {
                W_XVersionFile = vv2;
                invaildVersionAnalysis = true;
                //等待Update 调用 VersionAnalysis()

                LauncherGUIManager.Instance.UpdatePage.DrawVersion();
            });
        }

        #region 热更新逻辑.....
        protected void VersionAnalysis()
        {
            //加载首保内记录文件
            B_XAssetFiles = LoadFile<XAssetsFiles>(fileListFileName, true,2);
            if(B_XAssetFiles == null)
            {
                GameDebug.Log(string.Format("======================UpdateManager.VersionAnalysis {0}", "安装包错误，找不到file.txt文件"));
                return;
            }

            if (CompareVersion(L_XVersionFile, W_XVersionFile))
            {
                //有版本变化需要热更
                //下载AB记录文件
                GameDebug.Log(string.Format("======================UpdateManager.VersionAnalysis {0}", "开始热更!"));
                TextFileRequest<XAssetsFiles>(fileListFileName, out L_XAssetFiles, delegate(XAssetsFiles vv2){
                    this.W_XAssetFiles = vv2;
                    invaildFileAnalysis = true;
                    //等待Update 调用 FileAnalysis()
                }, true);
            }
            else
            {
                GameDebug.Log(string.Format("======================UpdateManager.VersionAnalysis {0}", "没有版本变化!"));
                //没有版本变化
                //不需要下载网络AB记录文件和assetminifest文件, 以免造成本地加载错误
                //直接是要本地AB记录文件
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
            //file.txt 收集启动下载文件

            //启动下载归类:LAUNCHDOWNLOAD,LUA,DLL 
            //后台下载归类:NONE,BUILDING
            //下载判断条件
            // 1.md5 码不同
            // 2.本地不存在

            List<FileStruct> needStartDown = new List<FileStruct>();
            long totalSize = 0;
            foreach (var map in W_XAssetFiles.allFilesMap)
            {
                FileStruct wbfile = map.Value;//最新web文件



                bool needCheck = (wbfile.options & FileOptions.LAUNCHDOWNLOAD) > 0 ||
                                 (wbfile.options & FileOptions.LUA) > 0 ||
                                 (wbfile.options & FileOptions.DLL) > 0 ||
                                 (wbfile.options & FileOptions.INSTALL) > 0;


                bool needDown = false;
                if (needCheck)
                    NeedUpdateload(wbfile, ref needDown);//检查是否需要下载

                if (needDown)
                {
                    totalSize += wbfile.size;
                    needStartDown.Add(wbfile);
                }

            }


            GameDebug.Log(string.Format("======================UpdateManager.CollectUpdateloadFiles 热更 Count {0}  Size {1}", needStartDown.Count, SUtility.FormatBytes(totalSize)));
            HotDownloadManager.Instance.DownloadTotalSize = totalSize;


            mainThreadSynContext.Send(new SendOrPostCallback((ss) =>
            {
                LauncherUpdateDownLoad(needStartDown);
            }), null);//通知主线程
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
            GameDebug.Log(string.Format("======================UpdateManager.HotDownloadComplete {0}", "热更新完毕!"));
            StartGame();

            OnBackGorundDownLoadAnlysis();
        }

        #endregion

        protected void StartGame()
        {
            update_done.Invoke();//开始游戏
        }

        #region 资源后台下载逻辑
        protected void OnNotvariety()
        {

            StartGame();

            //同时检查后台下载
            TextFileRequest<XAssetsFiles>(fileListFileName,out L_XAssetFiles, delegate(XAssetsFiles vv2)
            {
                this.W_XAssetFiles =  vv2;
                this.invaildBackgroundAnalysis = true;
                //在Update 中调用 OnBackGorundDownLoadAnlysis
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
        /// 子线程方法
        /// </summary>
        /// <param name="obj"></param>
        protected void CollectBackGroundFiles(object obj)
        {
            //收集后台下载文件
            //后台下载归类:NONE,BUILDING
            List<FileStruct> needStartDown = new List<FileStruct>();
            long totalSize = 0;
            foreach (var map in W_XAssetFiles.allFilesMap)
            {
                FileStruct wbfile = map.Value;//最新web文件



                bool needCheck = (wbfile.options & FileOptions.NONE) == FileOptions.NONE ||
                                 (wbfile.options & FileOptions.LAUNCHDOWNLOAD) == 0;


                bool needDown = false;
                if (needCheck)
                    NeedUpdateload(wbfile,ref needDown);//检查是否需要下载

                if (needDown)
                {
                    totalSize += wbfile.size;
                    needStartDown.Add(wbfile);
                }

            }

            GameDebug.Log(string.Format("======================UpdateManager.CollectBackGroundFiles 后台下载 Count {0}  Size {1}", needStartDown.Count, SUtility.FormatBytes(totalSize)));
            BackDownloadManager.Instance.DownloadTotalSize = totalSize;

            mainThreadSynContext.Send(new SendOrPostCallback((ss) =>
            {
                BackDownloadManager.Instance.Start(needStartDown);//开启后台下载
            }), null);//通知主线程
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