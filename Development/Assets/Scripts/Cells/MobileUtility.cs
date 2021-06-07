using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class XMobileUtility
{
    /// <summary>
    /// 复制内容到剪切版
    /// </summary>
    /// <param name="value"></param>
    public static void CopyClipboard(string value)
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            using (AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
            {
                string clipboard = new AndroidJavaClass("android.content.Context").GetStatic<string>("CLIPBOARD_SERVICE");
                using (AndroidJavaObject clipboardManager = currentActivity.Call<AndroidJavaObject>("getSystemService", clipboard))
                {
                    clipboardManager.Call("setText", value);
                }
            }
#elif UNITY_IOS

#endif
    }

    /// <summary>
    /// 修改app的屏幕亮度
    /// </summary>
    /// <param name="Brightness">亮度值，Brightness的有效范围是0~1，-1。 若设置为-1则跟随系统亮度</param>
    public static void SetApplicationBrightness(float Brightness)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID 
            AndroidJavaObject Activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            Activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                AndroidJavaObject Window = null, Attributes = null;
                Window = Activity.Call<AndroidJavaObject>("getWindow");
                Attributes = Window.Call<AndroidJavaObject>("getAttributes");
                Attributes.Set("screenBrightness", Brightness);
                Window.Call("setAttributes", Attributes);
            }));
#endif
    }

    public static float GetApplicationBrightness()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID 
        using (AndroidJavaObject Activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject ContentResolver = Activity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass SystemSetting = new AndroidJavaClass("android.provider.Settings$System");
            float SystemBrightness = SystemSetting.CallStatic<int>("getInt", ContentResolver, "screen_brightness") / 256.0f;
            return SystemBrightness;
        }
#endif
        return 1.0f;
    }

    public static float GetActivityBrightness()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        using (AndroidJavaObject Activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            AndroidJavaObject Window = Activity.Call<AndroidJavaObject>("getWindow");
            AndroidJavaObject Attributes = Window.Call<AndroidJavaObject>("getAttributes");
            float Brightness = Attributes.Get<float>("screenBrightness");
            return Brightness;
        }
#endif
        return 1;
    }


    /// <summary>
    /// 重启游戏
    /// </summary>
    public static void RestartApplication()
    {
#if UNITY_EDITOR

#elif UNITY_STANDALONE_WIN
        Application.Quit();
        string exepath = Path.Combine(Application.dataPath, string.Format("../{0}.exe", Application.productName));
        Process.Start(exepath);
#elif UNITY_ANDROID
        AndroidJavaObject activity =
 new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        string packageName = activity.Call<string>("getPackageName");
        AndroidJavaRunnable androidJavaRunnable = new AndroidJavaRunnable(() =>
        {
            AndroidJavaObject launchIntent =
 activity.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName); ;
            launchIntent.Call<AndroidJavaObject>("addFlags",new AndroidJavaClass("android.content.Intent").GetStatic<int>("FLAG_ACTIVITY_CLEAR_TOP"));
            activity.Call("startActivity", launchIntent);
            using (AndroidJavaClass process = new AndroidJavaClass("android.os.Process"))
            {
                process.CallStatic("killProcess", process.CallStatic<int>("myPid"));
                process.Dispose();
            }
        });
        AndroidJavaObject androidJavaThread = new AndroidJavaObject("java.lang.Thread", androidJavaRunnable);
        androidJavaThread.Call("start");
        activity.Call("finish");
#elif UNITY_IOS

#endif
    }



    /// <summary>
    /// 重启游戏
    /// 完全重启
    /// </summary>
    public static void RestartApplicationFull()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            string packageName = activity.Call<string>("getPackageName");

            AndroidJavaObject restartIntent = activity.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);

            using (AndroidJavaClass PendingIntent = new AndroidJavaClass("android.app.PendingIntent"))
            {
                AndroidJavaObject intent = PendingIntent.CallStatic<AndroidJavaObject>("getActivity", activity, 0, restartIntent, 0);

                string context_ALARM_SERVICE = new AndroidJavaClass("android.content.Context").GetStatic<string>("ALARM_SERVICE");

                int rtc = new AndroidJavaClass("android.app.AlarmManager").GetStatic<int>("RTC");
                long currentTimeMillis = new AndroidJavaClass("java.lang.System").CallStatic<long>("currentTimeMillis");
                AndroidJavaObject manager = activity.Call<AndroidJavaObject>("getSystemService", context_ALARM_SERVICE);
                manager.Call("set", rtc, currentTimeMillis, intent);
                PendingIntent.Dispose();
            }

            activity.Call("finish");
            using (AndroidJavaClass process = new AndroidJavaClass("android.os.Process"))
            {
                process.CallStatic("killProcess", process.CallStatic<int>("myPid"));
                process.Dispose();
            }

            restartIntent.Dispose();
            activity.Dispose();
        }
#elif UNITY_IOS

#endif
    }


    /// <summary>
    /// 检查权限是否获取
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    //<uses-permission android:name="android.permission.WRITE_SETTINGS"></uses-permission>
    public static bool CheckPermission(string name)
    {
        bool result = false;
        if (string.IsNullOrEmpty(name)) return result;
#if UNITY_EDITOR
#elif UNITY_ANDROID
        //AndroidJNIHelper.debug = true;
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            //int permission_granted = new AndroidJavaClass("android.content.pm.PackageManager").GetStatic<int>("PERMISSION_GRANTED");
            //int checkCallingOrSelfPermissionResult = activity.Call<int>("checkCallingOrSelfPermission", name);
            //result = permission_granted == checkCallingOrSelfPermissionResult;
            //Debug.LogFormat("checkCallingOrSelfPermissionResult:{0}", checkCallingOrSelfPermissionResult);

            string packageName = activity.Call<string>("getPackageName");
            AndroidJavaObject packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
            int permission_granted = new AndroidJavaClass("android.content.pm.PackageManager").GetStatic<int>("PERMISSION_GRANTED");
            UnityEngine.Debug.LogFormat("permission_granted:{0} name:{1}  checkPermission:{2}  getPackageName:{3}", permission_granted, name, packageManager.Call<int>("checkPermission", name, packageName), packageName);
            result = permission_granted == packageManager.Call<int>("checkPermission", name, packageName);
        }
#elif UNITY_IOS

#endif
        return result;
    }



    /// <summary>
    /// 获取清单文件的 targetSdkVersion
    /// </summary>
    /// <returns></returns>
    public static int GetTargetSdkVersion()
    {
        int result = -1;
#if UNITY_EDITOR
#elif UNITY_ANDROID
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            result = activity.Call<AndroidJavaObject>("getApplicationInfo").Get<int>("targetSdkVersion");
        }
#elif UNITY_IOS
#endif
        return result;
    }

    /// <summary>
    /// 获取编译sdk版本
    /// </summary>
    /// <returns></returns>
    public static int GetBuildSdkVersion()
    {
        int result = -1;
#if UNITY_EDITOR
#elif UNITY_ANDROID
        using (AndroidJavaClass Build_Version = new AndroidJavaClass("android.os.Build.VERSION"))
        {
            result = Build_Version.GetStatic<int>("SDK_INT");
        }
#elif UNITY_IOS
#endif
        return result;
    }
}
