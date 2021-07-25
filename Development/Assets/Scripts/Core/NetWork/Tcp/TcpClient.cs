using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using XLua;
using System.Runtime.InteropServices;
using System.Collections;
namespace NetWork
{
    public enum TcpEvent
    {
        ConnnectSuccess = 1,
        ConnnectFail = 2,
        SendSuccess = 4,
        ReceiveSuccess = 5,
        Exception = 6,
        Disconnect = 7,
        SendFail = 8,
        ReceiveFail = 9,
        ReceiveMessage = 255,
    }


    public class TcpClient
    {
        public string serverIp
        {
            get;
            private set;
        }

        public int serverPort
        {
            get;
            private set;
        }

        public bool isConnected
        {
            get
            {
                return socket != null && socket.Connected;
            }
        }

        private TcpParser tcpParser;
        private byte[] receiveBuffer;
        private const int BUFFER_SIZE = 8192;


        public int frameProcessCount = 20;
        private Socket socket;
        private bool isClosed;

        private TcpSafeQueue<TcpReceiveMsg> recvQueue;
        private LuaFunction luaCallback;

        public TcpClient()
        {
            tcpParser = new TcpParser();
            receiveBuffer = new byte[BUFFER_SIZE];
            NetworkManager.Instance.AddTcpClient(this);
            recvQueue = new TcpSafeQueue<TcpReceiveMsg>(2048);
        }

        public static TcpClient CreateTcpClient()
        {
            TcpClient obj = new TcpClient();
            return obj;
        }

        //字节截断函数，有多少长度多少字节，不然客户端decode有错
        public byte[] SubByte(byte[] srcBytes, int startIndex, int length)
        {
            System.IO.MemoryStream bufferStream = new System.IO.MemoryStream();
            byte[] returnByte = new byte[] { };
            if (srcBytes == null) { return returnByte; }
            if (startIndex < 0) { startIndex = 0; }
            if (startIndex < srcBytes.Length)
            {
                if (length < 1 || length > srcBytes.Length - startIndex) { length = srcBytes.Length - startIndex; }
                bufferStream.Write(srcBytes, startIndex, length);
                returnByte = bufferStream.ToArray();
                bufferStream.SetLength(0);
                bufferStream.Position = 0;
            }
            bufferStream.Close();
            bufferStream.Dispose();
            return returnByte;
        }

