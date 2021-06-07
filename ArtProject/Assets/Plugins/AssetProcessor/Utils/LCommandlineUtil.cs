
using System;
using System.Diagnostics;


//调用外部程序

public class LCommandlineUtil
{ 


    public static void StartCat(string fileName,string director, string arguments)
    {
        //director = director.Replace(@"\\", @"\");
        UnityEngine.Debug.Log(director);
        try
        {
            Process psi = new Process();
            psi.StartInfo.FileName = fileName;
            psi.StartInfo.Arguments = arguments;
            psi.StartInfo.WorkingDirectory = director;

            psi.Start();

            psi.WaitForExit();
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("Exception Occurred :{0},{1}：", ex.Message, ex.StackTrace.ToString()));
        }

    }
}
