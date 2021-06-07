//龙跃
using UnityEngine;

public enum DownAssetModle
{
    DLC_ONLINE,//网络资源模式，支持热更新
    WHOLE_OFFLINE//单机资源模式，只读本地资源
}

public class DownloadSetting : SingleScriptableObject<DownloadSetting>
{

    [Header("资源下载模式")]
    public DownAssetModle downAssetModle = DownAssetModle.DLC_ONLINE;


    [Header("远程资源http路径,可本机架设http服务器")]
    public string[] RemoteUrls = new string[1] { "http://10.11.132.169/assetBundles_tt/android/" };


    [Header("远程资源http下载并发最大数")]
    public int MaxWebRqeustNum = 3;

}
