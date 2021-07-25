using System;
using System.IO;

namespace NetWork
{
    public class TcpParser
    {
        static UInt64 XOR_KEY = 0xe07aea3911363aa9;
        public static bool isEncrypt = true;
        static byte[] keyMap;
        public static void SetXorKey(string str)
        {
            isEncrypt = (str != "");
            if (isEncrypt)
            {
                XOR_KEY = Convert.ToUInt64(str, 16);
                InitKeyMap();
            }
        }
        static byte ModXorKey(int k)
        {
            return (byte)(XOR_KEY >> (8 * k) & 0xff);
        }
        public static void InitKeyMap()
        {
            if (isEncrypt)
            {
                keyMap = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    keyMap[i] = ModXorKey(i);
                }
            }
        }
        private MemoryStream bufferStream;

        public TcpParser()
        {
            bufferStream = new MemoryStream(65536);
        }

        public void Receive(byte[] data, int length)
        {
            if (bufferStream.Capacity - bufferStream.Length < length)
            {
                long remainCount = bufferStream.Length - bufferStream.Position;
                Array.Copy(bufferStream.GetBuffer(), bufferStream.Position, bufferStream.GetBuffer(), 0, remainCount);
                bufferStream.Position = 0;
                bufferStream.SetLength(remainCount);
            }

            long position = bufferStream.Position;
            bufferStream.Seek(0, SeekOrigin.End);
            bufferStream.Write(data, 0, length);
            bufferStream.Position = position;
        }

        public int Unpack(out byte[] data)
        {
            long remainCount = bufferStream.Length - bufferStream.Position;
            if (remainCount > 2)
            {
                int len = ReadLength(bufferStream.GetBuffer(), (int)bufferStream.Position);
                if (remainCount >= len + 2)
                {
                    data = TcpCacheBuffer.Get(len);
                    Buffer.BlockCopy(bufferStream.GetBuffer(), (int)bufferStream.Position + 2, data, 0, len);
                    bufferStream.Position += len + 2;
                    if (isEncrypt)
                    {
                        Decrypt(ref data, 0, len);
                    }
                    return len;
                }
            }
            data = null;
            return 0;
        }

        public int Pack(byte[] msg, int len, out byte[] data)
        {
            data = TcpCacheBuffer.Get(len + 2);
            WriteLength(data, 0, len);

            WriteData(data, 2, len, msg);

            if (isEncrypt)
            {
                Encrypt(ref data, 2, len + 2);
            }
            return len + 2;
        }

        private int ReadLength(byte[] data, int index)
        {
            int length = (int)data[index] * 256 + data[index + 1];
            return length;
        }

        private void WriteLength(byte[] data, int index, int length)
        {
            data[index] = (byte)(length / 256);
            data[index + 1] = (byte)(length % 256);
        }

        private void WriteData(byte[] data, int index, int length, byte[] sData)
        {
            for (int i = 0; i < sData.Length && i < length; i++)
            {
                data[index + i] = sData[i];
            }
        }

        private void XorData(ref byte[] data, int index, int length)
        {
            int size = length - index;
            int mark = size % 10 + 1;
            int sp_mark = (size % 2 != 0) ? (size % 20) : (size / 7);
            for (int i = index; i < length; i++)
            {
                int xorIdx = i - index;
                if (xorIdx % mark == 0)
                    continue;
                if (xorIdx == sp_mark)
                    data[i] ^= 0xa3;
                else
                    data[i] ^= keyMap[xorIdx % 8];
            }
        }

        private void Encrypt(ref byte[] data, int index, int length)
        {
            XorData(ref data, index, length);
        }

        public void Decrypt(ref byte[] data, int index, int length)
        {
            XorData(ref data, index, length);
        }

        public void Release()
        {
            if (bufferStream != null)
            {
                bufferStream.Close();
                bufferStream = null;
            }
        }
    }
}