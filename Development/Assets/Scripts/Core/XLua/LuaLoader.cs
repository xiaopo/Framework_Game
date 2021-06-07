//龙跃
using AssetManagement;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using XLua;

public class LuaLoader 
{
    public static string s_LuaAssetBundleName = "000/00000000000000000000000000000000.asset";

    private static string _luaPath;
    private AssetBundle _scriptsAssetBundle;
    public bool isEditorLua;
    public LuaLoader()
    {

        _luaPath = Application.dataPath + "/../Lua_Scripts/{0}.lua";

    }

    public IEnumerator LoadLuaAssetBundle()
    {
        if (!Launcher.assetBundleMode || Launcher.UseEditorLua)
        {
            isEditorLua = true;
            yield break;
        }

        Stopwatch watch = new Stopwatch();
        watch.Start();

        string down_path;
        string save_path;
        string load_path;//加载路径
        if (AssetManagement.AssetUtility.CheckExists(s_LuaAssetBundleName, out down_path, out save_path, out load_path))
        {
            GameDebug.Log("【LuaLoader】纠正下载Lua：" + down_path);
            XAssetsFiles.FileStruct fStruct = UpdateManager.Instance.TryGetFileStruct(s_LuaAssetBundleName);
            yield return AssetHttpRequestManager.Instance.RequestFile(fStruct.path, 1, fStruct.md5);
        }

        _scriptsAssetBundle = AssetBundle.LoadFromFile(load_path);
        //lua 不参与管理
       // AssetbundleCache.Add(load_path, _scriptsAssetBundle, true);
        watch.Stop();
        GameDebug.Log(string.Format("LuaLoader::LoadLuaAssetBundle.  luaScript = {0}.  time = {1} ms", _scriptsAssetBundle != null, watch.ElapsedMilliseconds));
        
    }

    private byte[] GetLuaScript(string codeName)
    {
        if (_scriptsAssetBundle != null)
        {
            GameDebug.BeginSample("LuaLoader.GetLuaScript LoadAsset " + codeName);
            TextAsset text = _scriptsAssetBundle.LoadAsset<TextAsset>(codeName);
            GameDebug.EndSample();

            byte[] bytes = null;
            if (text != null)
            {
                bytes = text.bytes;
                Resources.UnloadAsset(text);
            }

            return bytes;
        }
        return null;
    }

    byte[] LoadData(ref string filepath)
    {
       
        if (_scriptsAssetBundle != null)
        {
            //资源包模式
            byte[] bytes = GetLuaScript(filepath);
            if (bytes != null)
                return bytes;

        }

        //编辑器文件模式
        filepath = filepath.Replace(".", "/");
        filepath = string.Format(_luaPath, filepath);
        if (File.Exists(filepath))
        {
            byte[] bytes = File.ReadAllBytes(filepath);

            return bytes;
        }
        else
        {
            return null;
        }
    }

    public static implicit operator LuaEnv.CustomLoader(LuaLoader loader)
    {
        return loader.LoadData;
    }

    public void Dispose()
    {
        _scriptsAssetBundle.Unload(true);
        _scriptsAssetBundle = null;
    }
}
