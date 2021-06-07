using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XGUI
{
    public class XProgressSize : MonoBehaviour
    {
        public class ProgressRenderer
        {
            public Vector3 waitVer;
            public bool isAction;

            public ProgressRenderer(Vector3 Ver, bool action = false)
            {
                waitVer = Ver;
                isAction = action;
            }
        }

        public Image progress_img;
        public Image bg_img;
        public Text label;
        public int interval = 0;
        public bool isShowMax = true;

        public PlayType m_playType = PlayType.WAIT_PRE_FINISH;
        [HideInInspector]
        public Ease m_easeType = Ease.InExpo;

        public Direction driction = Direction.HORIZONTAL;
        public LabeStyle labeStyle = LabeStyle.Value;

        public enum PlayType : int
        {
            NONE,//没有动画
            WAIT_PRE_FINISH,//等待上一次播放完毕
            COVER,           //直接覆盖
            OVERFLOW,        //溢出
        }


        public enum Direction
        {
            HORIZONTAL,
            VERTICAL,
        }

        public enum LabeStyle
        {
            Normal,
            Value
        }

        public Action onProgress;
        public Action onProgressEnd;
        public Action onProgressDone;//全部播放完毕

        private float m_curVal = 0;// 当前值

        public float m_duration = 0;//tween时间
        private string labStr = string.Empty; //按照进度文本该格式模式走

        [HideInInspector]
        public float m_maxValue = 100;

        [HideInInspector]
        public float m_value = 0;

        private List<ProgressRenderer> waitList = new List<ProgressRenderer>();

        private Vector2 osizeDelta; //原始长度
        private RectTransform imgRect;
        private RectTransform content;
        private Tween m_Tween;
        private bool validDraw = false;
        private void Awake()
        {
            InitUI();

            this.InitValue(m_value, m_maxValue, m_duration);
        }

        public void InitUI()
        {
            if (imgRect != null) return;

            content = gameObject.GetComponent<RectTransform>();
            imgRect = progress_img.GetComponent<RectTransform>();
            InitAnchore(imgRect, false);
            if (bg_img != null) InitAnchore(bg_img.rectTransform);

            //if (label != null) InitAnchore(label.rectTransform);

            osizeDelta = new Vector2(content.sizeDelta.x - interval, content.sizeDelta.y - interval);
            labStr = string.Empty;
        }

        private void InitAnchore(RectTransform tRect, bool full = true)
        {
            if (full)
            {
                tRect.anchorMin = new Vector2(0, 0);
                tRect.anchorMax = new Vector2(1, 1);
                tRect.sizeDelta = Vector2.zero;
            }
            else
            {
                tRect.anchorMin = new Vector2(0, 1);
                tRect.anchorMax = new Vector2(0, 1);
            }

            tRect.pivot = new Vector2(0, 1);
        }

        //初始化数据
        public void InitValue(float tarVal, float maxVal, float duration = 0)
        {
            tarVal = Math.Max(0, Math.Min(tarVal, maxVal));
            this.m_maxValue = maxVal;
            this.m_value = tarVal;
            this.m_curVal = tarVal;

            this.DrawUI();
        }

        //初始化文本格式
        public void SetText(string str)
        {
            labStr = str;
        }

        //有动画
        public void SetValue(float tarVal, float maxVal, float duration = 0, bool isAction = false)
        {
            tarVal = Math.Max(0, Math.Min(tarVal, maxVal));

            if (maxVal == 0)
            {
                if (tarVal > 0)
                {
                    maxVal = tarVal;
                }
                else
                {
                    maxVal = 1;
                }
            }

            if (m_playType == PlayType.COVER)
            {
                //直接播放最新
                CleanWait();
                StopAnim();
                waitList.Add(new ProgressRenderer(new Vector3(tarVal, maxVal, duration),isAction));
                this.validDraw = true;
            }
            else if (m_playType == PlayType.WAIT_PRE_FINISH)
            {
                //一个一次播放
                if (duration == 0)
                {
                    CleanWait();
                    StopAnim();
                    InitValue(tarVal, maxVal, duration);
                    this.validDraw = false;
                    return;
                }
                for (int i = 0; i < waitList.Count; i++)
                {
                    if (waitList[i].waitVer.x == tarVal && waitList[i].waitVer.y == maxVal) return;
                }
                waitList.Add(new ProgressRenderer(new Vector3(tarVal, maxVal, duration), isAction));
                this.validDraw = true;
            }
            else
            {
                InitValue(tarVal, maxVal, duration);
            }


        }

        public void CleanWait()
        {
            waitList.Clear();
        }

        private void DrawUI()
        {
            float rate = m_curVal / m_maxValue;
            Vector2 nSize = new Vector2(1, 1);
            if (driction == Direction.HORIZONTAL)
            {
                nSize.x = rate * osizeDelta.x;
                nSize.y = imgRect.sizeDelta.y;
            }
            else
            {
                nSize.x = imgRect.sizeDelta.x;
                nSize.y = rate * osizeDelta.y;
            }

            imgRect.sizeDelta = nSize;

            if (label)
            {
                if (labeStyle == LabeStyle.Normal)
                {
                    label.text = (rate * 100).ToString("0.0") + "%";
                }
                else if (labeStyle == LabeStyle.Value)
                {
                    if (string.IsNullOrEmpty(labStr))
                    {
                        label.text = string.Format("{0}/{1}", this.m_curVal, m_maxValue);
                        if (this.m_curVal == m_maxValue && isShowMax)
                        {
                            label.text = "Max";
                        }
                    }
                    else
                    {
                        label.text = string.Format(labStr, this.m_curVal, m_maxValue);
                    }
                }
            }

        }

        void Update()
        {
            if (validDraw && !isProRun)
            {
                runProgress();
            }

            validDraw = false;
        }


        public void StopAnim()
        {
            if (m_Tween != null)
            {
                m_Tween.Kill();
            }
            isProRun = false;
        }

        private bool isProRun = false;
        private void runProgress()
        {
            StopAnim();

            if (waitList.Count < 0) return;

            isProRun = true;

            ProgressRenderer curDe = waitList[0];
            waitList.RemoveAt(0);
            this.m_value = curDe.waitVer.x;
            this.m_maxValue = curDe.waitVer.y;
            this.m_duration = curDe.waitVer.z;

            //Debug.Log("需要到达的进度" + m_value + "总进度：" + m_maxValue + "当前所在进度：" + m_curVal);

            if (m_curVal > m_value) m_curVal = 0;//不能倒退

            m_Tween = DOTween.To(() =>
           {
               return this.m_curVal;
           }, (v) =>
           {
               m_curVal = Mathf.Floor(v);

               DrawUI();

               if (onProgress != null)
                   onProgress.Invoke();

           }, m_value, m_duration);

            m_Tween.SetEase(m_easeType);

            m_Tween.OnComplete(() =>
            {
                //Debug.Log("OnComplete需要到达的进度" + m_value + "总进度：" + m_maxValue + "当前所在进度：" + m_curVal);
                if (m_curVal == m_maxValue)
                {
                    m_curVal = 0;
                }
                if (waitList.Count > 0)
                {
                    if (onProgressEnd != null && curDe.isAction) onProgressEnd.Invoke();
                    runProgress();
                }
                else
                {
                    isProRun = false;
                    if (onProgressDone != null) onProgressDone.Invoke();
                }
            });
        }

        private void OnDestroy()
        {
            onProgress = null;
            onProgressEnd = null;
            onProgressDone = null;
        }
    }
}
