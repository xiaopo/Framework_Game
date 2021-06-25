
using UnityEngine;

public class GUIAvatarManager :MonoBehaviour
{
    public static GUIAvatarManager OnInitialize(Transform parent)
    {
        GameObject gameobject = new GameObject("Avatars");
        gameobject.transform.SetParent(parent);
        return gameobject.AddComponent<GUIAvatarManager>();
    }

    /**
        统一管理UI上模型显示
        方法一
            摄像机渲染到RenderTexture上,GUIAvatarRenderTexture

        方法二
            摄像机直接渲染到 Screen，GUIAvatarScreen

        方法三
            调整模型的sort order,由 GUIAvatarSortOrder
        
        管理节点 GUI/Avatars
    **/



}
