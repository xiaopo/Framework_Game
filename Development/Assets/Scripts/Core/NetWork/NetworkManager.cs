using System.Collections.Generic;

namespace NetWork
{

    public class NetworkManager : SingleBehaviourTemplate<NetworkManager>
    {
        private List<TcpClient> tcpList;
        private List<TcpClient> delList;
        private List<TcpClient> addList;

        protected override void OnInitialize()
        {
            tcpList = new List<TcpClient>();
            delList = new List<TcpClient>();
            addList = new List<TcpClient>();
        }

        public void OnStart()
        {
            //TCP ≥ı ºªØ
            TcpParser.InitKeyMap();
        }

        protected void Update()
        {
            for (int i = 0; i < addList.Count; i++)
            {
                tcpList.Add(addList[i]);
            }
            addList.Clear();

            for (int i = 0; i < delList.Count; i++)
            {
                tcpList.Remove(delList[i]);
                delList[i].Close();
            }
            delList.Clear();

            for (int i = 0; i < tcpList.Count; i++)
            {
                if (tcpList[i] != null)
                {
                    tcpList[i].Update();
                }
            }
        }

        public void AddTcpClient(TcpClient socket)
        {
            if (!addList.Contains(socket))
            {
                addList.Add(socket);
            }
        }

        public void RemoveTcpClient(TcpClient socket)
        {
            if (!delList.Contains(socket))
            {
                delList.Add(socket);
            }
        }

        public void OnDestroy()
        {
            for (int i = 0; i < addList.Count; i++)
            {
                addList[i].Close();
            }
            addList.Clear();

            for (int i = 0; i < delList.Count; i++)
            {
                delList[i].Close();
            }
            delList.Clear();

            for (int i = 0; i < tcpList.Count; i++)
            {
                tcpList[i].Close();
            }
            tcpList.Clear();

            StopAllCoroutines();
        }
    }
}