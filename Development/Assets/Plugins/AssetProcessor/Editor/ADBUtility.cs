using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ADBUtility
{
    static string s_adbPath = Path.Combine(Application.dataPath, "../A_Tools/adb/adb.exe");
    public static void GetDevices(System.Action<string[]> callback)
    {
        System.Threading.ThreadPool.QueueUserWorkItem((object state) =>
        {
            ExecuteCommande cmd = new ExecuteCommande();
            cmd.Execute(s_adbPath, "devices");
            List<string> devices = new List<string>();
            foreach (var item in cmd.result)
            {
                string value = item.Trim();
                if (value.EndsWith("device"))
                    devices.Add(value.Replace("device","").Trim());
            }

            EditorApplication.delayCall += () => 
            {
                callback.Invoke(devices.ToArray());
            };
        });
    }

    public static void PushFile(string deviceName, string srcPath, string devicePath, System.Action<string> callback)
    {
        System.Threading.ThreadPool.QueueUserWorkItem((object state) =>
        {
            ExecuteCommande cmd = new ExecuteCommande();
            string args = string.Format("-s {0} push {1} {2}", deviceName, srcPath, devicePath);
            cmd.Execute(s_adbPath, args);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var item in cmd.result)
                sb.AppendLine(item);
            EditorApplication.delayCall += () =>
            {
                callback.Invoke(sb.ToString());
            };
        });
    }

    //adb -s  9df7c6ea shell rm /sdcard/Android/data/com.DownLoad.test/files/devscript.lua
    public static void RemoveFile(string deviceName, string devicePath, System.Action<string> callback)
    {
        System.Threading.ThreadPool.QueueUserWorkItem((object state) =>
        {
            ExecuteCommande cmd = new ExecuteCommande();
            string args = string.Format("-s {0} shell rm {1}", deviceName, devicePath);
            cmd.Execute(s_adbPath, args);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var item in cmd.result)
                sb.AppendLine(item);
            EditorApplication.delayCall += () =>
            {
                callback.Invoke(sb.ToString());
            };
        });
    }


    public class ExecuteCommande
    {
        private List<string> m_Result = new List<string>();
        public List<string> result { get { return m_Result; } }
        public void Execute(string exePath, string args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = exePath;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data) && !string.IsNullOrEmpty(e.Data.Trim()))
                {
                    m_Result.Add(e.Data);
                }
            });

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            //process.WaitForExit();
            process.Close();
        }
    }
}