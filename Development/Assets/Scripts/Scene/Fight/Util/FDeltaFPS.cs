using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Events;


public class FDeltaFPS
{

    private static FDeltaFPS m_Instance;

    public static FDeltaFPS Instance { get { if (m_Instance == null) m_Instance =new FDeltaFPS(); return m_Instance; } }

    public UnityAction OnEvent;

    public float UpdateInterval = 0.1f;//更新周期
    public float Accum = 0f;
    public int Frames = 0;
    public float Timeleft = 0f;

    public float Fps = 0f;//当前帧率

    protected int updateNum = 0;
    protected float FrameLowTime = 0f;
    public int LowFrame = 15;
    private void CallChangeEvent()
    {
        if (this.OnEvent != null) this.OnEvent.Invoke();
    }

    private void Reset()
    {
        Timeleft = UpdateInterval;
        Accum = 0.0f;
        Frames = 0;
    }

    public void UpdateEx(float deltaTime, float time)
    {
        Timeleft = Timeleft - deltaTime;
        float rrr = Time.timeScale / deltaTime;
        Accum += rrr;
        Frames += 1;

        if (Timeleft <= 0)
        {
            Fps = Mathf.Floor(Accum/Frames);

            Reset();
        }

        if (Fps < LowFrame)
        {
            if (this.FrameLowTime == 0f) FrameLowTime = Time.unscaledTime;
            if (Time.unscaledTime - this.FrameLowTime >= 8)
            {
                this.FrameLowTime = 0f;
                CallChangeEvent();
            }
        }
        else
        {
            this.FrameLowTime = 0f;
        }
            
    }
 }
