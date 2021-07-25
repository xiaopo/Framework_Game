using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
namespace NetWork
{
    public class NetworkHelper
    {
        private enum NetworkType
        {
            Unknown,
            Ipv4,
            Ipv6
        }
        private NetworkType networkType;

        private static Regex pattern = new Regex("([012]?[0-9]{1,2}\\.){3}([012]?[0-9]{1,2})", RegexOptions.Compiled);


        public static bool IsIPFormat(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return false;
            }
            Match m = pattern.Match(host);
            return m != null && m.Success;
        }

        public static string ConvertIPV4To6(string ipv4)
        {
            if (string.IsNullOrEmpty(ipv4) || ipv4.Contains(":"))
            {
                return ipv4;
            }
            return string.Format("64:ff9b::{0}", ipv4);
        }

        public static string ConvertUrl4To6(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            Match m = pattern.Match(url);
            if (m.Success)
            {
                return url.Replace(m.Value, string.Format("[{0}]", ConvertIPV4To6(m.Value)));
            }
            else
            {
                return url;
            }
        }

        public static bool IsIPV4()
        {
            //#if UNITY_IOS && !UNITY_EDITOR
            //        if(networkType == NetworkType.Ipv4)
            //        {
            //            return true;
            //        }
            //        else if(networkType == NetworkType.Ipv6)
            //        {
            //            return false;
            //        }
            //        NetworkInfo info = NativeApi.GetNetworkInfo();
            //        if(info != null && !string.IsNullOrEmpty(info.deviceIp))
            //        {
            //            if(!info.deviceIp.Contains(":"))
            //            {
            //                networkType = NetworkType.Ipv4;
            //                return true;
            //            }
            //            else
            //            {
            //                networkType = NetworkType.Ipv6;
            //                return false;
            //            }
            //        }
            //#endif
            return true;
        }

        public static Uri GetUri(string uri)
        {
            if (NetworkHelper.IsIPV4())
            {
                return new Uri(uri);
            }
            else
            {
                return new Uri(ConvertUrl4To6(uri));
            }
        }

        public static string GetLocalIP()
        {
            try
            {
                IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress item in IpEntry.AddressList)
                {
                    //AddressFamily.InterNetwork  ipv4
                    //AddressFamily.InterNetworkV6 ipv6
                    if (item.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return item.ToString();
                    }
                }
                return "";
            }
            catch { return ""; }
        }

    }
}