Shader "ASGame/BRDF_Texture_Mask"
{
    //在切线空间计算光照模型
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color_Dif("Color InIt",Color) = (1,1,1,1)
        _Gloss("Gloss",Range(1.0,256)) = 8
        _Color_Specular("Color Specular",Color) = (1,1,1,1)

        _NormalTex("法线贴图",2D)="white"{}
        _BumpScale("_BumpScale",float) = 1

        _MaskTex("高光遮罩贴图",2D)="white"{}
        _MaskScale("遮罩比列",float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal:NORMAL;
                float2 texcoord : TEXCOORD0;
                float4 tangent:TANGENT;//切线
            };

            struct v2f
            {        
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD2;
                float2 maskuv : TEXCOORD3;

                float3 tangent_V:TEXCOORD4;
                float3 tangent_I:TEXCOORD5;
            };

            fixed4 _Color_Dif;
            fixed4 _Color_Specular;
            float _Gloss;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NormalTex;
            float4 _NormalTex_ST;
            float _BumpScale;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            float _MaskScale;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                o.uv.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
                o.uv.zw = TRANSFORM_TEX(v.texcoord,_NormalTex);
                o.maskuv = TRANSFORM_TEX(v.texcoord,_MaskTex);
                //得到旋转矩阵 t b n
                float3 binormal = cross(normalize(v.normal),normalize(v.tangent.xyz)) * v.tangent.w;
                float3x3 rotation = float3x3(v.tangent.xyz,binormal,v.normal);

                o.tangent_V = mul(rotation,normalize(ObjSpaceViewDir(v.vertex)));
                o.tangent_I = mul(rotation,normalize(ObjSpaceLightDir(v.vertex)));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //需要 V I H N
                float3 t_V = normalize(i.tangent_V);
                float3 t_I = normalize(i.tangent_I);

               // float3 N = normalize(i.worldNormal);  
                fixed4 albedo_N = tex2D(_NormalTex,i.uv.zw);

                float3 tangentNormal;
                //tangentNormal.xy = albedo_N.xy * 2 -1;
                tangentNormal.xy = UnpackNormal(albedo_N);
                tangentNormal.xy *= _BumpScale;
                tangentNormal.z = sqrt(1.0 - max(0,dot(tangentNormal.xy, tangentNormal.xy)));

                //纹理采样
                fixed3 albedo = tex2D(_MainTex, i.uv.xy).rgb * _Color_Dif.rgb;

                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

                //漫反射
                float3 diffuse = _LightColor0.rgb * albedo * max(0,dot(tangentNormal,t_I));

                //高光反射
                float3 H = normalize(t_V + t_I);

                float mask = tex2D(_MaskTex,i.maskuv).r * _MaskScale;

                float3 specular = _LightColor0.rgb * _Color_Specular * pow(max(0,dot(H,tangentNormal)),_Gloss) * mask;

                float3 col = ambient + diffuse + specular;
 
                return float4(col,1.0);
            }
            ENDCG
        }
    }
}
