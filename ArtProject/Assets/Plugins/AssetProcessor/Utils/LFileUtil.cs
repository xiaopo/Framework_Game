using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;


public class LFileUtil 
{
    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string md5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    public static string md5file(byte[] bt)
    {
        try
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(bt);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    public static string fileMD5(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    public static int fileSize(string file)
    {
        try
        {
            return (int)new FileInfo(file).Length;
        }
        catch (Exception ex)
        {

            throw new Exception("fileSize() fail, error:" + ex.Message);
        }
    }



    public static DateTime ConvertStringToDateTime(string timeStamp)
    {
        DateTime dtStart = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    public static string ConvertStringToDateStr(string timeStamp)
    {
        if (string.IsNullOrEmpty(timeStamp)) return string.Empty;

        DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
        dtFormat.ShortDatePattern = "yyyyMMddHHmmss";
        return System.Convert.ToDateTime(timeStamp, dtFormat).ToString("yyyy/MM/dd HH:mm:ss");
    }

    public static void WriteFile(string savePath, string datastr)
    {
        FileStream file = File.Open(savePath, FileMode.OpenOrCreate);
        StreamWriter streamWriter = new StreamWriter(file);//
        streamWriter.Write(datastr);
        streamWriter.Flush();
        streamWriter.Close();
    }

    public static void DirectoryCopy(string sourceDirName, string destDirName)
    {
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        foreach (string folderPath in Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories))
        {
            if (!Directory.Exists(folderPath.Replace(sourceDirName, destDirName)))
                Directory.CreateDirectory(folderPath.Replace(sourceDirName, destDirName));
        }

        foreach (string filePath in Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories))
        {
            var fileDirName = Path.GetDirectoryName(filePath).Replace("\\", "/");
            var fileName = Path.GetFileName(filePath);
            string newFilePath = Path.Combine(fileDirName.Replace(sourceDirName, destDirName), fileName);

            File.Copy(filePath, newFilePath, true);
        }
    }

}
