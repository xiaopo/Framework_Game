using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using UnityEngine.Networking;

public class SFileUtility
{
    public static Sprite ReadStreamingImg(string path, out string error)
    {
        error = string.Empty;
        Sprite sprite = null;
        Texture2D texture = null;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS
        try
        {
            byte[] bytes = File.ReadAllBytes(path);
            texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        catch (System.Exception e) { error = e.ToString(); }
#elif UNITY_ANDROID
        UnityWebRequest www = new UnityWebRequest(path);
        DownloadHandlerTexture hand = new DownloadHandlerTexture();
        www.downloadHandler = hand;
        www.SendWebRequest();
        while (!www.isDone)
            System.Threading.Thread.Sleep(5);
        error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            texture = hand.texture;
   
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            www.Dispose();
        }
#endif
        return sprite;
    }

    public static string ReadStreamingFile(string path, out string error)
    {
        error = string.Empty;
        string data = string.Empty;
        
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS
        try
        {
            data = File.ReadAllText(path);
        }
        catch (System.Exception e) { error = e.ToString(); }
#elif UNITY_ANDROID
        UnityWebRequest www = new UnityWebRequest(path);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SendWebRequest();
        while (!www.isDone)
            System.Threading.Thread.Sleep(5);
        error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            data = www.downloadHandler.text;
            www.Dispose();
        }
#endif
        return data;
    }


    public static AssetBundle ReadStreamingAssetBundle(string path, out string error)
    {

        error = string.Empty;
        AssetBundle data = null;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS
        try
        {
            if (File.Exists(path)) 
                data = AssetBundle.LoadFromFile(path);
            else
                error = "file not exist!  " + path;
        }
        catch (System.Exception e) { error = e.ToString(); }
#elif UNITY_ANDROID
        UnityWebRequest www = new UnityWebRequest(path);
        DownloadHandlerAssetBundle hand = new DownloadHandlerAssetBundle(www.url, 0);
        www.downloadHandler = hand;
        www.SendWebRequest();
        while (!www.isDone)
            System.Threading.Thread.Sleep(5);

        error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            data = hand.assetBundle;
            www.Dispose();
        }
 #endif
        return data;
    }
    
    public static Sprite ReadStreamingImgEx(string fileName, out string error)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            error = "path is error";
            return null;
        }

        return ReadStreamingImg(Path.Combine(AssetDefine.streamingAssetsPath, fileName), out error);
    }

    public static AssetBundle ReadStreamingAssetBundleEx(string fileName, out string error)
    {
        return ReadStreamingAssetBundle(Path.Combine(AssetDefine.streamingAssetsPath, fileName), out error);
    }

    public static AssetBundle ReadPersistentAssetBundle(string fileName, out string error)
    {
        error = string.Empty;
        string path = Path.Combine(AssetDefine.ExternalSDCardsPath, fileName);
        if (File.Exists(path))
        {
            try
            {
                return AssetBundle.LoadFromFile(path);
            }
            catch (Exception ex)
            {
                error = ex.ToString() + path;
            }
        }

        error = "file not exist!  " + path;
        return null;
    }

    public static void WriteText(string path, string data)
    {
        if (File.Exists(path))
            File.Delete(path);
        else
        {
            string parent = Path.GetDirectoryName(path);
            if (!Directory.Exists(parent))
                Directory.CreateDirectory(parent);
        }

        File.WriteAllText(path, data);
    }

    public static void WriteBytes(string path, byte[] bytes)
    {
        if (File.Exists(path))
            File.Delete(path);
        else
        {
            string parent = Path.GetDirectoryName(path);
            if (!Directory.Exists(parent))
                Directory.CreateDirectory(parent);
        }

        File.WriteAllBytes(path, bytes);
    }


    public static string FileMd5(string file)
    {
        return MD5Utility.FileMd5(file);
    }
}
