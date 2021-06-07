using AssetManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{ 
    /// <summary>
    /// 登录阶段场景加载
    /// 场景可以摆放相机，否则会创建一个OverCamera
    /// </summary>
    public class SingleSceneLoader : GameMapLoader
    {
        protected UCamera m_camera;
        public UCamera UCamera { get { return m_camera; } }
        public string cameraTag = "MapCamera";
     
        protected override void InitScene(Scene scene)
        {
            GameObject[] objects = scene.GetRootGameObjects();

            GameObject cameraObj = null;
            foreach (var item in objects)
            {
                if(item.CompareTag(cameraTag))
                {
                    cameraObj = item;
                    break;
                }
            }

            if (cameraObj == null)
                cameraObj = new GameObject("MapCamera");

            //需要把相机添加到Stack

            m_camera = cameraObj.TryGetComponent<UCamera>();


            GameCameraUtiliy.InsertOverlayCamera(m_camera);
        }

        protected override void OnUnLoadScene(Scene cur)
        {
            base.OnUnLoadScene(cur);

            GameCameraUtiliy.RemoveOverlayCamera(m_camera);
            m_camera = null;
        }
    }
}