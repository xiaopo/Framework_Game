using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Fight
{
    public class FightDelayCall
    {
        public class FtDelyCall
        {
            public int id;
            public float callTime;
            public float intervalTime;//间隔时间
            public int loopNum;// 0 无线循环
            public bool loop;//无限循环
            public UnityAction callFun;
        }

        // private static List<FtDelyCall> maplist = new List<FtDelyCall>();
        private static DictionaryExt<int, FtDelyCall> maplist = new DictionaryExt<int, FtDelyCall>();
        private static int localIndex = 1;
        //获取本地唯一ID
        public static int GetLocalId()
        {
            localIndex++;
            if (localIndex == 0) localIndex++;

            return localIndex;
        }

        public static int DelayCall(UnityAction actio,float inteval,int loopNum = 1)
        {
            if (actio == null) return 0;

            int gid = GetLocalId();
            if (inteval <= 0) inteval = 0.1f;
            FtDelyCall call = new FtDelyCall();
            call.id = gid;
            call.callFun = actio;
            call.callTime = Time.time + inteval;
            call.intervalTime = inteval;
            call.loopNum = loopNum;
            call.loop = loopNum <= 0;
            maplist.Add(gid,call);

            return gid;
        }

        public static void Delete(int gid)
        {
            if (gid == 0) return;
            FtDelyCall call = null;
            if(maplist.TryGetValue(gid,out call) )
            {
                maplist.Remove(gid);
            }
        }

        static List<int> needRemove = new List<int>();
        public static void Update(float time)
        {
            if (needRemove.Count > 0) needRemove.Clear();

            for (int i = 0; i < maplist.mList.Count; i++)
            {
                int key = maplist.GetKey(i);
                FtDelyCall ftDay = maplist.GetValue(i);
                if (time >= ftDay.callTime)
                {
                    try
                    {
                        ftDay.callFun.Invoke();//调用
                    }
                    catch (Exception e)
                    {
                        //maplist.Remove(map.Key);
                        Debug.Log(e.Message);
                        needRemove.Add(key);
                    }

                    if (ftDay.loop == true)
                    {
                        ftDay.callTime = time + ftDay.intervalTime;//设置下一次时间
                    }
                    else
                    {
                        --ftDay.loopNum;
                        if (ftDay.loopNum > 0)
                        {
                            ftDay.callTime = time + ftDay.intervalTime;//设置下一次时间
                        }
                        else
                        {
                            //自动移除
                            // maplist.Remove(map.Key);
                            needRemove.Add(key);
                        }
                    }
                }
            }


            for(int i = 0;i< needRemove.Count;i++)
            {
                maplist.Remove(needRemove[i]);
            }
        }
    }
}
