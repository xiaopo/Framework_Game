//龙跃
using System;
using System.Collections;
using UnityEngine;

namespace AssetManagement
{
    /// <summary>
    /// 自带生命周期的管理器
    /// 考虑到报错问题不能影响游戏逻辑
    /// </summary>
    public class AssetProgram : SingleBehaviourTemplate<AssetProgram>
    {
        private GameLoadOption _loadOptions;
        public GameLoadOption loadOptions { get { return _loadOptions; } }

        private Action _enterGame;

        /// <summary>
        /// 初始化资源管理
        /// </summary>
        /// <param name="callFun"></param>
        public void OnInitialize(Action callFun)
        {
            AssetDefine.RemoteDownloadUrls = DownloadSetting.Instance.RemoteUrls;
            _enterGame = callFun;

            if(DownloadSetting.Instance.downAssetModle == DownAssetModle.WHOLE_OFFLINE)
            {
                GameDebug.Log("2.单机模式");
                UpdateDone();
            }
            else
            {
                //热更新
                UpdateManager.Instance.update_done += UpdateDone;
                UpdateManager.Instance.Start();
            }
          
        }


        //热更新完毕
        private void UpdateDone()
        {
            StartCoroutine(PrepareGame());
        }

        IEnumerator PrepareGame()
        {
            GameDebug.Log("3.准备载入必要资源");

            LauncherGUIManager.Instance.UpdatePage.Mesage = "初始化资源";

           _loadOptions = new GameLoadOption();
            yield return loadOptions.Initialize(Launcher.assetBundleMode);


            //-------------加载必要资源---------------
            yield return LoadNecessaryAsset();


            if(_enterGame != null)
            {
                _enterGame.Invoke();
                _enterGame = null;
            }
        }

        //准备启动的必要资源
        IEnumerator LoadNecessaryAsset()
        {

            //1.shader AB包载入内存
            yield return ShaderHandler.InitShader();

            //2.lua AB包载入内存
            yield return LuaLauncher.Instance.InitLuaEnv();

            //3.主相机
            yield return GameCameraUtiliy.LoadMainCamera();
        }


        public void OnUpdate()
        {

            if (Time.frameCount % 5 == 0)
            {
                UpdateManager.Instance.Update();//热更新，后台下载
            }

            if (Time.frameCount % 6 == 0)
            {
                AssetsGetManger.Instance.Update();//资源内存加载

                AssetBundleManager.Instance.Update();//AssetBundle磁盘加载

                AssetHttpRequestManager.Instance.Update();//http端文件下载
            }


            if (Time.frameCount % Application.targetFrameRate == 0)
            {

                //检查释放
                AssetRawobjCache.DumpTrash();
            }
        }

    }
}
