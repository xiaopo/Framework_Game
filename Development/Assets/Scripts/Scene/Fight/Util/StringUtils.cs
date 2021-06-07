using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Fight
{
    public class StringUtils
    {

        /// <summary>
        /// 判断输入是否数字
        /// </summary>
        /// <param name="num">要判断的字符串</param>
        /// <returns></returns>
        static public bool VldInt(string num)
        {
            #region
            try
            {
                Convert.ToInt32(num);
                return true;
            }
            catch { return false; }
            #endregion
        }

        /// <summary>
        /// 判断输入是否中文
        /// </summary>
        /// <param name="CString">要判断的字符串</param>
        /// <returns></returns>
        static public bool IsChina(string CString)
        {
            #region
            bool BoolValue = false;
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                {
                    BoolValue = false;
                }
                else
                {
                    BoolValue = true;
                }
            }
            return BoolValue;
            #endregion
        }

        /// <summary>
        /// 过滤字符
        /// </summary>
        /// <param name="num">要过滤的字符串</param>
        /// <returns></returns>
        static public string VldString(string text)
        {
            #region
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = Regex.Replace(text, "[\\s]{2,}", " "); //two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //any other tags
            text = text.Replace("'", "''");
            return text;
            #endregion
        }

        /// <summary>
        /// 返回文本编辑器替换后的字符串
        /// </summary>
        /// <param name="str">要替换的字符串</param>
        /// <returns></returns>
        static public string GetHtmlEditReplace(string str)
        {
            #region
            return str.Replace("'", "''").Replace("&nbsp;", " ").Replace(",", "，").Replace("%", "％").Replace("script", "").Replace(".js", "");
            #endregion
        }

        /// <summary>
        /// 截取字符串函数
        /// </summary>
        /// <param name="str">所要截取的字符串</param>
        /// <param name="num">截取字符串的长度</param>
        /// <param name="flg">true:加...,flase:不加</param>
        /// <returns></returns>
        public static string GetSubString(string str, int num, bool flg)
        {
            #region
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += str.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > num)
                    break;
            }
            //如果截过则加上半个省略号
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(str);
            if (mybyte.Length > num)
                if (flg)
                {
                    tempString += "...";
                }
            return tempString;
            #endregion
        }

        /// <summary>
        /// 过滤输入信息
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>
        public static string InputText(string text, int maxLength)
        {
            #region
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " "); //two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //any other tags
            text = text.Replace("'", "''");
            return text;
            #endregion
        }

       
        /// <summary>
        /// 半角转全角
        /// </summary>
        /// <param name="BJstr"></param>
        /// <returns></returns>
        static public string GetQuanJiao(string BJstr)
        {
            #region
            char[] c = BJstr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 0)
                    {
                        b[0] = (byte)(b[0] - 32);
                        b[1] = 255;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }

            string strNew = new string(c);
            return strNew;

            #endregion
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="QJstr"></param>
        /// <returns></returns>
        static public string GetBanJiao(string QJstr)
        {
            #region
            char[] c = QJstr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }

            string strNew = new string(c);
            return strNew;
            #endregion
        }

        /// <summary>
        /// 将  [1,2,3,4,5][2,3,4,5,6] 转换为 List<string []>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static public List<string[]> AnalysisToArray(string source)
        {
            List<string[]> list = new List<string[]>();
            if (source == null || source == "") return list;
            int startIndex = 0;
            int endIndex = 0;
            while (true)
            {
                startIndex = source.IndexOf("[", startIndex);
                endIndex = source.IndexOf("]", endIndex);
                if (startIndex == -1 || endIndex == -1) break;


                if (startIndex != -1 && endIndex != -1)
                {
                    string sub = source.Substring(startIndex + 1, endIndex - startIndex - 1);
                    string[] value = sub.Split(',');
                    list.Add(value);

                }
                endIndex++;
                startIndex++;

            }


            return list;

        }

        /// <summary>
        ///  超过10万用万，超过1亿用亿
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static public string FormattingNumber(long number)
        {
            string str = "";

            if(number >= 100000 && number < 100000000)
            {
                str = (number / 10000).ToString() + "万";
            }
            else if(number >= 100000000)
            {
                str = (number / 100000000).ToString() + "亿";
            }
            else
            {
                str = number.ToString();
            }

            return str;
        }

        /// <summary>
        /// 是整数的话，不显示小数
        /// 不是整数的话，保留小数点后1位，四舍五入
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static public string FormattingFloat(float number)
        {
            string resStr = "";
            string str = Convert.ToString(number);
            int dot = str.IndexOf(".");
            bool hasnotzerochar;
            if (dot != -1)
            {
                string substr = str.Substring(dot + 1);
                hasnotzerochar = false;//记录是否小数点后存在不为0的字符
                for (int i = 0; i < substr.Length; i++)
                {
                    if (!substr[i].Equals("0"))
                    {
                        hasnotzerochar = true;
                    }
                }
            }
            else
            {
                hasnotzerochar = false;
            }

            //是小数
            if(hasnotzerochar)
            {
                //resStr = Math.Round(number, 1).ToString();
                resStr = String.Format("{0:N1}", number);
            }
            //不是小数
            else
            {
                resStr = number.ToString();
            }

            return resStr;
        }

        /// <summary>
        /// 分析短富文本参数 <lab txt=\"hello world!!\" color=#ffff00 data=数据 />[face,5][item,10000]<lab txt=\"hello!!\" color=#ffff00/>
        /// </summary>
        /// <returns>["normal","<lab txt=\"hello world!!\" color=#ffff00 data=数据 />"],["param","face,5"]</returns>
        /// <param name="rtf">Rtf.</param>
        public static List<string[]> AnalysisRtf(string rtf)
        {
            List<string[]> list = new List<string[]>();
            try
            {
                char[] chars = rtf.ToCharArray();
                StringBuilder sb = new StringBuilder();
                
                for (int i = 0; i < chars.Length; i++)
                {
                    char c = chars[i];
                    if (c == '[')
                    {

                        int endIdx = rtf.IndexOf(']', i);
                        if (endIdx != -1)
                        {

                            list.Add(new string[] { "normal", sb.ToString() });
                            list.Add(new string[] { "param", rtf.Substring(i + 1, endIdx - i - 1) });
                            sb.Remove(0, sb.Length);
                            i = endIdx;

                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                    else if (c == '{')
                    {

                        int endIdx = rtf.IndexOf('}', i);
                        if (endIdx != -1)
                        {

                            list.Add(new string[] { "normal", sb.ToString() });
                            list.Add(new string[] { "item", rtf.Substring(i + 1, endIdx - i - 1) });
                            sb.Remove(0, sb.Length);
                            i = endIdx;

                        }
                        else
                        {
                            sb.Append(c);
                            //Debug.Log(c);
                        }
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }

                list.Add(new string[] { "normal", sb.ToString() });
                for (int i = 0; i < list.Count; i++)
                {
                    ///判断是否是空字符
                    if (string.IsNullOrEmpty(list[i][1]))
                    {
                        list.RemoveAt(i);
                        //i++;
                    }
                    //if (list[i][1] == "")
                    //{
                    //    list.RemoveAt(i);
                    //    i++;
                    //}
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("富文本格式有问题:" + e.Message);
            }
            return list;
        }
    }
}
