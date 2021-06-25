using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game.MScene
{
    public class MapftCallUtil
    {
        public class FFramCall
        {
            public int id;
            public bool safe;
            public int nextframeNum;
            public int type = 1;
            public ActionX<float, float> callFun;
        }

        private  List<FFramCall> maplist = new List<FFramCall>();
        private  int localIndex = 1;
        //获取本地唯一ID
        public  int GetLocalId()
        {
            localIndex++;
            if (localIndex == 0) localIndex++;

            return localIndex;
        }

        public int AddEvent(ActionX<float, float> actio,bool safe = true)
        {
            if (actio == null) return 0;

            int gid = GetLocalId();

            FFramCall call = new FFramCall();
            call.id = gid;
            call.callFun = actio;
            call.safe = safe;
            call.type = 1;
            maplist.Add(call);

            return gid;
        }

        public  int NextFrameCall(ActionX<float, float> actio)
        {
            if (actio == null) return 0;

            int gid = GetLocalId();

            FFramCall call = new FFramCall();
            call.id = gid;
            call.callFun = actio;
            call.safe = true;
            call.type = 2;
            call.nextframeNum = Time.frameCount + 1;
            maplist.Add(call);

            return gid;
        }

        public  void Remove(int gid)
        {
            if (gid == 0) return;
            for(int i = 0;i< maplist.Count;i++)
            {
                if(maplist[i].id == gid)
                {
                    maplist.RemoveAt(i);
                    break;
                }
            }
        }

        public  void Remove2(ActionX<float, float> actio)
        {
            for (int i = 0; i < maplist.Count; i++)
            {
                if (maplist[i].callFun == actio)
                {
                    maplist.RemoveAt(i);
                    i--;
                }
            }
        }

       public  void Update(float deltaTime, float time)
        {
    
            for (int i = 0; i < maplist.Count; i++)
            {
                if(maplist[i].safe)
                {
                    try
                    {
                        if(maplist[i].type == 1)
                            maplist[i].callFun(deltaTime, time);
                        else if(maplist[i].type == 2)
                        {
                            if(Time.frameCount > maplist[i].nextframeNum)
                            {
                                maplist[i].callFun(deltaTime, time);
                                maplist.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    catch (Exception e)
                    {

                        maplist.RemoveAt(i);
                        i--;
                        UnityEngine.Debug.Log("[FightCallUtil]  Update -> invoke error" + e.Message + "\n" + e.StackTrace);


                    }
                }
                else
                    maplist[i].callFun(deltaTime, time);
            }
        }
    }
}
