//龙跃
using System.Collections.Generic;
using UnityEngine.Networking;

namespace AssetManagement
{
    /// <summary>
    /// 这种专门的下载处理程序的优势在于能够将数据流式传输到 Unity 的 AssetBundle 系统。
    /// 一旦 AssetBundle 系统收到足够的数据，AssetBundle 就可以作为 UnityEngine.AssetBundle 对象使用。
    /// 仅会创建 UnityEngine.AssetBundle 对象的一个副本。
    /// 因此大大减少了运行时内存分配以及加载 AssetBundle 带来的内存影响。
    /// 此下载处理程序还允许在未完全下载的情况下部分使用 AssetBundle，因此可以流式传输资源。所有下载和解压缩都在工作线程上进行。
    /// </summary>
    public class AssetBundHttpRequest : AssetHttpRequest
    {
        /// <summary>
        /// AssetBundle 的 crc 码
        /// </summary>
        private uint _crc;
        public AssetBundHttpRequest(List<string> weburls, string savePath,uint crc) :base(weburls, savePath)
        {
            _crc = crc;
        }

        
        protected override void EstablishRequest(string weburl)
        {
            _webRequest = new UnityWebRequest(weburl);

            DownloadHandlerAssetBundle hand = new DownloadHandlerAssetBundle(_webRequest.url, _crc);
            _webRequest.downloadHandler = hand;

        }

        protected override void OnDealCompleted()
        {
            base.OnDealCompleted();


            _reqest.data = null;

        }
    }
}
