using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class XMask : BaseMeshEffect
{
    [SerializeField]
    Sprite sprite;

    [SerializeField]
    float maskRotateSpeed = 0.0f;

    [Range(-1, 1)]
    [SerializeField]
    float uvAdd = 0f;

    public float p_UvAdd
    {
        set
        {
            uvAdd = value;
            if(material != null)
            {
                material.SetFloat("_addUV", uvAdd);
            }


        }
    }

    private Material material;
    protected override void Start()
    {
        base.Start();
        Shader sh = ShaderHandler.GetShader(maskRotateSpeed > 0 ? "X_Shader/G_GUI/MaskRotate" : "X_Shader/G_GUI/MaskGray");

        if (graphic && graphic.canvas)
            graphic.canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;

        if (sh != null)
        {
            material = new Material(sh);
            if (sprite != null)
            {
                material.SetTexture("_MaskTex", sprite.texture);
                if (maskRotateSpeed > 0)
                {
                    material.SetFloat("_RotateSpeed", maskRotateSpeed);
                }
            }

            if (graphic)
            {
                if (graphic.material && graphic.material.shader.name == "X_Shader/G_GUI/Gray")
                {
                    graphic.material = material;
                    graphic.material.SetFloat("_Saturation", 0);
                }
                else
                {
                    graphic.material = material;
                }
            }
            material.SetFloat("_uvAdd", uvAdd);
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() && graphic != null || !Application.isPlaying)
            return;

        int vertCount = vh.currentVertCount;
        var vert = new UIVertex();
        if (maskRotateSpeed == 0)
        {
            Vector4 uv1 = sprite != null ? DataUtility.GetOuterUV(sprite) : Vector4.zero;

            for (int i = 0; i < vertCount; ++i)
            {
                vh.PopulateUIVertex(ref vert, i);

                if (i == 0)
                    vert.uv1.Set(uv1.x, uv1.y, vert.uv1.z, vert.uv1.w);
                else if (i == 1)
                    vert.uv1.Set(uv1.x, uv1.w, vert.uv1.z, vert.uv1.w);
                else if (i == 2)
                    vert.uv1.Set(uv1.z, uv1.w, vert.uv1.z, vert.uv1.w);
                else if (i == 3)
                    vert.uv1.Set(uv1.z, uv1.y, vert.uv1.z, vert.uv1.w);

                vh.SetUIVertex(vert, i);
            }

            return;
        }


        Image image = graphic as Image;
        if (!image || image.overrideSprite == null) return;
        Vector4 uv = image.overrideSprite != null ? DataUtility.GetOuterUV(image.overrideSprite) : Vector4.zero;
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;
        if (uvWidth == 0 || uvHeight == 0)
        {
            return;
        }

        for (int i = 0; i < vertCount; ++i)
        {
            vh.PopulateUIVertex(ref vert, i);

            vert.uv1.x = (vert.uv0.x - uv.x) / uvWidth;
            vert.uv1.y = (vert.uv0.y - uv.y) / uvHeight;
            vh.SetUIVertex(vert, i);
        }
    }

    protected override void OnDestroy()
    {
        if (material)
            DestroyImmediate(material);
        base.OnDestroy();
    }
}
