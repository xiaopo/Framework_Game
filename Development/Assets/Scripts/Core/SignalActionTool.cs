
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// 启动流程节奏辅助
/// </summary>
public class SignalActionTool
{
    //启动节奏信号

    private static Dictionary<string, List<UnityAction>> SignalEvents = new Dictionary<string, List<UnityAction>>(10);
    private static Dictionary<string, float> SignalMarks = new Dictionary<string, float>(10);
    /// <summary>
    /// 启动一个信号
    /// </summary>
    /// <param name="time"></param>
    public static void Single(float time,string singalName)
    {
        float SignalMark  = Time.time + time;
        SignalMarks.Remove(singalName);
        SignalMarks.Add(singalName, SignalMark);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fun"></param>
    public static void Check(UnityAction fun, string singalName)
    {
        float SignalMark = -1;
        if (SignalMarks.TryGetValue(singalName,out SignalMark))
        {
            if(SignalMark != -1)
            {
                if(!SignalEvents.TryGetValue(singalName, out List<UnityAction> list))
                {
                    list = new List<UnityAction>();
                    SignalEvents.Add(singalName, list);
                }

                list.Add(fun);

                return;//跳出
            }
        }

        fun.Invoke();
    }

    protected static List<string> templist = new List<string>(10);
    public static void Update(float time)
    {
        foreach(var map in SignalMarks)
        {
            float SignalMark = map.Value;
            if (SignalMark != -1)
            {
                if (time > SignalMark)
                {
                    if(SignalEvents.TryGetValue(map.Key, out List<UnityAction> list))
                    {
                        foreach (var fun in list) fun.Invoke();
                        list.Clear();
                    }
                   
                    templist.Add(map.Key);
                }
            }
        }

        foreach(var key in templist)
        {
            SignalMarks.Remove(key);
            SignalEvents.Remove(key);
        }
        
    }
}