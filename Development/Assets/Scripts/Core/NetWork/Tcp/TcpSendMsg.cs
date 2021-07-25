using System;
namespace NetWork
{
    class TcpSendMsg
    {
        public byte[] data;
        public int length;

        private TcpSendMsg()
        {
        }

        private static TcpSafeQueue<TcpSendMsg> msgQueue = new TcpSafeQueue<TcpSendMsg>(256);

        public static TcpSendMsg Get()
        {
            TcpSendMsg tcpMsg = msgQueue.Dequeue();
            if (tcpMsg == null)
            {
                return new TcpSendMsg();
            }
            else
            {
                return tcpMsg;
            }
        }

        public static void Recycle(TcpSendMsg tcpMsg)
        {
            if (tcpMsg == null)
            {
                return;
            }
            TcpCacheBuffer.Recycle(tcpMsg.data);
            tcpMsg.data = null;
            tcpMsg.length = 0;
            msgQueue.Enqueue(tcpMsg);
        }
    }

}