using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Fight
{
    public class FightFixedUpdateUtil
    {
        public class FramCall
        {
            public int id;
            public UnityAction callFun;
        }

        private static List<FramCall> maplist = new List<FramCall>();
        private static int localIndex = 1;
        //获取本地唯一ID
        public static int GetLocalId()
        {
            localIndex++;
            if (localIndex == 0) localIndex++;

            return localIndex;
        }

        public static int AddEvent(UnityAction actio)
        {
            if (actio == null) return 0;

            int gid = GetLocalId();

            FramCall call = new FramCall();
            call.id = gid;
            call.callFun = actio;
            maplist.Add(call);

            return gid;
        }

        public static void Remove(int gid)
        {
            if (gid == 0) return;
            for(int i = 0;i< maplist.Count;i++)
            {
                if(maplist[i].id == gid)
                {
                    maplist.RemoveAt(i);
                }
            }
        }

        public static void Fixedupdate()
        {
            for (int i = 0; i < maplist.Count; i++)
            {
                maplist[i].callFun.Invoke();
            }
        }
    }
}
