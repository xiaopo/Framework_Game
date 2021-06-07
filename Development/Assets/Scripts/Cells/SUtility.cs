using UnityEngine;
using System.Collections;
using System.Text;

public class SUtility
{
    public static System.Reflection.Assembly LoadAssembly(string file)
    {
        return System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(file));
    }

    public static string FormatBytes(long bytes)
    {
        string result = string.Empty;
        if (bytes >= 1048576)
            result = string.Format("{0} MB", (bytes / 1048576f).ToString("f2"));
        else if (bytes >= 1024)
            result = string.Format("{0} KB", (bytes / 1024f).ToString("f2"));
        else
            result = string.Format("{0} B", bytes);
        return result;
    }

    public static string FormatBytes(double bytes)
    {
        return FormatBytes((long)bytes);
    }

    //加层
    public static int AddMask(int target, params int[] masks)
    {
        for (int i = 0; i < masks.Length; i++)
            target |= 1 << masks[i];
        return target;
    }

    //减层
    public static int SubMask(int target, params int[] masks)
    {
        for (int i = 0; i < masks.Length; i++)
            target &= ~(1 << masks[i]);
        return target;
    }


    public static bool ScreenPointToScene(Vector3 screenPos, out RaycastHit hitInfo, params string[] layerMask)
    {
        screenPos = GameCameraUtiliy.mainCamera.ViewportToScreenPoint(GameCameraUtiliy.uiCamera.ScreenToViewportPoint(screenPos));
        Ray ray = GameCameraUtiliy.mainCamera.ScreenPointToRay(screenPos);
        bool b = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(layerMask));
        Debug.DrawRay(ray.origin, ray.direction * 1000f, b ? Color.green : Color.red, 3f);
        return b;
    }


    //当前游戏版本
    public static string GetGameVerion()
    {
        StringBuilder sb = new StringBuilder();
        if (XAssetsFiles.s_CurrentVersion != null)
        {
            sb.AppendFormat("Art     {0}", Application.version);
            sb.AppendFormat(".{0}", XAssetsFiles.s_CurrentVersion.p_ArtVersion.svnVer);
            sb.Append("\n");
            sb.AppendFormat("Dev    {0}", Application.version);
            sb.AppendFormat(".{0}", XAssetsFiles.s_CurrentVersion.p_DevVersion.svnVer);
        }
        else
        {
            sb.Append(Application.version);
        }

        return sb.ToString();
    }

    //是否有连上wifi/4g/或有线
    public static bool IsNetworkValid() { return Application.internetReachability != NetworkReachability.NotReachable; }
    //是否为wifi下
    public static bool IsNetworkWifi() { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; }
    //是否为移动网络
    public static bool IsNetwork4G() { return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork; }
}
