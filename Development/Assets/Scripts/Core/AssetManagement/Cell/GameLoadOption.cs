using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetManagement
{
    public class GameLoadOption : DefaultAssetLoaderOptions
    {
        //编辑器资源
        protected  EAssetManifest _EAssetManifest = null;
        //AB包资源
        protected XAssetManifest _XAssetManifest = null;

      

        public const string assetmanifest = "assetmanifest";
        public const string xassetmanifest = "XAssetManifest.asset";


        private bool _assetBundleMode;
        protected bool issucces;
        public bool IsSucces { get { return issucces; } }
        public IEnumerator Initialize(bool bundleMode)
        {

            _assetBundleMode = bundleMode;
#if UNITY_EDITOR
            if (!bundleMode)
                _EAssetManifest = EAssetManifest.EditorLoadAssetManifest();
#endif

            if (!File.Exists(AssetDefine.ExternalSDCardsPath))
                Directory.CreateDirectory(AssetDefine.ExternalSDCardsPath);

            string localfestPath = Path.Combine(AssetDefine.ExternalSDCardsPath, assetmanifest);

            if (DownloadSetting.Instance.downAssetModle == DownAssetModle.WHOLE_OFFLINE)
            {
                //指定为单机资源模式
                //手机内必须包含全部资源
                if (File.Exists(localfestPath))
                {
                    AssetBundle Bundle = AssetBundle.LoadFromFile(localfestPath);
                    LoadAssetManifestDown(Bundle);
                    issucces = true;
                }
               
            }
            else
            {
                //联网
                string apath = AssetDefine.RemoteDownloadUrls[0] + assetmanifest;

                //立即远程下载，否则无法进入游戏
                yield return UnityWebRequestHandler.GetWebAssetByte(apath, localfestPath, delegate(DownloadHandler handle) {

                    AssetBundle Bundle = AssetBundle.LoadFromMemory(handle.data);
                    LoadAssetManifestDown(Bundle);
                     issucces = true;
                },delegate()
                {
                    issucces = false;
                    string loadInfo = string.Format("无法链接资源服务器! {0},\n{1}", AssetDefine.RemoteDownloadUrls[0] , "请查看DownloadSetting.asset");
                    //无法下载记录文件
                    LauncherGUIManager.Instance.Alert(loadInfo);
                    GameDebug.LogError(loadInfo);
                });
            }


        }

        protected void LoadAssetManifestDown(AssetBundle Bundle)
        {
            _XAssetManifest = Bundle.LoadAsset<XAssetManifest>(xassetmanifest);
            Bundle.Unload(false);
        }

        public override uint GetABCRCByABName(string assetBundlename)
        {
            if (_XAssetManifest == null) return 0;

            return _XAssetManifest.GetABCRCByABName(assetBundlename);
        }

        public override List<string> GetAllDepenceies(string assetBundlename)
        {
            if (_XAssetManifest == null) return null;

            return _XAssetManifest.GetAllDepenceies_string(assetBundlename);
        }

        public override string GetABPathByAssetName(string assetName)
        {
            if (_XAssetManifest == null) return assetName;


            return _XAssetManifest.GetABPathByAssetName(assetName);
        }

        /// <summary>
        /// 编辑器加载方式要把,weaponIcon.png 转换成 /Asset/GUI/Image/weponIcon.png
        /// </summary>
        /// <param name="asssetName"></param>
        /// <returns></returns>
        public override string GetAssetPathAtName(string asssetName)
        {
            if (!IsEditorLoad(asssetName))
                return asssetName;

            return _EAssetManifest.GetAssetPath(asssetName);
        }

        /// <summary>
        /// 非 assetAundle模式下，混合使用AssetBundle和编辑器资源，优先使用编辑器资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public override bool IsEditorLoad(string assetName)
        {

            if (_assetBundleMode || !Application.isEditor)
                return false;

            return _EAssetManifest.ContainsAsset(assetName);
        }

        /// <summary>
        /// 检测是否有这个资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public override bool ContainsAsset(string assetName, int checkTag = -1)
        {
            if(checkTag == -1)
            {
                //全部检查

                if (!_assetBundleMode && _EAssetManifest != null && _EAssetManifest.ContainsAsset(assetName))
                    return true;

                return _XAssetManifest != null && _XAssetManifest.ContainsAsset(assetName);
            }
            else if( checkTag == 1)
            {
                //只检查编辑器
                if (!_assetBundleMode && _EAssetManifest != null && _EAssetManifest.ContainsAsset(assetName))
                    return true;
            }
            else
            {
                //只检测包
                return _XAssetManifest != null && _XAssetManifest.ContainsAsset(assetName);
            }

            return false;
        }


    }
}