        public void Update()
        {
            //判断套接字和回调
            if (isClosed || luaCallback == null)
            {
                return;
            }

            TcpReceiveMsg revMsg = null;
            while (true)
            {
                //取出一条消息处理
                revMsg = recvQueue.Dequeue();
                if (revMsg == null)
                {
                    break;
                }

                try
                {
                    Debug.Log("!!!!!!!!!!!!!!!!!!!!cs.Update!!!!!!!!!!!!!!!!!!!! " + revMsg.tcpEvent + " " + revMsg.data + " " + revMsg.length + " " + luaCallback);

                    //这里enum要转下int
                    if (revMsg.data != null)
                    {
                        byte[] finalbuffer = SubByte(revMsg.data, 0, revMsg.length);

                        luaCallback.Call(Convert.ToInt32(revMsg.tcpEvent), finalbuffer);
                    }
                    else
                    {
                        luaCallback.Call(Convert.ToInt32(revMsg.tcpEvent));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
                TcpReceiveMsg.Recycle(revMsg);
            }
        }

        public void SetCallback(LuaFunction func)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!cs.SetCallback!!!!!!!!!!!!!!!!!!!!" + func);
            if (luaCallback != null)
            {
                luaCallback.Dispose();
            }

            luaCallback = func;
        }

        private void AddReceiveMsg(TcpEvent tcpEvent, byte[] data, int length)
        {
            TcpReceiveMsg revMsg = TcpReceiveMsg.Get();
            revMsg.tcpEvent = tcpEvent;
            revMsg.data = data;
            revMsg.length = length;
            recvQueue.Enqueue(revMsg);
        }


        public string Connect(string ip, int port)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!cs.Connect!!!!!!!!!!!!!!!!!!!!" + ip + " " + port);

            if (socket != null)
            {
                Close();
            }

            try
            {
                serverIp = ip;
                serverPort = port;

                IPAddress[] address = Dns.GetHostAddresses(ip);
                if (address == null)
                {
                    Debug.Log("获取GetHostAddresses失败");
                }

                if (address != null && address.Length > 0 && address[0].AddressFamily == AddressFamily.InterNetworkV6)
                {
                    socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                    socket.BeginConnect(address, port, OnConnect, null);
                }
                else
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.BeginConnect(address, port, OnConnect, null);
                }
            }
            catch (Exception ex)
            {
                PrintException(ex, TcpEvent.ConnnectFail);
                return ex.Message;
            }
            return null;
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                if (socket == null)
                {
                    throw new Exception("TCP OnConnect Error! socket不存在");
                }
                socket.EndConnect(ar);
                AddReceiveMsg(TcpEvent.ConnnectSuccess, null, 0);
            }
            catch (Exception ex)
            {
                PrintException(ex, TcpEvent.ConnnectFail);
            }
            Receive();
        }

        private void Receive()
        {
            try
            {
                if (!isConnected)
                {
                    throw new Exception("TCP Receive Error! 未与服务器建立连接");
                }
                socket.BeginReceive(receiveBuffer, 0, BUFFER_SIZE, SocketFlags.None, OnReceive, null);
            }
            catch (Exception ex)
            {
                PrintException(ex, TcpEvent.Exception);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            if (isClosed)
            {
                return;
            }
            try
            {
                if (!isConnected)
                {
                    throw new Exception("TCP OnReceive Error! 当前未连接到服务器");
                }
                int length = socket.EndReceive(ar);
                if (length > 0)
                {
                    ParseReceive(length);
                }

                Array.Clear(receiveBuffer, 0, BUFFER_SIZE);   //清空数组
                Receive();
            }
            catch (Exception ex)
            {
                PrintException(ex, TcpEvent.Exception);
            }
        }

        private void ParseReceive(int length)
        {
            tcpParser.Receive(receiveBuffer, length);
            byte[] data;
            while (true)
            {
                int dataLength = tcpParser.Unpack(out data);
                if (dataLength == 0)
                {
                    break;
                }
                AddReceiveMsg(TcpEvent.ReceiveMessage, data, dataLength);
            }
        }


        public static void PrintByteData(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("X2"));
            }
            UnityEngine.Debug.Log(sb.ToString());
        }

        public void Send(byte[] msg)
        {
            try
            {
                int len = msg.Length;
                if (!isConnected)
                {
                    throw new Exception("TCP Send Error! 没有连接到服务器");
                }
                byte[] data;
                int length = tcpParser.Pack(msg, len, out data);
                if (data == null || length <= 0 || data.Length < length)
                {
                    throw new Exception(string.Format("参数错误，无法生成数据包: data {0} length {1}", (data == null) ? -1 : data.Length, length));
                }
                TcpSendMsg sendMsg = TcpSendMsg.Get();
                sendMsg.data = data;
                sendMsg.length = length;
                socket.BeginSend(data, 0, length, SocketFlags.None, OnSend, sendMsg);
            }
            catch (Exception ex)
            {
                PrintException(ex, TcpEvent.SendFail);
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                if (ar != null)
                {
                    TcpSendMsg sendMsg = (TcpSendMsg)ar.AsyncState;
                    TcpSendMsg.Recycle(sendMsg);
                }
                if (!isConnected)
                {
                    throw new Exception("TCP OnSend Error! 没有连接到服务器");
                }
                socket.EndSend(ar);
            }
            catch (Exception ex)
            {
                PrintException(ex, TcpEvent.SendFail);
            }
        }

        public void Release()
        {
            isClosed = true;
            NetworkManager.Instance.RemoveTcpClient(this);
        }

        public void Close()
        {
            if (socket != null)
            {
                try
                {
                    if (socket.Connected)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    socket.Close();
                }
                catch (Exception ex)
                {
                    PrintException(ex, TcpEvent.Exception);
                }
                socket = null;
                //GameDebug.Log(string.Format("TCP Close ip={0} port={1}", serverIp, serverPort));
            }
            if (tcpParser != null)
            {
                tcpParser.Release();
                tcpParser = null;
            }

            if (luaCallback != null)
            {
                luaCallback.Dispose();
                luaCallback = null;
            }
        }

        private void PrintException(Exception ex, TcpEvent eventType, string msg = "")
        {
            if (ex == null)
            {
                return;
            }
            string str = null;
            if (string.IsNullOrEmpty(ex.Message))
            {
                if (string.IsNullOrEmpty(msg))
                {
                    str = ex.ToString();
                }
                else
                {
                    str = string.Format("{0} {1}", msg, ex.ToString());
                }
            }
            else
            {
                if (string.IsNullOrEmpty(msg))
                {
                    str = ex.Message;
                }
                else
                {
                    str = string.Format("{0} {1}", msg, ex.Message);
                }
            }
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            AddReceiveMsg(eventType, data, data.Length);
        }

    }
}