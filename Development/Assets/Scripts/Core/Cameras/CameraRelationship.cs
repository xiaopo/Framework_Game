//龙跃
using AssetManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Cameras
{
    /// <summary>
    /// 游戏相机
    /// Base Camera
    /// 
    /// </summary>
    

    public class CameraRelationship : SingleTemplate<CameraRelationship>
    {

        protected UCamera m_mainCamera;
        public UCamera MainCamera { get { return m_mainCamera; } }


        protected UCamera m_guiCamera;
        public UCamera GUICamera { get { return m_guiCamera; } }


        public void InitCamera()
        {
            GameObject mainCamobj = new GameObject("Main Camera");
            GameObject.DontDestroyOnLoad(mainCamobj);

            m_mainCamera = mainCamobj.AddComponent<UCamera>();

            //RenderType
            m_mainCamera.cameraData.renderType = CameraRenderType.Base;

            //Projection
            m_mainCamera.camera.orthographic = false;
            m_mainCamera.camera.fieldOfView = 30;
            m_mainCamera.camera.usePhysicalProperties = false;
            m_mainCamera.camera.nearClipPlane = 10;
            m_mainCamera.camera.farClipPlane = 1000;

            //Rendering
            m_mainCamera.cameraData.SetRenderer(-1);
            m_mainCamera.cameraData.renderPostProcessing = false;
            m_mainCamera.cameraData.antialiasing = AntialiasingMode.None;
            m_mainCamera.cameraData.stopNaN = false;
            m_mainCamera.cameraData.dithering = false;
            m_mainCamera.cameraData.renderShadows = true;
            m_mainCamera.camera.depth = 0;
            m_mainCamera.cameraData.requiresColorOption = CameraOverrideOption.UsePipelineSettings;
            m_mainCamera.cameraData.requiresDepthOption = CameraOverrideOption.UsePipelineSettings;
            m_mainCamera.camera.cullingMask = -1;
            m_mainCamera.camera.useOcclusionCulling = true;

            //Environment
            m_mainCamera.camera.clearFlags = CameraClearFlags.Skybox;
            m_mainCamera.cameraData.volumeLayerMask = LayerMask.NameToLayer("Default");
            m_mainCamera.cameraData.volumeTrigger = null;


        }
       
    }

}
