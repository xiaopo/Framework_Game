
using FileStruct = XAssetsFiles.FileStruct;
using FileOptions = XAssetsFiles.FileOptions;
using System.Collections.Generic;
using System;

namespace AssetManagement
{
    public class DownloadQueueManager
    {
        /// <summary>
        /// 是否输出下载进度
        /// </summary>
        public bool logProgress = false;

        public event Action oncomplete;

        /// <summary>
        /// 一个一个下载
        /// </summary>

        protected List<FileStruct> m_waitlist;
        protected List<FileStruct> m_finishList;
        public List<FileStruct> FinishList { get { return m_finishList; } }
        //当前正在下载的文件信息
        protected FileStruct m_fileStruct;

        //下载器
        protected HttpRequest m_loader;

        protected int totalCount = 0;
        
        /// <summary>
        /// 返回下载进度
        /// </summary>
        public float Progress { get { return m_finishList.Count / totalCount; } }
        /// <summary>
        /// 返回下载Size
        /// </summary>
        public long DownloadSize { set; get; }
        public long DownloadTotalSize { get; set; }
        protected bool m_start;
        protected virtual void OnInitialize()
        {
            m_finishList = new List<FileStruct>();
            m_start = false;
        }

        public virtual void Start(List<FileStruct> list)
        {
            m_waitlist = list;
            totalCount = list.Count;

            m_waitlist.Sort((x,y)=>-x.priority.CompareTo(y.priority));

            m_fileStruct = Next();

            m_start = true;
        }

        public virtual void Start()
        {
            m_start = true;
        }

        public virtual void Pause()
        {
            m_start = false;
        }


        protected virtual FileStruct Next()
        {
            if (m_waitlist.Count == 0) return null;
            FileStruct curent;
            curent = m_waitlist[0];
            m_waitlist.RemoveAt(0);

            return curent;
        }

        protected virtual void DownloadDone(FileStruct item)
        {
           
        }
        public virtual void Update()
        {
            if (!m_start) return;

            if (m_fileStruct == null) return;


            if (m_loader == null)
                m_loader = AssetHttpRequestManager.Instance.RequestFile(m_fileStruct.path, 1, m_fileStruct.md5);
            else
            {
                if(logProgress)
                    GameDebug.Log(string.Format("{0} == {1}", m_loader.progress, m_loader.webUrl));

                if(m_loader.isDown)
                {
                    //下载完成
                    m_finishList.Add(m_fileStruct);
                    DownloadSize += m_fileStruct.size;
                    DownloadDone(m_fileStruct);
         
                    //m_loader.Dispose();
                    m_loader = null;
                    //下一帧开始下一个下载
                    m_fileStruct = Next();
                }
               
            }

            if (m_fileStruct == null)
            {
                m_start = false;
                oncomplete?.Invoke();
                oncomplete = null;
            }


        }

        public void Dispose()
        {
            m_start = false;
            oncomplete = null;
            m_loader = null;
        }
    }
}
