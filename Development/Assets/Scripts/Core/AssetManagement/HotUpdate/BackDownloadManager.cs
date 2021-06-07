using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement
{
    public class BackDownloadManager : DownloadQueueManager
    {
        private static BackDownloadManager _instance;
        public static BackDownloadManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BackDownloadManager();
                    _instance.OnInitialize();
                }

                return _instance;
            }
        }

        public override void Start()
        {
            base.Start();

            GameDebug.Log("开启后台下载 BackDownloadManager:Start()");
        }
    }
}
