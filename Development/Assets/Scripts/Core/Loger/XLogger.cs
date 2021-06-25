using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Threading;

public class XLogger
{
    public static int s_MainThreadId;
    private static int s_FrameCount;

    public enum DebugLevel : int
    {
        DEBUG = 0,
        INFO,
        WARNING,
        ERROR,

        NOLOG,  // 放在最后面，使用这个时表示不输出任何日志
    }

    public static DebugLevel s_DebugLevel = DebugLevel.DEBUG;
    private static StringBuilder s_StringBuilder = new StringBuilder();
    public static string GetLoggerHead(string tag)
    {
        s_StringBuilder.Length = 0;

        if (s_MainThreadId == Thread.CurrentThread.ManagedThreadId)
            s_FrameCount = Time.frameCount;

        s_StringBuilder.Append("[");
        s_StringBuilder.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff 〈"));
        s_StringBuilder.Append(s_FrameCount);
        s_StringBuilder.Append("〉] ");
        s_StringBuilder.Append(tag);
        return s_StringBuilder.ToString();
    }

    public static void INFO(object s)
    {
        if (DebugLevel.INFO >= s_DebugLevel)
            Debug.Log(GetLoggerHead("INFO ") + s);
    }

    public static void INFO_Format(string format, params object[] s)
    {
        if (DebugLevel.INFO >= s_DebugLevel)
            INFO(string.Format(format, s));
    }

    public static void DEBUG(object s)
    {
        if (DebugLevel.DEBUG >= s_DebugLevel)
            Debug.Log(GetLoggerHead("DEBUG ") + s);
    }

    public static void DEBUG_Format(string format, params object[] s)
    {
        if (DebugLevel.DEBUG >= s_DebugLevel)
            DEBUG(string.Format(format, s));
    }

    public static void WARNING(object s)
    {
        if (DebugLevel.WARNING >= s_DebugLevel)
            Debug.LogWarning(GetLoggerHead("WARNING ") + s);
    }

    public static void WARNING_Format(string format, params object[] s)
    {
        if (DebugLevel.WARNING >= s_DebugLevel)
            WARNING(string.Format(format, s));
    }

    public static void ERROR(object s)
    {
        if (DebugLevel.ERROR >= s_DebugLevel)
            Debug.LogError(GetLoggerHead("ERROR ") + s);
    }

    public static void ERROR_Format(string format, params object[] s)
    {
        if (DebugLevel.ERROR >= s_DebugLevel)
            ERROR(string.Format(format, s));
    }


    //将日志记录到文件
    public static void RecordLog(string filePath, string content, string tag = "LOG ")
    {
        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (string.IsNullOrEmpty(content))
            File.AppendAllText(filePath, "\n");
        else
            File.AppendAllText(filePath, GetLoggerHead(tag) + content + "\n");
    }


    //日志上报
    public static void ReportException(string name, string message, string stackTrace)
    {
        INFO_Format("异常上报：ReportException name:{0} message:{1} stackTrace:{2}", name, message, stackTrace);
        //BuglyAgent.ReportException(name, message, stackTrace);
    }
}
