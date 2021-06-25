//��Ծ
using System;
using System.Collections;
using UnityEngine;

namespace AssetManagement
{
    /// <summary>
    /// �Դ��������ڵĹ�����
    /// ���ǵ��������ⲻ��Ӱ����Ϸ�߼�
    /// </summary>
    public class AssetProgram : SingleBehaviourTemplate<AssetProgram>
    {
        private GameLoadOption _loadOptions;
        public GameLoadOption loadOptions { get { return _loadOptions; } }

        private Action _enterGame;

        /// <summary>
        /// ��ʼ����Դ����
        /// </summary>
        /// <param name="callFun"></param>
        public void OnInitialize(Action callFun)
        {
            AssetDefine.RemoteDownloadUrls = DownloadSetting.Instance.RemoteUrls;
            _enterGame = callFun;

            if(DownloadSetting.Instance.downAssetModle == DownAssetModle.WHOLE_OFFLINE)
            {
                GameDebug.Log("2.����ģʽ");
                UpdateDone();
            }
            else
            {
                //�ȸ���
                UpdateManager.Instance.update_done += UpdateDone;
                UpdateManager.Instance.Start();
            }
          
        }


        //�ȸ������
        private void UpdateDone()
        {
            StartCoroutine(PrepareGame());
        }

        IEnumerator PrepareGame()
        {
            GameDebug.Log("3.׼�������Ҫ��Դ");

            LauncherGUIManager.Instance.UpdatePage.Mesage = "��ʼ����Դ";

           _loadOptions = new GameLoadOption();
            yield return loadOptions.Initialize(Launcher.assetBundleMode);


            //-------------���ر�Ҫ��Դ---------------
            yield return LoadNecessaryAsset();


            if(_enterGame != null)
            {
                _enterGame.Invoke();
                _enterGame = null;
            }
        }

        //׼�������ı�Ҫ��Դ
        IEnumerator LoadNecessaryAsset()
        {

            //1.shader AB�������ڴ�
            yield return ShaderHandler.InitShader();

            //2.lua AB�������ڴ�
            yield return LuaLauncher.Instance.InitLuaEnv();

            //3.�����
            yield return GameCameraUtiliy.LoadMainCamera();
        }


        public void OnUpdate()
        {

            if (Time.frameCount % 5 == 0)
            {
                UpdateManager.Instance.Update();//�ȸ��£���̨����
            }

            if (Time.frameCount % 6 == 0)
            {
                AssetsGetManger.Instance.Update();//��Դ�ڴ����

                AssetBundleManager.Instance.Update();//AssetBundle���̼���

                AssetHttpRequestManager.Instance.Update();//http���ļ�����
            }


            if (Time.frameCount % Application.targetFrameRate == 0)
            {

                //����ͷ�
                AssetRawobjCache.DumpTrash();
            }
        }

    }
}
