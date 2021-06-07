using System;

using System.Linq;
using System.Text;

namespace LZ4Sharp
{
    public class LZ4Util
    {
        public static string CompressString(string sourceStr)
        {
            var compressed = Convert.ToBase64String(LZ4Sharp.LZ4.Compress(Encoding.UTF8.GetBytes(sourceStr)));

            return compressed;
        }

        public static string DecompressString(string compressed)
        {

            var lorems = Encoding.UTF8.GetString(LZ4Sharp.LZ4.Decompress(Convert.FromBase64String(compressed)));

            return lorems;

        }
    }

       
}
