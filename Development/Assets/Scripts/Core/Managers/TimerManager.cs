using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TimerManager : MonoBehaviour
{
    private static Dictionary<int, Timer> s_AllTimer = new Dictionary<int, Timer>();
    public Dictionary<int, Timer> AllTimer { get { return s_AllTimer; } }
    private static Dictionary<int, Timer> s_RemoveAwait = new Dictionary<int, Timer>();
    private static Dictionary<int, Timer> s_AddAwait = new Dictionary<int, Timer>();
    public delegate void Tick();

    private static TimerManager s_TimerManager;
    static bool InitTimerManager()
    {
        if (s_TimerManager == null)
        {
            if (!Application.isPlaying) return false;
            GameObject go = new GameObject("TimerManager");
            s_TimerManager = go.AddComponent<TimerManager>();
            go.hideFlags = HideFlags.HideInHierarchy;
            Object.DontDestroyOnLoad(go);
        }
        return true;
    }

    static int AddTimer(Timer.TimerType tType, Tick tick, float interval, float delay = 1, int durationCount = 1, bool runback = true)
    {
        if (!InitTimerManager()) return -1;
        Timer timer = new Timer().SetData(tType, tick, interval, delay, durationCount, runback);
        s_AddAwait.Add(timer.tid, timer);
        return timer.tid;
    }



    public static int AddFrame(Tick tick, float interval = 1, int durationCount = 1, float delay = 1, bool runback = true)
    {
        return AddTimer(Timer.TimerType.FRAME, tick, interval, delay, durationCount, runback);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tick">回调</param>
    /// <param name="interval">间隔/s</param>
    /// <param name="durationCount">次数 -1无限</param>
    /// <param name="delay">延迟</param>
    /// <param name="runback">挂起执行 </param>
    /// <returns></returns>

    public static int AddTimer(Tick tick, float interval = 1, int durationCount = 1, float delay = 0, bool runback = true)
    {
        return AddTimer(Timer.TimerType.NORM, tick, interval, delay, durationCount, runback);
    }

    public static void DelTimer(int id)
    {
        Timer timer;
        if (s_AllTimer.TryGetValue(id, out timer) && !s_RemoveAwait.ContainsKey(id))
            s_RemoveAwait.Add(id, timer);

        if (s_AddAwait.ContainsKey(id))
            s_AddAwait.Remove(id);
    }

    public static int ResetStart(int id)
    {
        Timer timer;
        if (s_AllTimer.TryGetValue(id, out timer))
            return timer.ResetStart().tid;
        return -1;
    }

    //启协协程
    public static Coroutine AddCoroutine(IEnumerator routine)
    {
        if (!InitTimerManager()) return null;
        return s_TimerManager.StartCoroutine(routine);
    }

    void Update()
    {
        if (s_AddAwait.Count > 0)
        {
            foreach (var item in s_AddAwait)
                s_AllTimer.Add(item.Key, item.Value);
            s_AddAwait.Clear();
        }


        if (s_RemoveAwait.Count > 0)
        {
            foreach (var item in s_RemoveAwait)
            {
                item.Value.Clear();
                s_AllTimer.Remove(item.Key);
            }

            s_RemoveAwait.Clear();

        }

        foreach (var item in s_AllTimer)
        {
            item.Value.Update();
            if (item.Value.isDestory && !s_RemoveAwait.ContainsKey(item.Key))
                s_RemoveAwait.Add(item.Key, item.Value);
        }
    }

    void OnDestroy()
    {
        foreach (var item in s_AllTimer)
            item.Value.Clear();
        s_AllTimer.Clear();
    }




    public class Timer
    {
        static int s_tid = 99;
        public enum TimerType { NORM, FRAME, }

        private int m_CurrentCount = 0;
        private int m_NextFrame = -1;
        private int m_Tid = ++s_tid;

        private float m_NextTime = -1;
        private float m_StartTime = 0;
        private float m_Delay;
        private float m_Interval;
        private int m_DurationCount;
        private bool m_IsDestory;
        private bool m_RunBackground;

        private Tick m_Tick;
        private TimerType m_TType;
        public int tid { get { return m_Tid; } }
        public float delay { get { return m_Delay; } }
        public float interval { get { return m_Interval; } }
        public bool isDestory { get { return m_IsDestory; } }
        public int durationCount { get { return m_DurationCount; } }
        public TimerType type { get { return m_TType; } }

        public Timer SetData(TimerType tType, Tick tick, float interval = 1, float delay = 0, int durationCount = 1, bool runback = true)
        {
            this.m_Tick = tick;
            this.m_TType = tType;
            this.m_Delay = delay;
            this.m_Interval = Mathf.Max(0, interval);
            this.m_DurationCount = durationCount;
            this.m_NextFrame = Time.frameCount + (int)Mathf.Max(0, delay);
            this.m_StartTime = Time.realtimeSinceStartup;
            this.m_NextTime = this.m_StartTime + (delay == 0 ? this.m_Interval : delay);
            this.m_IsDestory = false;
            this.m_RunBackground = runback;
            return this;
        }

        public Timer ResetStart()
        {
            return SetData(this.m_TType, this.m_Tick, this.m_Interval, this.m_Delay, this.m_DurationCount, this.m_RunBackground);
        }

        public void Update()
        {
            if (m_TType == Timer.TimerType.FRAME)
            {
                if (Time.frameCount >= this.m_NextFrame)
                {
                    this.m_CurrentCount++;
                    this.m_NextFrame += (int)this.interval;
                    this.OnTick();
                }
            }
            else
            {
                float realtimeSinceStartup = Time.realtimeSinceStartup;
                if (realtimeSinceStartup >= this.m_NextTime)
                {
                    this.m_CurrentCount++;
                    this.m_NextTime = this.interval + realtimeSinceStartup;
                    this.OnTick();

                    if (this.m_DurationCount > 0 && m_RunBackground)
                        if (realtimeSinceStartup >= this.m_StartTime + (this.interval * this.durationCount) + delay)
                            this.m_IsDestory = true;
                }
            }

            if (this.m_DurationCount > 0 && this.m_CurrentCount >= this.m_DurationCount)
                this.m_IsDestory = true;
        }

        public void OnTick()
        {
            UnityEngine.Profiling.Profiler.BeginSample("TimerManager.OnTick");
            if (null != this.m_Tick) this.m_Tick.Invoke();
            UnityEngine.Profiling.Profiler.EndSample();
        }

        public void Clear()
        {
            this.m_Tick = null;
        }
    }
}