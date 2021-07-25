using System;
namespace NetWork
{
    public class TcpCacheBuffer
    {
        private static TcpSafeQueue<byte[]> cache32 = new TcpSafeQueue<byte[]>(1024);
        private static TcpSafeQueue<byte[]> cache64 = new TcpSafeQueue<byte[]>(1024);
        private static TcpSafeQueue<byte[]> cache128 = new TcpSafeQueue<byte[]>(1024);
        private static TcpSafeQueue<byte[]> cache256 = new TcpSafeQueue<byte[]>(512);
        private static TcpSafeQueue<byte[]> cache512 = new TcpSafeQueue<byte[]>(512);
        private static TcpSafeQueue<byte[]> cache1024 = new TcpSafeQueue<byte[]>(128);
        private static TcpSafeQueue<byte[]> cache2048 = new TcpSafeQueue<byte[]>(64);
        private static TcpSafeQueue<byte[]> cache4196 = new TcpSafeQueue<byte[]>(64);
        private static TcpSafeQueue<byte[]> cache8192 = new TcpSafeQueue<byte[]>(64);

        private static void GetCacheQueue(int size, out int cacheSzie, out TcpSafeQueue<byte[]> queue)
        {
            if (size <= 32)
            {
                cacheSzie = 32;
                queue = cache32;
            }
            else if (size <= 64)
            {
                cacheSzie = 64;
                queue = cache64;
            }
            else if (size <= 128)
            {
                cacheSzie = 128;
                queue = cache128;
            }
            else if (size <= 256)
            {
                cacheSzie = 256;
                queue = cache256;
            }
            else if (size <= 512)
            {
                cacheSzie = 512;
                queue = cache512;
            }
            else if (size <= 1024)
            {
                cacheSzie = 1024;
                queue = cache1024;
            }
            else if (size <= 2048)
            {
                cacheSzie = 2048;
                queue = cache2048;
            }
            else if (size <= 4196)
            {
                cacheSzie = 4196;
                queue = cache4196;
            }
            else if (size <= 8192)
            {
                cacheSzie = 8192;
                queue = cache8192;
            }
            else
            {
                cacheSzie = size;
                queue = null;
            }
        }

        public static byte[] Get(int size)
        {
            if (size <= 0)
                return null;

            int cacheSzie;
            TcpSafeQueue<byte[]> queue;
            GetCacheQueue(size, out cacheSzie, out queue);
            if (queue == null)
            {
                return new byte[cacheSzie];
            }
            byte[] bytes = queue.Dequeue();
            if (bytes == null)
            {
                return new byte[cacheSzie];
            }
            return bytes;
        }

        public static void Recycle(byte[] bytes)
        {
            if (bytes == null)
                return;

            int cacheSzie;
            TcpSafeQueue<byte[]> queue;
            GetCacheQueue(bytes.Length, out cacheSzie, out queue);
            if (queue != null && cacheSzie == bytes.Length)
            {
                queue.Enqueue(bytes);
            }
        }
    }
}