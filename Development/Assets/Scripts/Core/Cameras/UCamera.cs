
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class UCamera : MonoBehaviour
{

    private Camera m_camera;
    public new Camera camera
    { 
        get 
        { 
            if(m_camera == null) m_camera = this.gameObject.GetComponent<Camera>();

            return m_camera; 
        } 
    }


    private UniversalAdditionalCameraData m_cameraData;
    public UniversalAdditionalCameraData cameraData 
    { 
        get 
        {
            if (m_cameraData == null)
                m_cameraData = camera.GetUniversalAdditionalCameraData();

            return m_cameraData; 
        }
    }

    public void SwitchRenderType(CameraRenderType type)
    {
        cameraData.renderType = type;
    }

    /// <summary>
    /// 注意OverCamera相机的设置
    /// https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/manual/cameras-advanced.html
    /// </summary>
    /// <param name="overCamera"></param>
    public void AttachOverlayCamera(Camera overCamera)
    {
       
        cameraData.cameraStack.Add(overCamera);
    }


    public void InsertOverlayCamera(Camera overCamera)
    {

        cameraData.cameraStack.Insert(0,overCamera);
    }

    
    public void RemoveOverlayCamera(Camera overCamera)
    {
        for (int i = 0; i < cameraData.cameraStack.Count; i++)
        {
            if (cameraData.cameraStack[i] == overCamera)
            {
                cameraData.cameraStack.RemoveAt(i);
                i--;
            }
        }
    }

    public void TopOverlayCamera(Camera overCamera)
    {
   
        for(int i = 0;i < cameraData.cameraStack.Count;i++)
        {
            if(cameraData.cameraStack[i] == overCamera)
            {
                cameraData.cameraStack.RemoveAt(i);
                cameraData.cameraStack.Add(overCamera);
                break;
            }
        }
    }
}
