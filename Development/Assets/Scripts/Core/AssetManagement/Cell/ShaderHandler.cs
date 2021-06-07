//��Ծ
using AssetManagement;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShaderHandler
{

    //ӵ����ж��
    public static string[] necessary_asset = new string[3]
    {
        "002/shader.asset",//�Զ���shader
        "002/packages/comunityrenderpipelinescore.asset",//URP���
        "002/packages/comunityrenderpipelinesuniversal.asset",//URP���

    };


    private static AssetBundle m_AssetBundle;
    private static Dictionary<string, Shader> m_shaders = new Dictionary<string, Shader>();
    public static IEnumerator InitShader()
    {
        if (!Launcher.assetBundleMode) yield return null;

        Stopwatch watch = new Stopwatch();
        watch.Start();
        foreach (var item in necessary_asset)
        {
            string down_path;
            string save_path;
            string load_path;//����·��
            if (AssetUtility.CheckExists(item, out down_path, out save_path, out load_path))
            {
                GameDebug.Log(string.Format("��ShaderHandler���������ر�ҪShader��{0}", down_path));
    
                XAssetsFiles.FileStruct fStruct = UpdateManager.Instance.TryGetFileStruct(item);
                yield return AssetHttpRequestManager.Instance.RequestFile(fStruct.path, 1, fStruct.md5);
            }


            GameDebug.Log(string.Format("��ShaderHandler�����ر�Ҫ��Դ��{0}", load_path));
            AssetBundle Bundle = AssetBundle.LoadFromFile(load_path);
            AssetbundleCache.Add(item, Bundle, true);

        }

        //�����Զ���Shader
        m_AssetBundle = AssetbundleCache.TryGet(necessary_asset[0]).ab;
        watch.Stop();
        GameDebug.Log(string.Format("��ShaderHandler��Shader AssetBundle.shader = {0}.  time = {1} ms", m_AssetBundle != null, watch.ElapsedMilliseconds));
    }

   

    public static Shader GetShader(string shaderName)
    {
        Shader shader = null;
        if (m_shaders.TryGetValue(shaderName, out shader)) return shader;

        shader = m_AssetBundle.LoadAsset<Shader>(shaderName);

        if(shader != null)
        {
            m_shaders.Add(shaderName, shader);
        }

        return shader;
    }


    public static void Dispose()
    {

        m_shaders.Clear();

        if(m_AssetBundle != null) m_AssetBundle.Unload(true);

        m_AssetBundle = null;
    }
}
