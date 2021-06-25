
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
        ͳһ����UI��ģ����ʾ
        ����һ
            �������Ⱦ��RenderTexture��,GUIAvatarRenderTexture

        ������
            �����ֱ����Ⱦ�� Screen��GUIAvatarScreen

        ������
            ����ģ�͵�sort order,�� GUIAvatarSortOrder
        
        ����ڵ� GUI/Avatars
    **/



}
