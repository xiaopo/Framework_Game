//龙跃
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using UnityEngine;
/// <summary>
/// 此磁盘里加载 AB 资源
/// </summary>
namespace AssetManagement
{
    public class AssetBundleLoaderWebClient
    {
        public enum LoaderType
        {
            None,      //默认
            LocalSave, //sd卡
            Download,  //下载
        }


        public bool isDone;

        public string abPath;
        //private LoaderType _loaderType = LoaderType.None;
        private AssetBundleCreateRequest _abcreateRequest;
        private bool _isDownloading;
        private bool _isLoading;

        private WebClient _webClient;

        private string tempSavePath;
        private string savePath;
        public bool isLoading
        {
            get
            {
                return _isDownloading || _isLoading;
            }
        }

        public AssetBundleLoaderWebClient(string abPath)
        {
            this.abPath = abPath;
        }

        public void Update()
        {
            if (IsDone() || isLoading) return;

            //分别从  下载目标  安装目标  网络 加载资源

            string sdcard_path = Path.Combine(AssetDefine.ExternalSDCardsPath, abPath);
            string streaming_path = Path.Combine(AssetDefine.BuildinAssetPath, abPath);
            uint crc = AssetProgram.Instance.loadOptions.GetABCRCByABName(abPath);
            savePath = sdcard_path;//保存目录

            if (File.Exists(sdcard_path))
            {
                _isLoading = true;
                //下载目录AB资源
               
                _abcreateRequest = AssetBundle.LoadFromFileAsync(sdcard_path, crc);
            }
            else if(File.Exists(streaming_path))
            {
           
                _isLoading = true;
                //apk安装目录
                _abcreateRequest = AssetBundle.LoadFromFileAsync(streaming_path, crc);

            }
            else
            {
                _isDownloading = true;

                //需要去网络上下载,把资源下载到本地

                string webPath = Path.Combine(AssetDefine.RemoteDownloadUrls[0], abPath, "?v=0");

                _webClient = new WebClient();
                _webClient.DownloadFileCompleted += OnDownloadFileCompleted;
                _webClient.Disposed += OnDownloadDisposed;

                //创建下载对象的保存
                string saveFodler = Path.GetDirectoryName(sdcard_path);
                if (!Directory.Exists(saveFodler)) Directory.CreateDirectory(saveFodler);


                tempSavePath = sdcard_path + ".temp";
                _webClient.DownloadFileAsync(new Uri(webPath), tempSavePath);
            }
        }
        private void OnDownloadDisposed(object sender, EventArgs e)
        {

            if (_webClient != null)
            {
                _webClient.DownloadFileCompleted -= OnDownloadFileCompleted;
                _webClient.Disposed -= OnDownloadDisposed;
            }

        }
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Dispose();

            if (File.Exists(savePath))
                File.Delete(savePath);

            //File.Move(tempSavePath, savePath);

            _isDownloading = false;
        }

        public bool IsDone()
        {
            if (_abcreateRequest != null)
            {
                return _abcreateRequest.isDone;
            }

            return false;
        }

        public AssetBundle assetBundle
        {
            get
            {
                if (IsDone())
                {
                    return _abcreateRequest.assetBundle;
                }

                return null;
            }
        }

        public void Dispose()
        {
            _abcreateRequest = null;
            if (_webClient != null)
            {
                _webClient.CancelAsync();
                _webClient.Dispose();
                _webClient = null;
            }
        }

    }
}