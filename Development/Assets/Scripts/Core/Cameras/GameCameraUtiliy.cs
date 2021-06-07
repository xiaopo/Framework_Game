
using AssetManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameCameraUtiliy
{
    public static string s_mainCamera = "Main_Camera.prefab";


    #region 主相机部分

    protected static UCamera m_mainCamera;
    public static UCamera UMainCamera { get { return m_mainCamera; } }
    public static Camera mainCamera { get { return m_mainCamera.camera; } }

 
    protected static MainCameraController m_mainCameraCtroler;
    /// <summary>
    /// 主相机控制器
    /// </summary>
    public static MainCameraController mmCameraCtrl { get { return m_mainCameraCtroler; } }
    public static IEnumerator LoadMainCamera(Transform parent = null)
    {
        //加载主相机预制体
        AssetLoaderParcel loader = AssetUtility.LoadAsset<GameObject>(s_mainCamera);
        yield return loader;

        UCamera mucam = loader.Instantiate<GameObject>().AddComponent<UCamera>();
       
        GameObject.DontDestroyOnLoad(mucam.gameObject);

        mucam.SwitchRenderType(CameraRenderType.Base);
        //相机AI控制脚本
        m_mainCameraCtroler =  mucam.TryGetComponent<MainCameraController>();
        mucam.tag = "MainCamera";

        m_mainCamera = mucam;
    }

    public static void AttachOverlayCamera(UCamera overlayCamera)
    {
        overlayCamera.SwitchRenderType(CameraRenderType.Overlay);
        m_mainCamera.AttachOverlayCamera(overlayCamera.camera);
    }

    public static void InsertOverlayCamera(UCamera overlayCamera)
    {
        overlayCamera.SwitchRenderType(CameraRenderType.Overlay);
        m_mainCamera.InsertOverlayCamera(overlayCamera.camera);
    }

    public static void TopOverlayCamera(UCamera overlayCamera)
    {
        if (overlayCamera.cameraData.renderType != CameraRenderType.Overlay) return;
        m_mainCamera.TopOverlayCamera(overlayCamera.camera);
    }

    public static void RemoveOverlayCamera(UCamera overlayCamera)
    {
        if (overlayCamera.cameraData.renderType != CameraRenderType.Overlay) return;
        m_mainCamera.RemoveOverlayCamera(overlayCamera.camera);
    }

    #endregion

    #region UI Camera 部分....

    protected static UCamera m_uiCamera;
    public static UCamera UUICamera { get { return m_uiCamera; } }
    public static Camera uiCamera { get { return m_uiCamera.camera; } }
    public static UCamera CreateUICamera(Transform parent = null)
    {
        //加载主相机预制体
        GameObject gameObject = new GameObject("GUI Camera");

        m_uiCamera = gameObject.AddComponent<UCamera>();
  
        m_uiCamera.cameraData.renderType = CameraRenderType.Overlay;
        m_uiCamera.camera.depth = 2;
        m_uiCamera.camera.clearFlags = CameraClearFlags.Depth;
        m_uiCamera.camera.useOcclusionCulling = false;

        GameCameraUtiliy.AttachOverlayCamera(m_uiCamera);
        return m_uiCamera;
    }

    #endregion

}


