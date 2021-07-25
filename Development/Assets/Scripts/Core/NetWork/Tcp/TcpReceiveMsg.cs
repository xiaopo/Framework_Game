using System;
namespace NetWork
{
    public class TcpReceiveMsg
    {
        public TcpEvent tcpEvent;
        public byte[] data;
        public int length;

        private TcpReceiveMsg()
        {
        }

        private static TcpSafeQueue<TcpReceiveMsg> msgQueue = new TcpSafeQueue<TcpReceiveMsg>(2048);

        public static TcpReceiveMsg Get()
        {
            TcpReceiveMsg tcpMsg = msgQueue.Dequeue();
            if (tcpMsg == null)
            {
                return new TcpReceiveMsg();
            }
            else
            {
                return tcpMsg;
            }
        }

        public static void Recycle(TcpReceiveMsg tcpMsg)
        {
            if (tcpMsg == null)
            {
                return;
            }
            TcpCacheBuffer.Recycle(tcpMsg.data);
            tcpMsg.tcpEvent = 0;
            tcpMsg.data = null;
            tcpMsg.length = 0;
            msgQueue.Enqueue(tcpMsg);
        }
    }
}