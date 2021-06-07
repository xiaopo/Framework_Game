
using System.IO;
using System.Text;
using UnityEngine;

namespace Fight
{
    public class FightDebug
    {

        public static void pritnLog(string str)
        {
//            if(MapFtingPro.instance.fightProgram.writeOutBroascat)
//            {
//#if UNITY_EDITOR
//                string path = "E:\\Broascat.txt";
//#else
//               string path = Application.persistentDataPath   + "/Broascat.txt";
//#endif
      
//                string line = "";
//                if (File.Exists(path))
//                {
//                    StreamReader sr = new StreamReader(path, Encoding.Default);
//                    line = sr.ReadToEnd();
//                    sr.Close();
//                    line += "\n" + str;

//                    str = line;
//                }

//                FileStream fs = new FileStream(path, FileMode.Create);
//                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
//                //开始写入
//                sw.Write(str);
//                //清空缓冲区
//                sw.Flush();
//                //关闭流
//                sw.Close();
//                fs.Close();
//            }
        }

        public static void LogWarning(string str)
        {
            Debug.LogWarning(str);
        }

    
        public static void ErrorLog(string str)
        {

            Debug.LogError("【战斗】 " + str);

        }

        public static void ErrorConfig(string str)
        {
            Debug.LogError("【战斗】 配置错误" + str);
        }


    }

}
