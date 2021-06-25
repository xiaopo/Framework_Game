using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class GameDebug
{

    public static void Log(string msg)
    {
        Debug.Log(msg);
    }

    public static void LogRed(string msg)
    {
        Debug.Log(string.Format("<color=#ff0000>{0}</color>", msg));
    }

    public static void LogGreen(string msg)
    {
        Debug.Log(string.Format("<color=#00ff00>{0}</color>", msg));
    }


    public static void LogWarning(string msg)
    {
        Debug.LogWarning(msg);
    }

    public static void LogWarningFormat(string format, params object[] args)
    {
        Debug.LogWarningFormat(format, args);
    }


    [System.Diagnostics.Conditional("DEBUG")]
    public static void LogError(string msg)
    {
        Debug.LogError(msg);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        Debug.LogErrorFormat(format, args);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void BeginSample(string name)
    {
        Profiler.BeginSample(name);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void BeginSample(string name, UnityEngine.Object targetObject)
    {
        Profiler.BeginSample(name, targetObject);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void EndSample()
    {
        Profiler.EndSample();
    }

}
