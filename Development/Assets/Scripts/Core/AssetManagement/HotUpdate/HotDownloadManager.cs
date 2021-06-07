
using FileStruct = XAssetsFiles.FileStruct;
using FileOptions = XAssetsFiles.FileOptions;
using System.Collections.Generic;

namespace AssetManagement
{

    /// <summary>
    /// 热更新下载管理器
    /// </summary>
    public class HotDownloadManager: DownloadQueueManager
    {
        private static HotDownloadManager _instance;
        public static HotDownloadManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HotDownloadManager();
                    _instance.OnInitialize();
                }

                return _instance;
            }
        }

        protected override void DownloadDone(FileStruct item)
        {
            LauncherGUIManager.Instance.UpdatePage.Mesage = string.Format("热更：{0}/{1}", SUtility.FormatBytes(this.DownloadSize), SUtility.FormatBytes(this.DownloadTotalSize));

            LauncherGUIManager.Instance.UpdatePage.RunProgress(m_finishList.Count, totalCount, 0.4f);
        }

    }
}
